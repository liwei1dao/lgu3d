using ByteDance.Union;
using UnityEngine;

namespace lgu3d
{
    public class PangolinAdvModule : ManagerContorBase<PangolinAdvModule>,IAppDownloadListener
    {
         public int INTERACTION_TYPE_DOWNLOAD = 4;
        private AndroidJavaObject activity;
        private AndroidJavaObject mSplashAdManager;
        private AdNative adNative;
        public override void Load(params object[] _Agr)
        {
            base.Load(_Agr);
        }

      
        public AdNative AdNative
        {
            get
            {
                if (this.adNative == null)
                {
                    this.adNative = SDK.CreateAdNative();
                }
                #if UNITY_ANDROID
                    SDK.RequestPermissionIfNecessary();
                #endif
                return this.adNative;
            }
        }
    
        public AndroidJavaObject GetActivity()
        {
            if (activity == null)
            {
                var unityPlayer = new AndroidJavaClass(
                "com.unity3d.player.UnityPlayer");
                activity = unityPlayer.GetStatic<AndroidJavaObject>(
                "currentActivity");
            }
            return activity;
        }

        public AndroidJavaObject GetSplashAdManager()
        {
            if (mSplashAdManager != null)
            {
                return mSplashAdManager;
            }
            var jc = new AndroidJavaClass(
                "com.bytedance.android.SplashAdManager");
            mSplashAdManager = jc.CallStatic<AndroidJavaObject>("getSplashAdManager");
            return mSplashAdManager;
        }

        #region 下载监听
        public void OnIdle()
        {
            
        }

        public void OnDownloadActive(long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("下载中，点击下载区域暂停");
        }

        public void OnDownloadPaused(long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.Log("下载暂停，点击下载区域继续");
        }

        public void OnDownloadFailed(long totalBytes, long currBytes, string fileName, string appName)
        {
            Debug.LogError("下载失败，点击下载区域重新下载");
        }

        public void OnDownloadFinished(long totalBytes, string fileName, string appName)
        {
            Debug.Log("下载完成，点击下载区域重新下载");
        }

        public void OnInstalled(string fileName, string appName)
        {
            Debug.Log("安装完成，点击下载区域打开");
        }
        #endregion
    }
}
