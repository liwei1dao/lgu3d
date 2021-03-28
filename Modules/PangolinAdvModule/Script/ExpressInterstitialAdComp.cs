using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;

namespace lgu3d
{
    /// <summary>
    /// 穿山甲广告模块 插屏广告
    /// </summary>
    public class ExpressInterstitialAdComp : ModelCompBase<PangolinAdvModule>,IExpressAdListener,IExpressAdInteractionListener
    {      
        #if UNITY_IOS
        private ExpressInterstitialAd iExpressInterstitialAd;   // for iOS
        #endif
        private ExpressAd mExpressInterstitialAd;               // for Android
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        /// <summary>
        /// 显示插屏广告
        /// </summary>
        public void ShowAdv(string advId){
            #if !UNITY_EDITOR && UNITY_IOS
                ShowAdv_IOS(advId);
            #elif !UNITY_EDITOR && UNITY_ANDROID
                ShowAdv_Android(advId);
            #else
                Debug.Log("PangolinAdvModule ExpressInterstitialAdComp ShowAdv current platform not support");
            #endif
        }

        private void  ShowAdv_IOS(string advId){
            #if UNITY_IOS
            if (this.iExpressInterstitialAd != null)
            {
                this.iExpressInterstitialAd.Dispose();
                this.iExpressInterstitialAd = null;
            }
            #endif
            int width = UnityEngine.Screen.width;
            int height = width / 200 * 300;

            var adSlot = new AdSlot.Builder()
                        .SetCodeId(advId)
                        .SetExpressViewAcceptedSize(width, height)
                        ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
                        .SetSupportDeepLink(true)
                        .SetAdCount(1)
                        .SetImageAcceptedSize(1080, 1920)
                        .Build();
            this.MyModule.AdNative.LoadExpressInterstitialAd(adSlot, this);
        }

        private void  ShowAdv_Android(string advId){
            if (this.mExpressInterstitialAd != null)
            {
                this.mExpressInterstitialAd.Dispose();
                this.mExpressInterstitialAd = null;
            }
            int width = UnityEngine.Screen.width;
            int height = width / 200 * 300;
            var adSlot = new AdSlot.Builder()
                        .SetCodeId(advId)
                        .SetExpressViewAcceptedSize(width, height)
                        ////期望模板广告view的size,单位dp，//高度设置为0,则高度会自适应
                        .SetSupportDeepLink(true)
                        .SetAdCount(1)
                        .SetImageAcceptedSize(1080, 1920)
                        .Build();
            this.MyModule.AdNative.LoadExpressInterstitialAd(adSlot, this);
        }


        #region 加载回调
        public void OnError(int code, string message)
        {
            Debug.LogError("ExpressInterstitialAdCompError: " + message);
        }

        public void OnExpressAdLoad(List<ExpressAd> ads)
        {
            Debug.LogError("OnExpressAdLoad");
            IEnumerator<ExpressAd> enumerator = ads.GetEnumerator();
            if(enumerator.MoveNext())
            {
               this.mExpressInterstitialAd = enumerator.Current;
            }
            this.mExpressInterstitialAd.SetDownloadListener(MyModule);
            NativeAdManager.Instance().ShowExpressInterstitialAd(MyModule.GetActivity(), mExpressInterstitialAd.handle, this);
        }
        #if UNITY_IOS
        public void OnExpressBannerAdLoad(ExpressBannerAd ad)
        {
            
        }

        public void OnExpressInterstitialAdLoad(ExpressInterstitialAd ad)
        {
            Debug.Log("OnExpressInterstitialAdLoad");
            ad.SetExpressInteractionListener(this);
            ad.SetDownloadListener(this.MyModule);
            this.iExpressInterstitialAd = ad;
            this.iExpressInterstitialAd.ShowExpressAd(0, 100);
        }
        #endif
        #endregion

        #region 显示监听
        public void OnAdViewRenderSucc(ExpressAd ad, float width, float height)
        {
              Debug.Log("express OnAdViewRenderSucc,type:ExpressInterstitialAdComp");
        }

        public void OnAdViewRenderError(ExpressAd ad, int code, string message)
        {
            Debug.LogError("express OnAdViewRenderError,type:ExpressInterstitialAdComp");
        }

        public void OnAdShow(ExpressAd ad)
        {
            Debug.Log("express OnAdShow,type:ExpressInterstitialAdComp");
        }

        public void OnAdClicked(ExpressAd ad)
        {
            Debug.Log("express OnAdClicked,type:ExpressInterstitialAdComp");
        }

        public void OnAdClose(ExpressAd ad)
        {
              Debug.Log("express OnAdClose,type:ExpressInterstitialAdComp");
        }
        #endregion
    }
}