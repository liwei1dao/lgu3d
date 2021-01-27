using UnityEngine;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace lgu3d.Editor
{
    //打包选项
    public enum BuildSwitchType
    {
        OnlyApp,                                        //仅编译App
        OnlyModule,                                     //仅编译模块
        AppAndModule,                                   //APP和模块一起编译
    }

    public enum ResourceBuildNodelType
    {
        UselessNodel,            //无用文件
        ModelNodel,              //模块节点
        FolderNodel,             //文件夹节点
        ResourcesItemNodel,      //资源文件节点
    }

    [System.Serializable]
    public class ResourceCatalog
    {
        public string Name;
        public string Path;
    }

    [System.Serializable]
    public class ResourceModelConfig : ResourceItemConfig
    {
        public bool IsSelection = true;
        public bool IsUpdataModule = false;
        public ResourceModelConfig(string _RootName, string _Path)
            :base(_RootName,null,_Path)
        {
            if (NodelType == ResourceBuildNodelType.FolderNodel)
            {
                NodelType = ResourceBuildNodelType.ModelNodel;
                AssetBundleName = Name;
                ModelName = Name;
                IsMergeBuild = false;
                RefreshChildViewGUI();
            }
        }

        public void MergeConfig(ResourceModelConfig other)
        {
            for (int i = 0; i < other.ChildBuildItem.Count; i++)
            {
                ResourceItemConfig item1 = other.ChildBuildItem[i];
                bool IsKeep = false;
                for (int j = 0; j < ChildBuildItem.Count; j++)
                {
                    ResourceItemConfig item2 = ChildBuildItem[j];
                    if (item1.Name == item2.Name) {
                        if (item2.NodelType == ResourceBuildNodelType.FolderNodel)
                        {
                            item2.MergeConfig(item1);
                        }
                        else
                        {
                            ChildBuildItem[j] = item1;          
                        }
                        IsKeep = true;
                        break;
                    }
                }
                if (!IsKeep)
                {
                    ChildBuildItem.Add(item1);
                }
            }
        }

        public override void OnGUI()
        {
#if UNITY_EDITOR
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel = Layer;
            IsSelection = EditorGUILayout.Toggle(IsSelection, GUILayout.Width(10));
            Foldout = EditorGUILayout.Foldout(Foldout, Name);
            EditorGUILayout.Space();
            IsUpdataModule = GUILayout.Toggle(IsUpdataModule, "IsUpData");
            EditorGUILayout.EndHorizontal();
            if (Foldout)
            {
                foreach (var item in ChildBuildItem)
                {
                    item.OnGUI();
                }
            }
#endif
        }
    }

    [System.Serializable]
    public class ResourceItemConfig
    {
        public string Name;
        public string Path;
        public string RootName;
        public string ModelName;
        public string AssetBundleName;
        public int Layer;
        public ResourceBuildNodelType NodelType;
        public bool Foldout = false;
        public bool IsMergeBuild = true;
        public bool IsShowBuildToogle = true;
        public List<ResourceItemConfig> ChildBuildItem;

        public ResourceItemConfig(string _RootName, ResourceItemConfig _Parent, string _Path)
        {
            RootName = _RootName;
            Path = _Path;
            Name = PathTools.GetPathFolderName(Path);
            ModelName = _Parent == null ? Name : _Parent.ModelName;
            Layer = _Parent == null ? 0 : _Parent.Layer + 1; 
            ChildBuildItem = new List<ResourceItemConfig>();
            if (PathTools.IsDirectory(Application.dataPath + Path))
            {
                string[] fileList = Directory.GetFileSystemEntries(Application.dataPath + Path);
                foreach (string file in fileList)
                {
                    string _file = file.Substring(Application.dataPath.Length, file.Length - Application.dataPath.Length);
                    ResourceItemConfig Item = new ResourceItemConfig(RootName,this, _file);
                    if (Item.NodelType != ResourceBuildNodelType.UselessNodel)
                    {
                        ChildBuildItem.Add(Item);
                    }
                }
                if (ChildBuildItem.Count > 0)
                {
                    NodelType = ResourceBuildNodelType.FolderNodel;
                }
                else
                {
                    NodelType = ResourceBuildNodelType.UselessNodel;    
                }
            }
            else
            {
                if (PathTools.CheckSuffix(Path, ToolsConfig.CanBuildFileTypes))
                {
                    Name = Name.Substring(0, Name.IndexOf("."));
                    NodelType = ResourceBuildNodelType.ResourcesItemNodel;
                }
                else
                {
                    NodelType = ResourceBuildNodelType.UselessNodel;
                }
            }
        }

        /// <param name="_Other"></param>
        public void MergeConfig(ResourceItemConfig other)
        {
            for (int i = 0; i < other.ChildBuildItem.Count; i++)
            {
                ResourceItemConfig item1 = other.ChildBuildItem[i];
                bool IsKeep = false;
                for (int j = 0; j < ChildBuildItem.Count; j++)
                {
                    ResourceItemConfig item2 = ChildBuildItem[j];
                    if (item1.Name == item2.Name)
                    {
                        if (item2.NodelType == ResourceBuildNodelType.FolderNodel)
                        {
                            item2.MergeConfig(item1);
                        }
                        else
                        {
                            ChildBuildItem[j] = item1;      //覆盖
                        }
                        IsKeep = true;
                        break;
                    }
                }
                if (!IsKeep)
                {
                    ChildBuildItem.Add(item1);
                }
            }
        }

        /// <summary>
        /// 拆分资源列表
        /// </summary>
        /// <param name="RootName"></param>
        public void SplitConfig(string RootName)
        {
            List<ResourceItemConfig> Tmp = new List<ResourceItemConfig>();
            foreach (var item in ChildBuildItem)
            {
                if (item.NodelType == ResourceBuildNodelType.ResourcesItemNodel && item.RootName == RootName)
                {
                    Tmp.Add(item);
                }
                else
                {
                    if (item.NodelType == ResourceBuildNodelType.FolderNodel || item.NodelType == ResourceBuildNodelType.ModelNodel)
                    {
                        item.SplitConfig(RootName);
                        if (item.NodelType == ResourceBuildNodelType.UselessNodel)
                        {
                            Tmp.Add(item);
                        }
                    }

                }
            }
            foreach (var item in Tmp)
            {
                ChildBuildItem.Remove(item);
            }
            if (ChildBuildItem.Count == 0)
            {
                NodelType = ResourceBuildNodelType.UselessNodel;
            }
        }

        public void RefreshViewGUI(ResourceItemConfig Parent)
        {
            if (Parent.IsShowBuildToogle && !Parent.IsMergeBuild)
            {
                IsShowBuildToogle = true;
                IsMergeBuild = true;
                AssetBundleName = Parent.AssetBundleName +"/"+ Name;
            }
            else
            {
                IsShowBuildToogle = false;
                AssetBundleName = Parent.AssetBundleName;
            }
            RefreshChildViewGUI();
        }

        public void RefreshChildViewGUI()
        {
            foreach (var item in ChildBuildItem)
            {
                item.RefreshViewGUI(this);
            }
        }

        public virtual void OnGUI()
        {
#if UNITY_EDITOR
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            EditorGUI.indentLevel = Layer;
            if (NodelType == ResourceBuildNodelType.FolderNodel)
            {
                Foldout = EditorGUILayout.Foldout(Foldout, Name);
                EditorGUILayout.Space();
                if (IsShowBuildToogle)
                {
                    bool mIsMergeBuild = GUILayout.Toggle(IsMergeBuild, "合并");

                    if (mIsMergeBuild != IsMergeBuild)
                    {
                        IsMergeBuild = mIsMergeBuild;
                        RefreshChildViewGUI();
                    }
                }
            }
            else
            {
                EditorGUILayout.LabelField(Name);
                EditorGUILayout.Space();
                if (IsShowBuildToogle)
                {
                    IsMergeBuild = GUILayout.Toggle(IsMergeBuild, "打包");
                }
            }
            EditorGUILayout.EndHorizontal();
            if (Foldout)
            {
                foreach (var item in ChildBuildItem)
                {
                    item.OnGUI();
                }
            }
#endif
        }

        //获取编译输出文件
        public virtual string GetBuildOutFile() {
            if (!Path.EndsWith("lua") && !Path.EndsWith("proto"))
            {
                return (Application.dataPath + Path).Substring(AppConfig.PlatformRoot.Length + 1);
            }
            else {
                if (Path.EndsWith("lua")) {
                    string luabuildfile = (Application.dataPath + Path).Replace(".lua", ".txt");
                    FilesTools.CopyFile(Application.dataPath + Path, luabuildfile);
                    string outstr = luabuildfile.Substring(AppConfig.PlatformRoot.Length + 1);
                    return outstr;
                }
                else if(Path.EndsWith("proto")){
                    string protobuildfile = (Application.dataPath + Path).Replace(".proto", ".txt");
                    FilesTools.CopyFile(Application.dataPath + Path, protobuildfile);
                    string outstr = protobuildfile.Substring(AppConfig.PlatformRoot.Length + 1);
                    return outstr;
                }
            }
            return "";
        }
        /// <summary>
        /// 清理编译输出文件
        /// </summary>
        /// <returns></returns>
        public virtual void ClearBuildOutFile()
        {
            if (Path.EndsWith("lua"))
            { 
                string luabuildfile = (Application.dataPath + Path).Replace(".lua", ".txt");
                if (File.Exists(luabuildfile)) {
                    File.Delete(luabuildfile);
                }
            }else if (Path.EndsWith("proto"))
            {
                string protobuildfile = (Application.dataPath + Path).Replace(".proto", ".txt");
                if (File.Exists(protobuildfile))
                {
                    File.Delete(protobuildfile);
                }
            }
        }

    }

    public class PackingConfig : ScriptableObject
    {
        public AppPlatform BuildPlatform;
        public BuildSwitchType BuildTarget;
        public int ProVersion;
        public int ResVersion;
        public bool IsCompress;
        public string ResourceOutPath;
        public List<ResourceCatalog> ResourceCatalog;
        public List<ResourceModelConfig> ModelBuildConfig;

        public PackingConfig()
        {
            BuildPlatform = AppPlatform.Windows;
            ResourceOutPath = Application.streamingAssetsPath;
            ResourceCatalog = new List<ResourceCatalog>();
            ModelBuildConfig = new List<ResourceModelConfig>();
        }
    }
}