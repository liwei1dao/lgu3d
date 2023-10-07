using UnityEngine;
using System;
using System.IO;

namespace lgu3d
{
    public abstract class Main : MonoBehaviour
    {
        [LGAttributeRename("是否开启版本检测")]
        public bool IsOpenVersionCheck;
        [LGAttributeRename("App资源加载方式")]
        public AppResModel AppResModel;
        [LGAttributeRename("Web服务器地址")]
        public string WebServiceAddr;
        protected virtual void SetConfig()
        {
            AppConfig.IsOpenVersionCheck = IsOpenVersionCheck;
            AppConfig.AppResModel = AppResModel;
            AppConfig.ServiceAddr = WebServiceAddr;
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

