using UnityEngine;
using GoogleMobileAds.Api;
using System;


namespace lgu3d
{
  /// <summary>
  /// Admob广告模块 开屏广告
  /// </summary>
  public class Admob_OpenAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private AdvState state;
    private AppOpenAd ad;
    private bool isShowingAd;
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      state = AdvState.NoLoad;
      if (MyModule.advs.ContainsKey(AdvType.AppOpenAd))
      {
        adUnitId = MyModule.advs[AdvType.AppOpenAd];
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
        Debug.LogError("AdmobModule Admob_OpenAd_Comp No adUnitId");
        return;
      }
      if (state == AdvState.Loading)
      {
        return;
      }
      state = AdvState.Loading;
      AdRequest request = new AdRequest.Builder().Build();
      // Load an app open ad for portrait orientation
      AppOpenAd.LoadAd(adUnitId, ScreenOrientation.Portrait, request, ((appOpenAd, error) =>
      {
        if (error != null)
        {
          // Handle the error.
          Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
          state = AdvState.NoLoad;
          return;
        }
        // App open ad is loaded.
        ad = appOpenAd;
        state = AdvState.Loaded;
      }));
    }
    public void Show()
    {
      if (state != AdvState.Loaded)
      {
        if (state == AdvState.NoLoad)
          Request();
        return;
      }
      ad.OnAdDidDismissFullScreenContent += HandleAdDidDismissFullScreenContent;
      ad.OnAdFailedToPresentFullScreenContent += HandleAdFailedToPresentFullScreenContent;
      ad.OnAdDidPresentFullScreenContent += HandleAdDidPresentFullScreenContent;
      ad.OnAdDidRecordImpression += HandleAdDidRecordImpression;
      ad.OnPaidEvent += HandlePaidEvent;
      ad.Show();
    }

    public void Hide()
    {
      ad?.Destroy();
      state = AdvState.NoLoad;
    }

    private void HandleAdDidDismissFullScreenContent(object sender, EventArgs args)
    {
      Debug.Log("Closed app open ad");
      // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
      state = AdvState.NoLoad;
      Request();
    }

    private void HandleAdFailedToPresentFullScreenContent(object sender, AdErrorEventArgs args)
    {
      Debug.LogFormat("Failed to present the ad (reason: {0})", args.AdError.GetMessage());
      // Set the ad to null to indicate that AppOpenAdManager no longer has another ad to show.
      state = AdvState.NoLoad;
      Request();
    }

    private void HandleAdDidPresentFullScreenContent(object sender, EventArgs args)
    {
      Debug.Log("Displayed app open ad");
    }

    private void HandleAdDidRecordImpression(object sender, EventArgs args)
    {
      Debug.Log("Recorded ad impression");
    }

    private void HandlePaidEvent(object sender, AdValueEventArgs args)
    {
      Debug.LogFormat("Received paid event. (currency: {0}, value: {1}",
              args.AdValue.CurrencyCode, args.AdValue.Value);
    }
  }
}