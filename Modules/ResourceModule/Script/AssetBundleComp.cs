using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace lgu3d
{
  /// <summary>
  /// AssetBundle 加载方式
  /// </summary>
  public class AssetBundleComp : ModelCompBase<ResourceModule>
  {
    // private bool IsHaveLoadView;
    // private GameObject LoadView;
    // private Image Progress;
    // private Text Progress1Describe;
    private Dictionary<string, Dictionary<string, AssetBundle>> Bundles;
    private Dictionary<string, Dictionary<string, UnityEngine.Object>> Assets;
    AppModuleAssetInfo ResourceInfo;


    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl, _Agr);
      // IsHaveLoadView = false;
      Bundles = new Dictionary<string, Dictionary<string, AssetBundle>>();
      Assets = new Dictionary<string, Dictionary<string, UnityEngine.Object>>();
      bool IsSucc = FilesTools.IsKeepFileOrDirectory(AppConfig.AppAssetBundleAddress + "/AssetInfo.json");
      if (IsSucc)
      {
        string jsonStr = FilesTools.ReadFileToStr(AppConfig.AppAssetBundleAddress + "/AssetInfo.json");
        ResourceInfo = LoadAppBuileInfo(jsonStr);
        // MyModule.LoadViewComp?.Hide();
        LoadEnd();
      }
      else
      {
        // if (GameObject.Find("UIRoot/HightUIRoot/LoadingView") != null)
        // {
        //   IsHaveLoadView = true;
        //   LoadView = GameObject.Find("UIRoot/HightUIRoot/LoadingView");
        //   Progress = LoadView.OnSubmit<Image>("Progress");
        //   Progress1Describe = LoadView.OnSubmit<Text>("Describe");
        // }
        Manager_ManagerModel.Instance.StartCoroutine(AssemblyAsset(() =>
        {
          string jsonStr = FilesTools.ReadFileToStr(AppConfig.AppAssetBundleAddress + "/AssetInfo.json");
          ResourceInfo = LoadAppBuileInfo(jsonStr);
          // MyModule.LoadViewComp?.Hide();
          LoadEnd();
        }));
      }
    }

    IEnumerator AssemblyAsset(Action CallBack)
    {
#if UNITY_EDITOR || UNITY_EDITOR_WIN
      string resPath = "file://" + AppConfig.GetstreamingAssetsPath + "/Res.zip";
#else
            string resPath = AppConfig.GetstreamingAssetsPath + "/Res.zip";
#endif
      Debug.Log("AssemblyAsset Ready: " + resPath);
      UnityWebRequest www = new UnityWebRequest(resPath);
      DownloadHandlerBuffer dowle = new DownloadHandlerBuffer();
      www.downloadHandler = dowle;
      Debug.Log("AssemblyAsset Start: " + resPath);
      yield return www.SendWebRequest();
      Debug.Log("AssemblyAsset Result: " + resPath);
      if (www.error != null)
        Debug.LogError("读取内部资源文件失败 path:" + resPath + " err:" + www.error);
      else
      {
        // if (IsHaveLoadView)
        MyModule.LoadViewComp?.Show();
        yield return ZipTools.UnzipFile(dowle.data, AppConfig.AppAssetBundleAddress, AppConfig.ResZipPassword, (string describe, float progress) =>
        {
          // if (IsHaveLoadView)
          // {
          //   Progress1Describe.text = "正在解压资源:" + describe;
          //   Progress.fillAmount = progress;
          // }
          // else
          // {
          MyModule.LoadViewComp?.UpdateProgress(progress, "正在解压资源:" + describe);
          Debug.Log(string.Format("正在解压资源 {0}:{1}", progress, describe));
          // }
        });
        // if (IsHaveLoadView)
        //   LoadView.SetActive(false);
        if (CallBack != null)
        {
          CallBack();
        }
        www.Dispose();
      }
    }

    //加载App资源配置文件
    public void LoadAppAssetInfo()
    {
      bool IsSucc = FilesTools.IsKeepFileOrDirectory(AppConfig.AppAssetBundleAddress + "/AssetInfo.json");
      if (IsSucc)
      {
        string jsonStr = FilesTools.ReadFileToStr(AppConfig.AppAssetBundleAddress + "/AssetInfo.json");
        AppModuleAssetInfo appResourceInfo = LoadAppBuileInfo(jsonStr);
        foreach (var item in appResourceInfo.AppResInfo)
        {
          ResourceInfo.AppResInfo[item.Key] = item.Value;
        }
      }
    }

    //加载模块资源配置文件
    public void LoadModuleAssetInfo(string ModuleName)
    {
      ModuleName = ModuleName.ToLower();
      string modulepath = AppConfig.AppAssetBundleAddress + "/" + ModuleName + "/AssetInfo.json";
      bool IsSucc = FilesTools.IsKeepFileOrDirectory(modulepath);
      if (IsSucc)
      {
        string jsonStr = FilesTools.ReadFileToStr(modulepath);
        AppModuleAssetInfo moduleInfo = LoadAppBuileInfo(jsonStr);
        foreach (var item in moduleInfo.AppResInfo)
        {
          ResourceInfo.AppResInfo[item.Key] = item.Value;
        }
      }
    }

    private AppModuleAssetInfo LoadAppBuileInfo(string JsonStr)
    {
      AppModuleAssetInfo data = new AppModuleAssetInfo();
      data.AppResInfo = new Dictionary<string, ResBuileInfo>();
      JSONNode json = JSON.Parse(JsonStr);
      foreach (var item in json["AppResInfo"].Childs)
      {
        ResBuileInfo asinfo = new ResBuileInfo();
        asinfo.Id = item["Id"].Value;
        asinfo.Model = item["Model"].Value;
        asinfo.Size = float.Parse(item["Size"].Value);
        asinfo.Md5 = item["Md5"].Value;
        asinfo.Dependencies = new string[item["Dependencies"].Count];
        int n = 0;
        foreach (var item1 in item["Dependencies"].Childs)
        {
          asinfo.Dependencies[n++] = item1.Value;
        }
        data.AppResInfo[asinfo.Id] = asinfo;
      }
      return data;
    }

    //加载lua文件
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
        AssetBundle bundle = LoadAssetBundle(ModelName, BundleName);
        if (bundle != null)
        {
          TextAsset ret;
          if (AssetName != null)
            ret = bundle.LoadAsset<TextAsset>(GetAssetName(bundle, AssetName));
          else
            ret = bundle.LoadAllAssets<TextAsset>()[0];

          if (null != ret)
          {
            Assets[ModelName][Key] = ret;
            return ret.bytes;
          }
          else
          {
            Debug.LogError("Asset文件不存在 ModelName = " + ModelName + " BundleName = " + BundleName + " AssetName = " + AssetName);
          }
        }
      }
      return null;
    }


    //加载pb文件
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
        AssetBundle bundle = LoadAssetBundle(ModelName, BundleName);
        if (bundle != null)
        {
          TextAsset ret;
          if (AssetName != null)
            ret = bundle.LoadAsset<TextAsset>(GetAssetName(bundle, AssetName));
          else
            ret = bundle.LoadAllAssets<TextAsset>()[0];

          if (null != ret)
          {
            Assets[ModelName][Key] = ret;
            return ret.bytes;
          }
          else
          {
            Debug.LogError("Asset文件不存在 ModelName = " + ModelName + " BundleName = " + BundleName + " AssetName = " + AssetName);
          }
        }
      }
      return null;
    }

    /// <summary>
    /// 加载资源文件
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="bundleOrPath">资源相对路径</param>
    /// <param name="assetName">资源名称</param>
    /// <param name="IsSave">是否保存</param>
    /// <returns></returns>
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
        AssetBundle bundle = LoadAssetBundle(ModelName, BundleName);
        if (bundle != null)
        {
          T ret = null;
          if (AssetName != null)
            ret = bundle.LoadAsset<T>(GetAssetName(bundle, AssetName));
          else
            ret = bundle.LoadAllAssets<T>()[0];

          if (null != ret)
          {
            Assets[ModelName][Key] = ret;
            return ret;
          }
          else
          {
            Debug.LogError("Asset文件不存在 ModelName = " + ModelName + " BundleName = " + BundleName + " AssetName = " + AssetName);
          }
        }
      }
      return null;
    }

    /// <summary>
    /// 加载资源文件
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="bundleOrPath">资源相对路径</param>
    /// <param name="assetName">资源名称</param>
    /// <param name="IsSave">是否保存</param>
    /// <returns></returns>
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
      AssetBundle bundle = LoadAssetBundle(ModelName, BundleName);
      if (bundle != null)
      {
        T[] ret = null;
        if (AssetName != string.Empty)
          ret = bundle.LoadAssetWithSubAssets<T>(GetAssetName(bundle, AssetName));
        else
          ret = bundle.LoadAllAssets<T>();
        if (null != ret)
        {
          foreach (var item in ret)
          {
            Assets[ModelName][item.name] = item;
          }
          return ret;
        }
        else
        {
          Debug.LogError("Asset文件不存在 ModelName = " + ModelName + " BundleName = " + BundleName + " AssetName = " + AssetName);
        }
      }
      return null;
    }

    /// <summary>
    /// 加载bundle文件
    /// </summary>
    /// <param name="bundleName">bundle名称</param>
    /// <returns></returns>
    public AssetBundle LoadAssetBundle(string ModelName, string BundleName)
    {
      //统一全部小写
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      if (!Bundles.ContainsKey(ModelName))
      {
        Bundles[ModelName] = new Dictionary<string, AssetBundle>();
      }
      if (Bundles[ModelName].ContainsKey(BundleName))
      {
        return Bundles[ModelName][BundleName];
      }
      else
      {
        string bundlepath = (ModelName + "/" + BundleName + AppConfig.ResFileSuffix).ToLower();
        if (ResourceInfo.AppResInfo.ContainsKey(bundlepath))
        {
          ResBuileInfo Resinfo = ResourceInfo.AppResInfo[bundlepath];
          for (int i = 0; i < Resinfo.Dependencies.Length; i++)
          {
            string _modelname = Resinfo.Dependencies[i].Substring(0, Resinfo.Dependencies[i].IndexOf("/"));
            string _bundlename = Resinfo.Dependencies[i].Substring(_modelname.Length + 1);
            _bundlename = _bundlename.Substring(0, _bundlename.IndexOf("."));
            LoadAssetBundle(_modelname, _bundlename);
          }
          string path = Path.Combine(AppConfig.AppAssetBundleAddress, bundlepath);
          if (File.Exists(path))
          {
            Bundles[ModelName][BundleName] = AssetBundle.LoadFromMemory(File.ReadAllBytes(path));
            Debug.Log("加载 AssetBundle ModelName =" + ModelName + " BundleName =" + BundleName);
            return Bundles[ModelName][BundleName];
          }
        }

      }
      Debug.LogError("Bundle文件不存在 ModelName = " + ModelName + " BundleName = " + BundleName);
      return null;
    }

    /// <summary>
    /// 获取资源名称
    /// </summary>
    /// <param name="bundle"></param>
    /// <param name="assetName"></param>
    /// <returns></returns>
    static string GetAssetName(AssetBundle bundle, string assetName)
    {
      assetName = assetName.ToLower();
      if (assetName.IndexOf('/') >= 0)
      {
        if (assetName.IndexOf('.') >= 0)
        {
          assetName = bundle.name + assetName;
        }
        else
        {
          assetName = assetName.Substring(assetName.LastIndexOf('/') + 1);
        }
      }
      return assetName.ToLower();
    }

    public void UnloadAsset(string ModelName, string BundleName, string AssetName)
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      AssetName = AssetName.ToLower();
      if (Assets.ContainsKey(ModelName))
      {
        string Key = BundleName + "/" + AssetName;
        if (Assets[ModelName].ContainsKey(Key))
        {
          Resources.UnloadAsset(Assets[ModelName][Key]);
        }
        Assets[ModelName].Remove(Key);
      }
    }

    public void UnloadBundle(string ModelName, string BundleName)
    {
      ModelName = ModelName.ToLower();
      BundleName = BundleName.ToLower();
      if (Bundles.ContainsKey(ModelName))
      {
        if (Bundles[ModelName].ContainsKey(BundleName))
        {
          Bundles[ModelName][BundleName].Unload(false);
          Bundles[ModelName].Remove(BundleName);
        }
      }
    }

    public void UnloadModel(string ModelName)
    {
      ModelName = ModelName.ToLower();
      if (Bundles.ContainsKey(ModelName))
      {
        foreach (var item in Bundles[ModelName])
        {
          //注意 这里true 表示卸载资源文件以及销毁所有生成的实例对象，false 表示卸载镜像文件 但不会销毁所关联的实例对象
          item.Value.Unload(false);
        }
        Bundles.Remove(ModelName);
      }
      if (Assets.ContainsKey(ModelName))
      {
        foreach (var item in Assets[ModelName])
        {
          Resources.UnloadAsset(item.Value);
        }
        Assets.Remove(ModelName);
      }
    }

  }
}