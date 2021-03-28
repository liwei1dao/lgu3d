using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;
using System;

namespace lgu3d
{
    /// <summary>
    /// 穿山甲广告模块 横幅广告
    /// </summary>
    public class ExpressBannerAdComp : ModelCompBase<PangolinAdvModule>,IExpressAdListener,IExpressAdInteractionListener,IDislikeInteractionListener
    {
        private ExpressBannerAd iExpressBannerAd; // for iOS
        private ExpressAd mExpressBannerAd;// for Android
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        /// <summary>
        /// 显示横幅广告
        /// </summary>
        public void ShowAdv(string advId){
            #if !UNITY_EDITOR && UNITY_IOS
                ShowAdv_IOS(advId);
            #elif !UNITY_EDITOR && UNITY_ANDROID
                ShowAdv_Android(advId);
            #else
                Debug.Log("PangolinAdvModule ExpressBannerAdComp ShowAdv current platform not support");
            #endif
        }

        private void  ShowAdv_IOS(string advId){
            if (this.iExpressBannerAd != null)
            {
                this.iExpressBannerAd.Dispose();
                this.iExpressBannerAd = null;
            }
            int width = UnityEngine.Screen.width;
            int height =  width / 600 * 90;
            var adSlot = new AdSlot.Builder()
                        .SetCodeId(advId)
                        .SetExpressViewAcceptedSize(width, height)
                        .SetSupportDeepLink(true)
                        .SetImageAcceptedSize(1080, 1920)
                        .SetAdCount(1)
                        .SetOrientation(AdOrientation.Horizontal)
                        .Build();

            Debug.Log("SetExpressViewAcceptedSize:" + float.Parse(UnityEngine.Screen.width.ToString()) / 600 * 90);
            this.MyModule.AdNative.LoadExpressBannerAd(adSlot, this);
        }

        private void  ShowAdv_Android(string advId){
            if (this.mExpressBannerAd != null)
            {
                this.mExpressBannerAd.Dispose();
                this.mExpressBannerAd = null;
            }
            int width = UnityEngine.Screen.width;
            int height =  width / 600 * 90;
            var adSlot = new AdSlot.Builder()
                        .SetCodeId(advId)
                        .SetExpressViewAcceptedSize(width, height)
                        .SetSupportDeepLink(true)
                        .SetImageAcceptedSize(1080, 1920)
                        .SetAdCount(1)
                        .SetOrientation(AdOrientation.Horizontal)
                        .Build();
            Debug.Log("SetExpressViewAcceptedSize:" + float.Parse(UnityEngine.Screen.width.ToString()) / 600 * 90);
            this.MyModule.AdNative.LoadExpressBannerAd(adSlot, this);
        }

        #region 加载接口
        public void OnError(int code, string message)
        {
            Debug.LogError("onExpressAdError: " + message);
        }

        public void OnExpressAdLoad(List<ExpressAd> ads)
        {
            Debug.LogError("OnExpressAdLoad");
            IEnumerator<ExpressAd> enumerator = ads.GetEnumerator();
            if(enumerator.MoveNext())
            {
                this.mExpressBannerAd = enumerator.Current;
            }
            //设置轮播间隔 30s--120s;不设置则不开启轮播
            this.mExpressBannerAd.SetSlideIntervalTime(30*1000);
            this.mExpressBannerAd.SetDownloadListener(MyModule);
            NativeAdManager.Instance().ShowExpressBannerAd(MyModule.GetActivity(), mExpressBannerAd.handle, this, this);
        }
        public void OnExpressBannerAdLoad(ExpressBannerAd ad)
        {
            Debug.Log("OnExpressBannerAdLoad");
            ad.SetExpressInteractionListener(this);
            ad.SetDownloadListener(MyModule);
            this.iExpressBannerAd = ad;
            this.iExpressBannerAd.ShowExpressAd(0, 100);
        }

        public void OnExpressInterstitialAdLoad(ExpressInterstitialAd ad)
        {

        }
        #endregion

        #region 显示监听
        public void OnAdViewRenderSucc(ExpressAd ad, float width, float height)
        {
            Debug.Log("express OnAdViewRenderSucc,type:ExpressBannerAdComp");
        }

        public void OnAdViewRenderError(ExpressAd ad, int code, string message)
        {
            Debug.Log("express OnAdViewRenderSucc,type:ExpressBannerAdComp");
        }

        public void OnAdShow(ExpressAd ad)
        {
            Debug.Log("express OnAdViewRenderSucc,type:ExpressBannerAdComp");
        }

        public void OnAdClicked(ExpressAd ad)
        {
            Debug.Log("express OnAdViewRenderSucc,type:ExpressBannerAdComp");
        }

        public void OnAdClose(ExpressAd ad)
        {
            Debug.Log("express OnAdViewRenderSucc,type:ExpressBannerAdComp");
        }
        #endregion

        #region 回调函数
        public void OnSelected(int var1, string var2)
        {
            Debug.Log("express dislike OnSelected:");
            NativeAdManager.Instance().DestoryExpressAd(this.mExpressBannerAd.handle);
            this.mExpressBannerAd = null;
        }

        public void OnCancel()
        {
            Debug.Log("express dislike OnCancel");
        }

        public void OnRefuse()
        {
            Debug.Log("express dislike onRefuse");
        }
        #endregion
    }

}