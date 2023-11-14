#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using lgu3d.Editor;

namespace lgu3d
{
  public class EditorResourComp : ModelCompBase<ResourceModule>
  {
    private Dictionary<string, Dictionary<string, UnityEngine.Object>> Assets;
    public PackingConfig Config;
    public override void Load(ModuleBase _ModelContorl, params object[] agrs)
    {
      base.Load(_ModelContorl, agrs);
      Assets = new Dictionary<string, Dictionary<string, Object>>();
      string ConfigPath = Path.Combine(ToolsConfig.RelativeEditorResources, "PackingToolsSetting.asset");
      Config = AssetDatabase.LoadAssetAtPath<PackingConfig>(ConfigPath);
      if (Config == null)
      {
        Debug.LogError("资源编译配置文件没有生成 PackingConfig = null");
      }
      LoadEnd();
    }

    public override void Start(params object[] agrs)
    {
      base.Start(agrs);
      MyModule.LoadViewComp?.UpdateProgress(1, "资源初始化完毕!");
    }
    public byte[] LoadByteFile(string ModelName, string BundleName, string AssetName)
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();
      string Key = string.Empty;
      if (AssetName != null)
        Key = BundleName + "/" + AssetName;
      else
        Key = BundleName;

      if (!Assets.ContainsKey(ModelName))
      {
        Assets[ModelName] = new Dictionary<string, UnityEngine.Object>();
      }
      if (Assets[ModelName].ContainsKey(Key))
      {
        TextAsset luaCode = Assets[ModelName][Key] as TextAsset;
        if (luaCode != null)
        {
          return luaCode.bytes;
        }
        else
        {
          Debug.LogError(ModelName + "No Find :" + Key);
        }
      }
      else
      {
        for (int i = Config.ResourceCatalog.Count - 1; i >= 0; i--)
        {
          string _Path = Path.Combine(Application.dataPath + Config.ResourceCatalog[i].Path, ModelName + "/" + BundleName);
          if (PathTools.IsDirectory(_Path))
          {
            _Path += "/" + AssetName + ".bytes";
            if (File.Exists(_Path))
            {
              byte[] FileBytes = File.ReadAllBytes(_Path);
              return FileBytes;
            }
          }
        }
      }
      Debug.LogError("No Find Lua File:" + ModelName + "/" + Key);
      return null;
    }

    public byte[] LoadLuaFile(string ModelName, string BundleName, string AssetName)
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();
      string Key = string.Empty;
      if (AssetName != null)
        Key = BundleName + "/" + AssetName;
      else
        Key = BundleName;

