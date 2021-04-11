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
        private SplashAdComp splashAdComp;                  //开屏广告
        private ExpressSplashAdComp expressSplashAdComp;  //开屏广告
        private ExpressInterstitialAdComp expressInterstitialAdComp; //插屏广告
        private ExpressFullScreenVideoAdComp expressFullScreenVideoAdComp; //全屏视频广告
        private ExpressBannerAdComp expressBannerAdComp;    //横幅广告
        private ExpressRewardAdComp expressRewardAdComp;    //奖励广告
        private ExpressFeedAdComp expressFeedAdComp;     //Feed广告

        public override void Load(params object[] _Agr)
        {
            splashAdComp = AddComp<SplashAdComp>();
            expressSplashAdComp = AddComp<ExpressSplashAdComp>();
            expressInterstitialAdComp = AddComp<ExpressInterstitialAdComp>();
            expressFullScreenVideoAdComp = AddComp<ExpressFullScreenVideoAdComp>();
            expressBannerAdComp = AddComp<ExpressBannerAdComp>();
            expressRewardAdComp = AddComp<ExpressRewardAdComp>();
            expressFeedAdComp = AddComp<ExpressFeedAdComp>();
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

        #region 接口
        public void ShowSplashAd(string advId){
            splashAdComp.ShowAdv(advId);
        }
        public void ExpressShowSplashAd(string advId){
            expressSplashAdComp.ShowAdv(advId);
        }
        
        public void ShowInterstitialAd(string advId){
            expressInterstitialAdComp.ShowAdv(advId);
        }
        public void ShowFullScreenVideoAd(string advId){
            expressFullScreenVideoAdComp.ShowAdv(advId);
        }
        public void ShowBannerAd(string advId){
            expressBannerAdComp.ShowAdv(advId);
        }
        public void ShowRewardAd(string advId){
            expressRewardAdComp.ShowAdv(advId);
        }
        public void ShowFeedAd(string advId){
            expressFeedAdComp.ShowAdv(advId); 
        }
        #endregion

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
