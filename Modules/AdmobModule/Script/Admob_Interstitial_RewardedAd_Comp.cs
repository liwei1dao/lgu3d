using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;

namespace lgu3d
{
  /// <summary>
  /// Admob广告模块 插页式激励广告
  /// </summary>
  public class Admob_Interstitial_RewardedAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private RewardedInterstitialAd rewardedInterstitialAd;
    private Action<bool> RewardedFuncCall;
    private Action CloseFuncCall;
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      state = AdvState.NoLoad;
      if (MyModule.advs.ContainsKey(AdvType.IntersitialAd_RewardedAd))
      {
        adUnitId = MyModule.advs[AdvType.IntersitialAd_RewardedAd];
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
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("AdmobModule Admob_Interstitial_RewardedAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading)
      {
        Debug.Log("AdmobModule Admob_Interstitial_RewardedAd_Comp Loading...");
        return;
      }
      state = AdvState.Loading;
      AdRequest request = new AdRequest.Builder().Build();
      // Create an interstitial.
      RewardedInterstitialAd.LoadAd(adUnitId, request, (rewardedInterstitialAd, error) =>
        {
          if (error != null)
          {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
              Debug.Log("RewardedInterstitialAd load failed, error: " + error);
            });
            state = AdvState.NoLoad;
            return;
          }
          this.rewardedInterstitialAd = rewardedInterstitialAd;
          MobileAdsEventExecutor.ExecuteInUpdate(() =>
          {
            Debug.Log("RewardedInterstitialAd loaded");
            state = AdvState.Loaded;
          });
          // Register for ad events.
          this.rewardedInterstitialAd.OnAdDidPresentFullScreenContent += OnAdDidPresentFullScreenContent;
          this.rewardedInterstitialAd.OnAdDidDismissFullScreenContent += OnAdDidDismissFullScreenContent;
          this.rewardedInterstitialAd.OnAdFailedToPresentFullScreenContent += OnAdFailedToPresentFullScreenContent;
        });
    }

    public void Show(Action<bool> rewardefunc)
    {
      this.RewardedFuncCall = rewardefunc;
      if (state == AdvState.Loaded)
      {
        this.rewardedInterstitialAd.Show((reward) =>
        {
          MobileAdsEventExecutor.ExecuteInUpdate(() =>
          {
            this?.RewardedFuncCall(true);
            Request();
          });
        });
      }
      else
      {
        this?.RewardedFuncCall(false);
        if (state == AdvState.NoLoad)
          Request();
      }
    }

    public void Hide()
    {
      this.rewardedInterstitialAd?.Destroy();
      this?.RewardedFuncCall(false);
      this.rewardedInterstitialAd = null;
      this.RewardedFuncCall = null;
      state = AdvState.NoLoad;
    }

    public void OnAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
      MobileAdsEventExecutor.ExecuteInUpdate(() =>
      {
        Debug.Log("Rewarded Interstitial presented");
        state = AdvState.NoLoad;
        Request();
      });
    }
    public void OnAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
      MobileAdsEventExecutor.ExecuteInUpdate(() =>
      {
        Debug.Log("Rewarded Interstitial dismissed");
        this?.RewardedFuncCall(false);
        state = AdvState.NoLoad;
        Request();
      });
      this.rewardedInterstitialAd = null;
    }
    public void OnAdFailedToPresentFullScreenContent(object sender, EventArgs args)
    {
      MobileAdsEventExecutor.ExecuteInUpdate(() =>
      {
        Debug.Log("Rewarded Interstitial failed to present");
        state = AdvState.NoLoad;
        Request();
      });
      this.rewardedInterstitialAd = null;
    }
  }
}