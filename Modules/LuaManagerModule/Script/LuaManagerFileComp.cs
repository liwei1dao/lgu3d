using LuaInterface;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// lua模块文件管理组件
    /// </summary>
    public class LuaManagerFileComp : ModelCompBase<LuaManagerModule>
    {

        protected Dictionary<string, string> searchPaths = new Dictionary<string, string>();
        protected Dictionary<string, AssetBundle> zipMap = new Dictionary<string, AssetBundle>();

        #region 框架构造
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            searchPaths = new Dictionary<string, string>();
            zipMap = new Dictionary<string, AssetBundle>();
            base.Load(_ModelContorl);
            base.LoadEnd();
        }

        public override void Close()
        {
            searchPaths.Clear();
            foreach (KeyValuePair<string, AssetBundle> iter in zipMap)
            {
                iter.Value.Unload(true);
            }
            zipMap.Clear();
            base.Close();
        }
        #endregion

//        public void AddLuaModel(string ModelName)
//        {
//#if UNITY_EDITOR
//            string luaPath = Path.Combine(Path.Combine(AppConfig.AppLuaAddress, ModelName), "Lua");
//            searchPaths[ModelName] = luaPath;
//#else
//            string luaPath = Path.Combine(AppConfig.AppLuaAddress, Path.Combine(ModelName, "Lua" + AppConfig.ResFileSuffix).ToLower());
//            AssetBundle abundle = AssetBundle.LoadFromFile(luaPath);
//            if (abundle != null)
//            {
//                zipMap[ModelName] = abundle;
//            }
//            else
//            {
//                LoggerHelper.Error(ModelName + "No Lua AssetBundle:" + luaPath);
//            }
//#endif

//        }

//        public void RemoveLuaModel(string ModelName)
//        {
//#if UNITY_EDITOR
//            if (searchPaths.ContainsKey(ModelName))
//            {
//                searchPaths.Remove(ModelName);
//            }
//#else
//            if (zipMap.ContainsKey(ModelName))
//            {
//                zipMap.Remove(ModelName);
//            }
//#endif
//        }


        public string FindFile(string ModelFileName)
        {
            string ModelName = ModelFileName.Substring(0, ModelFileName.IndexOf("/"));
            string FileName = ModelFileName.Substring(ModelName.Length + 1);
            FileName = (ModelName + "/Lua/" + FileName + ".lua").ToLower();
            return FileName;
        }

        public byte[] ReadFile(string ModelFileName)
        {
            string ModelName = ModelFileName.Substring(0, ModelFileName.IndexOf("/"));
            string FileName = ModelFileName.Substring(ModelName.Length + 1);
            return ReadFile(ModelName, FileName);
        }

        public byte[] ReadFile(string ModelName, string FileName)
        {
            return ResourceModule.Instance.LoadLuaFile(ModelName, "Lua", FileName);
        }

        public byte[] ReadProtoFile(string ModelFileName)
        {
            string ModelName = ModelFileName.Substring(0, ModelFileName.IndexOf("/"));
            string FileName = ModelFileName.Substring(ModelName.Length + 1);
            return ReadProtoFile(ModelName, FileName);
        }
        public byte[] ReadProtoFile(string ModelName, string FileName)
        {
            return ResourceModule.Instance.LoadProtoFile(ModelName, "Pb", FileName);
        }
    }
}