      if (!Assets.ContainsKey(ModelName))
      {
        Assets[ModelName] = new Dictionary<string, UnityEngine.Object>();
      }
      if (Assets[ModelName].ContainsKey(Key))
      {
        TextAsset luaCode = Assets[ModelName][Key] as TextAsset;
        if (luaCode != null)
        {
          return luaCode.bytes;
        }
        else
        {
          Debug.LogError(ModelName + "No Find :" + Key);
        }
      }
      else
      {
        for (int i = Config.ResourceCatalog.Count - 1; i >= 0; i--)
        {
          string _Path = Path.Combine(Application.dataPath + Config.ResourceCatalog[i].Path, ModelName + "/" + BundleName);
          if (PathTools.IsDirectory(_Path))
          {
            _Path += "/" + AssetName + ".lua";
            if (File.Exists(_Path))
            {
              byte[] FileBytes = File.ReadAllBytes(_Path);
              return FileBytes;
            }
          }
        }
      }
      Debug.LogError("No Find Lua File:" + ModelName + "/" + Key);
      return null;
    }

    public byte[] LoadProtoFile(string ModelName, string BundleName, string AssetName)
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();
      string Key = string.Empty;
      if (AssetName != null)
        Key = BundleName + "/" + AssetName;
      else
        Key = BundleName;

      if (!Assets.ContainsKey(ModelName))
      {
        Assets[ModelName] = new Dictionary<string, UnityEngine.Object>();
      }
      if (Assets[ModelName].ContainsKey(Key))
      {
        TextAsset luaCode = Assets[ModelName][Key] as TextAsset;
        if (luaCode != null)
        {
          return luaCode.bytes;
        }
        else
        {
          Debug.LogError(ModelName + "No Find :" + Key);
        }
      }
      else
      {
        for (int i = Config.ResourceCatalog.Count - 1; i >= 0; i--)
        {
          string _Path = Path.Combine(Application.dataPath + Config.ResourceCatalog[i].Path, ModelName + "/" + BundleName);
          if (PathTools.IsDirectory(_Path))
          {
            _Path += "/" + AssetName + ".proto";
            if (File.Exists(_Path))
            {
              byte[] FileBytes = File.ReadAllBytes(_Path);
              return FileBytes;
            }
          }
        }
      }
      Debug.LogError("No Find Lua File:" + ModelName + "/" + Key);
      return null;
    }

    public T LoadAsset<T>(string ModelName, string BundleName, string AssetName) where T : UnityEngine.Object
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();

      string Key = string.Empty;
      if (AssetName != null)
        Key = BundleName + "/" + AssetName;
      else
        Key = BundleName;

      if (!Assets.ContainsKey(ModelName))
      {
        Assets[ModelName] = new Dictionary<string, UnityEngine.Object>();
      }
      if (Assets[ModelName].ContainsKey(Key))
      {
        return Assets[ModelName][Key] as T;
      }
      else
      {
        for (int i = Config.ResourceCatalog.Count - 1; i >= 0; i--)
        {
          string _Path = Path.Combine(Application.dataPath + Config.ResourceCatalog[i].Path, ModelName + "/" + BundleName);
          if (PathTools.IsDirectory(_Path))
          {
            string FileName = GetAssetPathToPath(_Path, AssetName);
            if (FileName != string.Empty)
            {
              T obj = AssetDatabase.LoadAssetAtPath<T>(FileName);
              Assets[ModelName][Key] = obj;
              return obj;
            }
          }
          else
          {
            string dir = PathTools.GetDirectory(_Path);
            if (PathTools.IsDirectory(dir))
            {
              string assetName = _Path.Substring(dir.Length + 1);
              string FileName = GetAssetPathToPath(dir, assetName);
              if (FileName != string.Empty)
              {
                T obj = AssetDatabase.LoadAssetAtPath<T>(FileName);
                Assets[ModelName][Key] = obj;
                return obj;
              }
            }
          }

        }
      }
      return null;
    }

    public T[] LoadAllAsset<T>(string ModelName, string BundleName, string AssetName) where T : UnityEngine.Object
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();

      string Key = string.Empty;
      if (AssetName != null)
        Key = BundleName + "/" + AssetName;
      else
        Key = BundleName;

      if (!Assets.ContainsKey(ModelName))
      {
        Assets[ModelName] = new Dictionary<string, UnityEngine.Object>();
      }


      for (int i = Config.ResourceCatalog.Count - 1; i >= 0; i--)
      {
        string _Path = Path.Combine(Application.dataPath + Config.ResourceCatalog[i].Path, ModelName + "/" + BundleName);
        if (PathTools.IsDirectory(_Path))
        {
          string FileName = GetAssetPathToPath(_Path, AssetName);
          if (FileName != string.Empty)
          {
            List<T> ret = new List<T>();
            Object[] obj = AssetDatabase.LoadAllAssetsAtPath(FileName);
            if (obj != null)
            {
              foreach (var item in obj)
              {
                if (item is T)
                {
                  ret.Add(item as T);
                }
              }
            }
            return ret.ToArray();
          }
        }
        else
        {
          string dir = PathTools.GetDirectory(_Path);
          if (PathTools.IsDirectory(dir))
          {
            string assetName = _Path.Substring(dir.Length + 1);
            string FileName = GetAssetPathToPath(dir, assetName);
            if (FileName != string.Empty)
            {
              List<T> ret = new List<T>();
              Object[] obj = AssetDatabase.LoadAllAssetsAtPath(FileName);
              if (obj != null)
              {
                foreach (var item in obj)
                {
                  if (item is T)
                  {
                    ret.Add(item as T);
                  }
                }
              }
              return ret.ToArray();
            }
          }
        }
      }
      return null;
    }
    /// <summary>
    /// 獲取目錄下的資源文件
    /// </summary>
    /// <param name="_Directory"></param>
    /// <param name="_FileName"></param>
    /// <returns></returns>
    private string GetAssetPathToPath(string _Directory, string _FileName)
    {
      string[] fileList = Directory.GetFileSystemEntries(_Directory);
      foreach (string file in fileList)
      {
        if (File.Exists(file))
        {
          string FileName = PathTools.GetPathFolderName(file);
          FileName = FileName.Substring(0, FileName.IndexOf(".")).ToLower();
          if (FileName == _FileName)
          {
            return file.Substring(Application.dataPath.Length - "Assets".Length); ;
          }
        }
        if (Directory.Exists(file))
        {
          string result = GetAssetPathToPath(file, _FileName);
          if (result != string.Empty)
          {
            return result;
          }
        }
      }
      return string.Empty;
    }


    #region 卸載資源

    public void UnloadAsset(string ModelName, string BundleName, string AssetName)
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();
      if (Assets.ContainsKey(ModelName))
      {
        string Key = BundleName + "/" + AssetName;
        //if (Assets[ModelName].ContainsKey(Key))
        //{
        //    Resources.UnloadAsset(Assets[ModelName][Key]);
        //}
        Resources.UnloadUnusedAssets();
        Assets[ModelName].Remove(Key);
      }
    }

    public void UnloadModel(string ModelName)
    {
      ModelName = ModelName.ToLower();
      if (Assets.ContainsKey(ModelName))
      {
        //foreach (var item in Assets[ModelName])
        //{
        //    Resources.UnloadAsset(item.Value);
        //}
        Resources.UnloadUnusedAssets();
        Assets.Remove(ModelName);
      }
    }

    #endregion
  }
}
#endif