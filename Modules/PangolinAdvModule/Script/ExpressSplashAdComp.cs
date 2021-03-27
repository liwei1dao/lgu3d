using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;
using System;

namespace lgu3d
{
    /// <summary>
    /// 穿山甲广告模块 开屏广告组件
    /// </summary>
    public class ExpressSplashAdComp : ModelCompBase<PangolinAdvModule>,ISplashAdListener,ISplashAdInteractionListener
    {
        private BUSplashAd splashAd;
        private BUExpressSplashAd expressSplashAd;
        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            LoadEnd();
        }

        /// <summary>
        /// 显示开屏广告
        /// </summary>
        public void ShowAdv(string advId){
            #if !UNITY_EDITOR && UNITY_IOS
                ShowAdv_IOS(advId);
            #elif !UNITY_EDITOR && UNITY_ANDROID
                ShowAdv_Android(advId);
            #else
                Debug.Log("PangolinAdvModule SplashAdComp ShowAdv current platform not support");
            #endif
        }

 #if UNITY_IOS
        private void  ShowAdv_IOS(string advId){
            if (this.splashAd != null)
            {
                this.splashAd.Dispose();
                this.splashAd = null;
            }
            if (this.expressSplashAd != null)
            {
                this.expressSplashAd.Dispose();
                this.expressSplashAd = null;
            }

            var adSlot = new AdSlot.Builder()
            .SetCodeId(advId)
            .SetImageAcceptedSize(1080, 1920)
            .SetExpressViewAcceptedSize(UnityEngine.Screen.width, UnityEngine.Screen.height)
            .Build();
            this.expressSplashAd = (this.MyModule.AdNative.LoadExpressSplashAd_iOS(adSlot, this)) as BUExpressSplashAd;
        }
#elif UNITY_ANDROID
        private void  ShowAdv_Android(string advId){
            var adSlot = new AdSlot.Builder()
                .SetCodeId(advId)
                .SetImageAcceptedSize(1080, 1920)
                .SetExpressViewAcceptedSize(UnityEngine.Screen.width, UnityEngine.Screen.height)
                .Build();
            this.MyModule.AdNative.LoadSplashAd(adSlot, this);
        }
#endif

        private void DestorySplash()
        {
            #if UNITY_ANDROID
                this.MyModule.GetSplashAdManager().Call("destorySplashView", this.MyModule.GetActivity());
            #else
                this.splashAd.Dispose();
                this.splashAd = null;
            #endif
        }

#region 加载监听
        /// <summary>
        /// 加载广告错误
        /// </summary>
        /// <param name="code"></param>
        /// <param name="message"></param>
        public void OnError(int code, string message)
        {
            Debug.Log("splash load Onerror:" + code + ":" + message);
        }

        /// <summary>
        /// 广告加载回调
        /// </summary>
        /// <param name="ad"></param>
        public void OnSplashAdLoad(BUSplashAd ad)
        {
            this.splashAd = ad;
            ad.SetSplashInteractionListener(this);
            #if UNITY_IOS
                if (ad != null)
                {
                    Debug.Log("expressSplash load Onsucc:");
                    this.expressSplashAd.SetSplashInteractionListener(this);
                }
            #elif UNITY_ANDROID
                if (ad.GetInteractionType() == this.MyModule.INTERACTION_TYPE_DOWNLOAD)
                {
                    Debug.Log("splash is download type ");
                    ad.SetDownloadListener(MyModule);
                }
                if (ad != null && this.MyModule.GetSplashAdManager() != null && this.MyModule.GetActivity() != null)
                {
                    this.MyModule.GetSplashAdManager().Call("showSplashAd", this.MyModule.GetActivity(), ad.getCurrentSplshAd());
                }
            #endif
        }
#endregion

#region 显示监听
        /// <summary>
        /// 点击广告后调用
        /// </summary>
        /// <param name="type"></param>
        public void OnAdClicked(int type)
        {
            #if UNITY_ANDROID
                if (type != this.MyModule.INTERACTION_TYPE_DOWNLOAD)
                {
                    DestorySplash();
                }
            #endif
        }

        /// <summary>
        /// 显示广告时调用
        /// </summary>
        /// <param name="type"></param>
        public void OnAdShow(int type)
        {
            
        }

        /// <summary>
        /// 跳过广告时调用
        /// </summary>
        public void OnAdSkip()
        {
            DestorySplash();
        }

        /// <summary>
        /// 广告超时后调用
        /// </summary>
        public void OnAdTimeOver()
        {
            DestorySplash();
        }

        /// <summary>
        /// 广告关闭时调用
        /// </summary>
        public void OnAdClose()
        {
            DestorySplash();
        }
#endregion
    }

}
