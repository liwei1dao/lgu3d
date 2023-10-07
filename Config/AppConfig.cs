using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace lgu3d
{
    public enum AppPlatform
    {
        IOS,
        Android,
        Windows,
    }
    public enum AppResModel
    {
        debug,
        release,
    }

    public static class AppConfig
    {
        public static bool IsOpenVersionCheck;                                  //是否开启版本检测
        public static string ServiceAddr;                                       //服务器地址
        public static AppResModel AppResModel = AppResModel.debug;              //资源加载模式
        public static AppPlatform TargetPlatform = AppPlatform.Windows;         //当前平台
        public static string ResZipPassword = "liwei1dao";                      //内部资源压缩密码
        public static string ResFileSuffix = ".1dao";
        public const string mApplogAddress = "Log";
        public const string mAppExternalAddress = "AppResources";
        public const string mAppResourcesTemp = "AppResourcesTemp";

        public static string PlatformRoot
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return Application.persistentDataPath;
                    case RuntimePlatform.Android:
                        return Application.persistentDataPath;
                    case RuntimePlatform.WindowsPlayer:
                        return Application.persistentDataPath;
                    case RuntimePlatform.WindowsEditor:
                        return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length);
                    default:
                        return Application.dataPath.Substring(0, Application.dataPath.Length - "Assets/".Length);
                }
            }
        }                                        //平台沙盒存储根目录

        public static string GetstreamingAssetsPath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.IPhonePlayer:
                        return Application.streamingAssetsPath;
                    case RuntimePlatform.Android:
                        return Application.streamingAssetsPath;
                    case RuntimePlatform.WindowsPlayer:
                        return Application.streamingAssetsPath;
                    case RuntimePlatform.WindowsEditor:
                        return Application.streamingAssetsPath;
                    default:
                        return Application.streamingAssetsPath;
                }
            }
        }

        public static string AppLogAddress
        {
            get
            {
                return Path.Combine(PlatformRoot, mApplogAddress);
            }
        }

        public static string AppAssetBundleTemp
        {
            get
            {
                return Path.Combine(PlatformRoot, mAppResourcesTemp);
            }
        }

        public static string AppAssetBundleAddress
        {
            get
            {
                return Path.Combine(PlatformRoot, mAppExternalAddress);
            }
        }
    }
}

