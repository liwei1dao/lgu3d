using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 模块资源组件
  /// </summary>
  public class Module_ResourceComp : ModelCompBase
  {
    #region 构架
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      base.LoadEnd();
    }
    public override void Close()
    {
      if (ResourceModule.Instance != null)
        ResourceModule.Instance.UnloadModel(MyModule.ModuleName);
      base.Close();
    }
    #endregion

    public AssetBundle LoadAssetBundle(string BundleName)
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
      }
      if (ResourceModule.Instance != null)
      {
        return ResourceModule.Instance.LoadAssetBundle(MyModule.ModuleName, BundleName);
      }
      else
      {
        Debug.LogError("ResourceModel No Load");
        return null;
      }
    }

    public T LoadAsset<T>(string BundleName, string AssetName) where T : UnityEngine.Object
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
      }
      if (ResourceModule.Instance != null)
      {
        return ResourceModule.Instance.LoadAsset<T>(MyModule.ModuleName, BundleName, AssetName);
      }
      else
      {
        Debug.LogError("ResourceModel No Load");
        return null;
      }
    }
    public T[] LoadAllAsset<T>(string BundleName, string AssetName) where T : UnityEngine.Object
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
      }
      if (ResourceModule.Instance != null)
      {
        return ResourceModule.Instance.LoadAllAsset<T>(MyModule.ModuleName, BundleName, AssetName);
      }
      else
      {
        Debug.LogError("ResourceModel No Load");
        return null;
      }
    }

    public byte[] LoadLuaFile(string BundleName, string AssetName)
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
      }
      if (ResourceModule.Instance != null)
      {
        return ResourceModule.Instance.LoadLuaFile(MyModule.ModuleName, BundleName, AssetName);
      }
      else
      {
        Debug.LogError("ResourceModel No Load");
        return null;
      }
    }

    public byte[] LoadProtoFile(string BundleName, string AssetName)
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
        if (AssetName != null)
          AssetName = AssetName.ToLower();
      }
      if (ResourceModule.Instance != null)
      {
        return ResourceModule.Instance.LoadProtoFile(MyModule.ModuleName, BundleName, AssetName);
      }
      else
      {
        Debug.LogError("ResourceModel No Load");
        return null;
      }
    }

    public void UnloadAsset(string BundleName, string AssetName)
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
        AssetName = AssetName.ToLower();
      }
      ResourceModule.Instance.UnloadAsset(MyModule.ModuleName, BundleName, AssetName);
    }

    public void UnloadBundle(string BundleName)
    {
      string ModelName = MyModule.ModuleName;
      if (AppConfig.AppResModel == AppResModel.release)
      {
        ModelName = ModelName.ToLower();
        BundleName = BundleName.ToLower();
      }
      ResourceModule.Instance.UnloadBundle(MyModule.ModuleName, BundleName);
    }

  }
}
