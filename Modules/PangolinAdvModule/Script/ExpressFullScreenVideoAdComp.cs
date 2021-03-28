using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;
using System;

namespace lgu3d
{
    /// <summary>
    /// 穿山甲广告模块 全屏视频广告
    /// </summary>
    public class ExpressFullScreenVideoAdComp : ModelCompBase<PangolinAdvModule>,IFullScreenVideoAdListener,IFullScreenVideoAdInteractionListener
    {
        private ExpressFullScreenVideoAd expressFullScreenVideoAd; // for iOS
        private FullScreenVideoAd fullScreenVideoAd;
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        /// <summary>
        /// 显示全屏视频广告
        /// </summary>
        public void ShowAdv(string advId){
            #if !UNITY_EDITOR && UNITY_IOS
                ShowAdv_IOS(advId);
            #elif !UNITY_EDITOR && UNITY_ANDROID
                ShowAdv_Android(advId);
            #else
                Debug.Log("PangolinAdvModule ExpressFullScreenVideoAdComp ShowAdv current platform not support");
            #endif
        }

        private void  ShowAdv_IOS(string advId){
            if (this.expressFullScreenVideoAd != null)
            {
                this.expressFullScreenVideoAd.Dispose();
                this.expressFullScreenVideoAd = null;
            }
            var adSlot = new AdSlot.Builder()
                            .SetCodeId(advId)
                            .SetSupportDeepLink(true)
                            .SetImageAcceptedSize(1080, 1920)
                            .SetOrientation(AdOrientation.Horizontal)
                            .Build();
            this.MyModule.AdNative.LoadExpressFullScreenVideoAd(adSlot, this);
        }

        private void  ShowAdv_Android(string advId){
            var adSlot = new AdSlot.Builder()
                            .SetCodeId(advId)
                            .SetSupportDeepLink(true)
                            .SetImageAcceptedSize(1080, 1920)
                            .SetOrientation(AdOrientation.Horizontal)
                            .Build();
            this.MyModule.AdNative.LoadFullScreenVideoAd(adSlot, this);
        }

        #region 加载接口
        public void OnError(int code, string message)
        {
             Debug.LogError("OnFullScreenError: " + message);
        }

        public void OnFullScreenVideoAdLoad(FullScreenVideoAd ad)
        {
            Debug.Log("OnFullScreenAdLoad");
            ad.SetFullScreenVideoAdInteractionListener(this);
            ad.SetDownloadListener(MyModule);

            this.fullScreenVideoAd = ad;
        }

        public void OnFullScreenVideoCached()
        {
            Debug.LogError("express OnFullScreenVideoCached,type:ExpressFullScreenVideoAdComp");
        }

        public void OnExpressFullScreenVideoAdLoad(ExpressFullScreenVideoAd ad)
        {
            Debug.Log("OnExpressFullScreenAdLoad");
            ad.SetFullScreenVideoAdInteractionListener(this);
            ad.SetDownloadListener(MyModule);
            this.expressFullScreenVideoAd = ad;
            this.expressFullScreenVideoAd.ShowFullScreenVideoAd();
        }
        #endregion
        #region 显示监控
        public void OnAdShow()
        {
           Debug.Log("fullScreenVideoAd OnAdShow");
        }

        public void OnAdVideoBarClick()
        {
           Debug.Log("fullScreenVideoAd OnAdVideoBarClick");
        }

        public void OnAdClose()
        {
            Debug.Log("fullScreenVideoAd OnAdClose");
            this.fullScreenVideoAd = null;
            this.expressFullScreenVideoAd = null;
        }

        public void OnVideoComplete()
        {
           Debug.Log("fullScreenVideoAd OnVideoComplete");
        }

        public void OnSkippedVideo()
        {
           Debug.Log("fullScreenVideoAd OnSkippedVideo");
        }

        public void OnVideoError()
        {
           Debug.Log("fullScreenVideoAd OnVideoError");
        }
        #endregion
    }
}