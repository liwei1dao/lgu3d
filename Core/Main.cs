using UnityEngine;
using System;
using System.IO;

namespace lgu3d
{
    public abstract class Main : MonoBehaviour
    {
        [MFWAttributeRename("是否显示日志")]
        public bool IsShowLog;
        [MFWAttributeRename("是否开启版本检测")]
        public bool IsOpenVersionCheck;
        [MFWAttributeRename("App资源加载方式")]
        public AppResModel AppResModel;
        protected virtual void SetConfig()
        {
            if (IsShowLog) {
                string logpath = string.Format(AppConfig.AppLogAddress + "/{0}.log", string.Format("{0:yyyy年MM月dd日HH时mm分ss秒}", DateTime.Now));
                if (!Directory.Exists(AppConfig.AppLogAddress))
                {
                    Directory.CreateDirectory(AppConfig.AppLogAddress);
                }
                File.Create(logpath).Dispose();
                HiLog.HiLogTextPath = logpath;
                HiLog.SetOn(IsShowLog);
                Debug.Log("日志地址:" + logpath);
            }
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

