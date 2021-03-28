using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;
using System;

namespace lgu3d
{
    /// <summary>
    /// 穿山甲广告模块 Feed广告
    /// </summary>
    public class ExpressFeedAdComp : ModelCompBase<PangolinAdvModule>,IExpressAdListener,IExpressAdInteractionListener,IDislikeInteractionListener
    {
        private ExpressAd mExpressFeedad;
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
            if (this.mExpressFeedad != null)
            {
                this.mExpressFeedad.Dispose();
                this.mExpressFeedad = null;
            }
            var adSlot = new AdSlot.Builder()
                                .SetCodeId(advId)
                                .SetExpressViewAcceptedSize(350, 0)
                                .SetSupportDeepLink(true)
                                .SetImageAcceptedSize(1080, 1920)
                                .SetOrientation(AdOrientation.Horizontal)
                                .SetAdCount(1) //请求广告数量为1到3条
                                .Build();
            this.MyModule.AdNative.LoadNativeExpressAd(adSlot, this);
        }

        private void  ShowAdv_Android(string advId){
            if (this.mExpressFeedad != null)
            {
                this.mExpressFeedad.Dispose();
                this.mExpressFeedad = null;
            }
            var adSlot = new AdSlot.Builder()
                    .SetCodeId(advId)
                    .SetExpressViewAcceptedSize(350, 0)
                    .SetSupportDeepLink(true)
                    .SetImageAcceptedSize(1080, 1920)
                    .SetOrientation(AdOrientation.Horizontal)
                    .SetAdCount(1) //请求广告数量为1到3条
                    .Build();
            this.MyModule.AdNative.LoadNativeExpressAd(adSlot, this);
        }

        #region 加载接口
        public void OnError(int code, string message)
        {
            Debug.LogError("IExpressAdListener OnError: " + message);
        }

        public void OnExpressAdLoad(List<ExpressAd> ads)
        {
            Debug.LogError("OnExpressAdLoad");
            IEnumerator<ExpressAd> enumerator = ads.GetEnumerator();
            if(enumerator.MoveNext())
            {
                this.mExpressFeedad = enumerator.Current;
                this.mExpressFeedad.SetExpressInteractionListener(this);
                this.mExpressFeedad.SetDownloadListener(MyModule);
            }
            #if UNITY_IOS
                this.mExpressFeedad.ShowExpressAd(0, 100);
            #elif UNITY_ANDROID
                this.mExpressFeedad.SetExpressInteractionListener(this);
                this.mExpressFeedad.SetDownloadListener();
                NativeAdManager.Instance().ShowExpressFeedAd(GetActivity(),mExpressFeedad.handle,this,this);
            #endif
        }

        public void OnExpressBannerAdLoad(ExpressBannerAd ad)
        {

        }

        public void OnExpressInterstitialAdLoad(ExpressInterstitialAd ad)
        {

        }
        #endregion
        #region 播放监听实例接口
        public void OnAdViewRenderSucc(ExpressAd ad, float width, float height)
        {
            Debug.LogError("express OnAdViewRenderSucc,type:ExpressFeedAdComp");
        }

        public void OnAdViewRenderError(ExpressAd ad, int code, string message)
        {
            Debug.LogError("express OnAdViewRenderError,type:ExpressFeedAdComp");
        }

        public void OnAdShow(ExpressAd ad)
        {
            Debug.LogError("express OnAdShow,type:ExpressFeedAdComp");
        }

        public void OnAdClicked(ExpressAd ad)
        {
            Debug.LogError("express OnAdClicked,type:ExpressFeedAdComp");
        }

        public void OnAdClose(ExpressAd ad)
        {
            Debug.LogError("express OnAdClose,type:ExpressFeedAdComp");
        }
        #endregion
        #region 回调监听
        public void OnSelected(int var1, string var2)
        {
            Debug.Log("express dislike OnSelected");
            if (this.mExpressFeedad != null)
            {
                NativeAdManager.Instance().DestoryExpressAd(this.mExpressFeedad.handle);
                this.mExpressFeedad = null;
            }
        }

        public void OnCancel()
        {
            Debug.Log("express dislike OnCancel");
        }

        public void OnRefuse()
        {
            Debug.Log("express dislike OnRefuse");
        }
        #endregion
    }
}