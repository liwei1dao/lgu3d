using UnityEngine;
using GoogleMobileAds.Api;
using System;


namespace lgu3d
{
  /// <summary>
  /// Admob广告模块 Banner广告
  /// </summary>
  public class Admob_BannerAd_Comp : ModelCompBase<AdmobModule>
  {
    private string adUnitId;
    private BannerView bannerView;

    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      if (MyModule.advs.ContainsKey(AdvType.BannerAd))
      {
        adUnitId = MyModule.advs[AdvType.BannerAd];
      }
      LoadEnd();
    }
    /// <summary>
    /// 请求Banner广告
    /// </summary>
    public void Show()
    {

      if (string.IsNullOrEmpty(adUnitId))
      {
        Debug.LogError("AdmobModule Admob_BannerAd_Comp No adUnitId");
        return;
      }

      /// <summary>
      /// BannerView 的构造函数包含以下参数：
      /// adUnitId - AdMob 广告单元 ID，BannerView 应通过该 ID 加载广告。
      /// AdSize - 要使用的 AdMob 广告尺寸（有关详情，请参阅横幅广告尺寸）。
      /// AdPosition - 应放置横幅广告的位置。AdPosition 枚举列出了有效的广告位置值。
      /// </summary>
      this.bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Top);

      // 广告请求成功加载后调用。
      this.bannerView.OnAdLoaded += this.HandleOnAdLoaded;
      // 广告请求加载失败时调用。
      this.bannerView.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
      // 点击广告时调用。
      this.bannerView.OnAdOpening += this.HandleOnAdOpened;
      // 用户点击广告后从应用返回时调用。
      this.bannerView.OnAdClosed += this.HandleOnAdClosed;
      // Create an empty ad request.
      AdRequest request = new AdRequest.Builder().Build();
      // Load the banner with the request.
      this.bannerView.LoadAd(request);
    }

    public void Hide()
    {
      bannerView?.Destroy();
    }

    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
      Debug.Log("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
      Debug.Log("HandleFailedToReceiveAd event received with message: "
                          + args.LoadAdError.ToString());
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
      Debug.Log("HandleAdOpened event received");
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
      Debug.Log("HandleAdClosed event received");
    }



  }
}