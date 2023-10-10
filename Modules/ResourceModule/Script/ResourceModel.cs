using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 资源管理模块
  /// </summary>
  public class ResourceModule : ManagerContorBase<ResourceModule>
  {
    public ILoadViewComp LoadViewComp;
    private AssetBundleComp BundleResComp;
#if UNITY_EDITOR
    private EditorResourComp EditorResComp;
#endif
    public override void Load(params object[] agrs)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        LoadViewComp = agrs[0] as ILoadViewComp;
        BundleResComp = AddComp<AssetBundleComp>();
      }
      else
      {
#if UNITY_EDITOR
        EditorResComp = AddComp<EditorResourComp>();
#else
        LoadViewComp = agrs[0] as ILoadViewComp;
        BundleResComp = AddComp<AssetBundleComp>();
#endif
      }
      LoadViewComp?.Load(this);
      base.Load(agrs);
    }

    public override void Start(params object[] agrs)
    {
      LoadViewComp?.Start();
      base.Start(agrs);
    }
    public override void Close()
    {
      LoadViewComp?.Close();
      base.Close();
    }
    //加载资源文件
    public void LoadAppAssetInfo()
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        BundleResComp.LoadAppAssetInfo();
      }
      else
      {
#if !UNITY_EDITOR
                BundleResComp.LoadAppAssetInfo();
#endif
      }
    }

    //加载模块资源文件
    public void LoadModuleAssetInfo(string ModuleName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        BundleResComp.LoadModuleAssetInfo(ModuleName);
      }
      else
      {
#if !UNITY_EDITOR
                BundleResComp.LoadModuleAssetInfo(ModuleName);
#endif
      }
    }

    public AssetBundle LoadAssetBundle(string ModelName, string BundleName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        return BundleResComp.LoadAssetBundle(ModelName, BundleName);
      }
      else
      {
#if !UNITY_EDITOR
                 return BundleResComp.LoadAssetBundle(ModelName, BundleName);
#endif
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
    public T LoadAsset<T>(string ModelName, string BundlePath, string AssetName) where T : UnityEngine.Object
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        return BundleResComp.LoadAsset<T>(ModelName, BundlePath, AssetName);
      }
      else
      {
#if UNITY_EDITOR
        return EditorResComp.LoadAsset<T>(ModelName, BundlePath, AssetName);
#else
                 return BundleResComp.LoadAsset<T>(ModelName, BundlePath, AssetName);
#endif
      }
    }

    /// <summary>
    /// 加载资源文件
    /// </summary>
    /// <typeparam name="T">加载资源类型</typeparam>
    /// <param name="bundleOrPath">资源相对路径</param>
    /// <param name="assetName">资源名称</param>
    /// <param name="IsSave">是否保存</param>
    /// <returns></returns>
    public T[] LoadAllAsset<T>(string ModelName, string BundlePath, string AssetName) where T : UnityEngine.Object
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        return BundleResComp.LoadAllAsset<T>(ModelName, BundlePath, AssetName);
      }
      else
      {
#if UNITY_EDITOR
        return EditorResComp.LoadAllAsset<T>(ModelName, BundlePath, AssetName);
#else
                 return BundleResComp.LoadAllAsset<T>(ModelName, BundlePath, AssetName);
#endif
      }
    }
    public byte[] LoadByteFile(string ModelName, string BundlePath, string AssetName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundlePath = BundlePath.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
        return BundleResComp.LoadByteFile(ModelName, BundlePath, AssetName);
      }
      else
      {
#if UNITY_EDITOR
        return EditorResComp.LoadByteFile(ModelName, BundlePath, AssetName);
#else
                ModelName = ModelName.ToLower();
                BundlePath = BundlePath.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
                return BundleResComp.LoadByteFile(ModelName, BundlePath, AssetName);
#endif
      }

    }
    public byte[] LoadLuaFile(string ModelName, string BundlePath, string AssetName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundlePath = BundlePath.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
        return BundleResComp.LoadByteFile(ModelName, BundlePath, AssetName);
      }
      else
      {
#if UNITY_EDITOR
        return EditorResComp.LoadLuaFile(ModelName, BundlePath, AssetName);
#else
                ModelName = ModelName.ToLower();
                BundlePath = BundlePath.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
                return BundleResComp.LoadByteFile(ModelName, BundlePath, AssetName);
#endif
      }

    }

    //加载proto 文件
    public byte[] LoadProtoFile(string ModelName, string BundlePath, string AssetName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundlePath = BundlePath.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
        return BundleResComp.LoadByteFile(ModelName, BundlePath, AssetName);
      }
      else
      {
#if UNITY_EDITOR
        return EditorResComp.LoadProtoFile(ModelName, BundlePath, AssetName);
#else
                ModelName = ModelName.ToLower();
                BundlePath = BundlePath.ToLower();
                if (AssetName != null)
                    AssetName = AssetName.ToLower();
                return BundleResComp.LoadByteFile(ModelName, BundlePath, AssetName);
#endif
      }

    }

    #region 清除资源
    public void UnloadAsset(string ModelName, string BundlePath, string AssetName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        BundleResComp.UnloadAsset(ModelName, BundlePath, AssetName);
      }
      else
      {
#if UNITY_EDITOR
        EditorResComp.UnloadAsset(ModelName, BundlePath, AssetName);
#else
                BundleResComp.UnloadAsset(ModelName, BundlePath, AssetName);
#endif
      }

    }

    public void UnloadBundle(string ModelName, string BundleName)
    {
      BundleResComp.UnloadBundle(ModelName, BundleName);
    }

    public void UnloadModel(string ModelName)
    {
      if (AppConfig.AppResModel == AppResModel.release)
      {
        BundleResComp.UnloadModel(ModelName);
      }
      else
      {
#if UNITY_EDITOR
        EditorResComp.UnloadModel(ModelName);
#else
                BundleResComp.UnloadModel(ModelName);
#endif
      }

    }

    #endregion

  }
}
