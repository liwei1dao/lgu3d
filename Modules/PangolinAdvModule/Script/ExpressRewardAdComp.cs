using System.Collections;
using System.Collections.Generic;
using ByteDance.Union;
using UnityEngine;
using System;

namespace lgu3d
{
  /// <summary>
  /// 穿山甲广告模块 奖励广告
  /// </summary>
  public class ExpressRewardAdComp : ModelCompBase<PangolinAdvModule>, IRewardVideoAdListener, IRewardAdInteractionListener
  {
    private ExpressRewardVideoAd expressRewardAd;   // for iOS
    private RewardVideoAd rewardAd;                 // for Android
    private Action<bool> callback;

    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl, _Agr);
      LoadEnd();
    }


    /// <summary>
    /// 显示奖励广告
    /// </summary>
    public void ShowAdv(string advId, Action<bool> callback)
    {
      this.callback = callback;
#if !UNITY_EDITOR && UNITY_IOS
                ShowAdv_IOS(advId);
#elif !UNITY_EDITOR && UNITY_ANDROID
                ShowAdv_Android(advId);
#else
      Debug.Log("PangolinAdvModule ExpressBannerAdComp ShowAdv current platform not support");
#endif
    }

    private void ShowAdv_IOS(string advId)
    {
      if (this.expressRewardAd != null)
      {
        this.expressRewardAd.Dispose();
        this.expressRewardAd = null;
      }
      var adSlot = new AdSlot.Builder()
                  .SetCodeId(advId)
                  .SetSupportDeepLink(true)
                  .SetImageAcceptedSize(1080, 1920)
                  .SetUserID("user123") // 用户id,必传参数
                  .SetMediaExtra("media_extra") // 附加参数，可选
                  .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
                  .Build();
      this.MyModule.AdNative.LoadExpressRewardAd(adSlot, this);
    }

    private void ShowAdv_Android(string advId)
    {
      if (this.rewardAd != null)
      {
        this.rewardAd.Dispose();
        this.rewardAd = null;
      }
      var adSlot = new AdSlot.Builder()
                  .SetCodeId(advId)
                  .SetSupportDeepLink(true)
                  .SetImageAcceptedSize(1080, 1920)
                  .SetUserID("user123") // 用户id,必传参数
                  .SetMediaExtra("media_extra") // 附加参数，可选
                  .SetOrientation(AdOrientation.Horizontal) // 必填参数，期望视频的播放方向
                  .Build();
      this.MyModule.AdNative.LoadRewardVideoAd(adSlot, this);
    }

    #region 加载接口
    public void OnError(int code, string message)
    {
      Debug.Log("ExpressRewardAdComp OnError:" + message);
      this?.callback(false);
    }

    public void OnRewardVideoAdLoad(RewardVideoAd ad)
    {
      Debug.Log("OnRewardVideoAdLoad");
      ad.SetRewardAdInteractionListener(this);
      ad.SetDownloadListener(MyModule);
      this.rewardAd = ad;
      this.rewardAd.ShowRewardVideoAd();
    }

    public void OnRewardVideoCached()
    {

    }

    public void OnExpressRewardVideoAdLoad(ExpressRewardVideoAd ad)
    {
      Debug.Log("OnRewardExpressVideoAdLoad");
      ad.SetRewardAdInteractionListener(this);
      ad.SetDownloadListener(MyModule);
      this.expressRewardAd = ad;
      this.expressRewardAd.ShowRewardVideoAd();
    }



    #endregion
    #region 显示监控接口
    public void OnAdShow()
    {
      Debug.Log("rewardVideoAd show");
    }

    public void OnAdVideoBarClick()
    {
      Debug.Log("rewardVideoAd bar click");
    }

    public void OnAdClose()
    {
      this.rewardAd = null;
      this.expressRewardAd = null;
    }

    public void OnVideoComplete()
    {
      Debug.Log("rewardVideoAd complete");
      this?.callback(true);
    }

    public void OnVideoError()
    {
      Debug.LogError("rewardVideoAd error");
      this?.callback(false);
    }

    public void OnRewardVerify(bool rewardVerify, int rewardAmount, string rewardName)
    {
      Debug.Log("verify:" + rewardVerify + " amount:" + rewardAmount +
          " name:" + rewardName);
    }
    #endregion
  }
}