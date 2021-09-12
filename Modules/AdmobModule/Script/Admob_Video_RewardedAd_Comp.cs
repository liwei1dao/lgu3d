using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

namespace lgu3d
{
  /// <summary>
  /// Admob广告模块 激励广告
  /// </summary>
  public class Admob_Video_RewardedAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private RewardedAd rewardedAd;
    private Action<bool> RewardedFuncCall;

    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      state = AdvState.NoLoad;
      if (MyModule.advs.ContainsKey(AdvType.Video_RewardedAd))
      {
        adUnitId = MyModule.advs[AdvType.Video_RewardedAd];
      }
      LoadEnd();
    }

    public override void Start(params object[] agr)
    {
      base.Start(agr);
      if (!string.IsNullOrEmpty(adUnitId))
      {
        Request();
      }
    }
    /// <summary>
    /// 请求广告
    /// </summary>
    /// <param name="adUnitId"></param>
    /// <param name="func"></param>
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("AdmobModule Admob_Video_RewardedAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading)
      {
        Debug.Log("AdmobModule Admob_Video_RewardedAd_Comp Loading...");
        return;
      }
      state = AdvState.Loading;
      this.rewardedAd = new RewardedAd(adUnitId);
      // 广告请求成功加载后调用。
      this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
      // 显示广告时调用。
      this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
      // 广告请求显示失败时调用。
      this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
      // 在与广告互动应获得奖励的情况下调用。
      this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
      // 广告关闭时调用。
      this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;

      // Create an empty ad request.
      AdRequest request = new AdRequest.Builder().Build();
      // Load the rewarded ad with the request.
      this.rewardedAd.LoadAd(request);
    }

    public void Show(Action<bool> rewardefunc)
    {
      this.RewardedFuncCall = rewardefunc;
      if (state == AdvState.Loaded)
      {
        this.rewardedAd.Show();
      }
      else
      {
        this?.RewardedFuncCall(false);
        if (state == AdvState.NoLoad)
        {
          Request();
        }
      }
    }
    public void Hide()
    {
      this.rewardedAd?.Destroy();
      this?.RewardedFuncCall(false);
      this.rewardedAd = null;
      this.RewardedFuncCall = null;
      state = AdvState.NoLoad;
    }
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
      Debug.Log("HandleRewardedAdLoaded event received");
      state = AdvState.Loaded;
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
      Debug.Log("HandleRewardedAdOpening event received");
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
      Debug.Log("HandleRewardedAdFailedToShow event received with message: " + args.AdError.ToString());
      state = AdvState.NoLoad;
      Request();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
      Debug.Log("HandleRewardedAdClosed event received");
      state = AdvState.NoLoad;
      Request();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
      MobileAdsEventExecutor.ExecuteInUpdate(() =>
      {
        string type = args.Type;
        double amount = args.Amount;
        Debug.Log("HandleRewardedAdRewarded event received for " + amount.ToString() + " " + type);
        this?.RewardedFuncCall(true);
      });

    }
  }
}