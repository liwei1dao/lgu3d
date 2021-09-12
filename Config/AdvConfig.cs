using System;

namespace lgu3d
{

  public enum AdvType
  {
    AppOpenAd,//开屏广告
    BannerAd, //横幅广告
    IntersitialAd,//插屏广告
    Video_RewardedAd,//视频奖励广告
    IntersitialAd_RewardedAd,//插屏奖励广告
  }

  public enum AdvState
  {
    NoLoad, //未加载
    Loading,//加载中
    Loaded,//加载完毕
  }

  public interface IAdv
  {
    void OpenAd_Show();
    void OpenAd_Hide();
    void BannerAd_Show();
    void BannerAd_Hide();
    void Intersitial_Show();
    void Intersitial_Hide();
    void Video_RewardedAd_Show(Action<bool> backcall);
    void Video_RewardedAd_Hide();
    void Interstitial_RewardedAd_Show(Action<bool> backcall);
    void Interstitial_RewardedAd_Hide();
  }

}