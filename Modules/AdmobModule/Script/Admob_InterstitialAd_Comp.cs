using UnityEngine;
using GoogleMobileAds.Api;
using System;
namespace lgu3d
{
  /// <summary>
  /// Admob广告模块 Interstitial广告
  /// </summary>
  public class Admob_InterstitialAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private InterstitialAd interstitial;

    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      if (MyModule.advs.ContainsKey(AdvType.IntersitialAd))
      {
        adUnitId = MyModule.advs[AdvType.IntersitialAd];
      }
      LoadEnd();
    }

    /// <summary>
    /// 请求Interstitial广告
    /// </summary>
    public void Request()
    {
      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("AdmobModule Admob_InterstitialAd_Comp No adUnitId");
        return;
      }
      // Initialize an InterstitialAd.
      this.interstitial = new InterstitialAd(adUnitId);

      // 广告请求成功加载后调用。
      this.interstitial.OnAdLoaded += HandleOnAdLoaded;
      // 广告请求加载失败时调用。
      this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
      // 显示广告时调用。
      this.interstitial.OnAdOpening += HandleOnAdOpened;
      // 关闭广告时调用。
      this.interstitial.OnAdClosed += HandleOnAdClosed;
      // Create an empty ad request.
      AdRequest request = new AdRequest.Builder().Build();
      // Load the interstitial with the request.
      this.interstitial.LoadAd(request);
    }

    public void Show()
    {
      if (this.interstitial.IsLoaded())
      {
        this.interstitial.Show();
      }
    }
    public void Hide()
    {
      this.interstitial?.Destroy();
      this.interstitial = null;
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
      Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
      Debug.Log("HandleFailedToReceiveAd event received with message: "
                          + args.LoadAdError.ToString());
      this.Request();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
      Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
      Debug.Log("HandleAdClosed event received");
      this.interstitial.Destroy();
      this.Request();
    }
  }
}