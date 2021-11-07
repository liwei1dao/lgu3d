using UnityEngine;
using System;
using System.IO;

namespace lgu3d
{
  public abstract class Main : MonoBehaviour
  {
    [MFWAttributeRename("是否开启版本检测")]
    public bool IsOpenVersionCheck;
    [MFWAttributeRename("App资源加载方式")]
    public AppResModel AppResModel;
    protected virtual void SetConfig()
    {
      AppConfig.IsOpenVersionCheck = IsOpenVersionCheck;
      AppConfig.AppResModel = AppResModel;
#if UNITY_IOS
            AppConfig.TargetPlatform = AppPlatform.IOS;
#elif UNITY_ANDROID
      AppConfig.TargetPlatform = AppPlatform.Android;
#else
            AppConfig.TargetPlatform = AppPlatform.Windows;
#endif
    }

    private void Awake()
    {
      SetConfig();
      StartApp();
    }

    protected abstract void StartApp();
  }
}

