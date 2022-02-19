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
    Playing,//播放中
    Loaded,//加载完毕
  }

  public enum AdPosition
  {
    Top = 0,
    Bottom = 1,
    TopLeft = 2,
    TopRight = 3,
    BottomLeft = 4,
    BottomRight = 5,
    Center = 6
  }

  public interface IAdv
  {
    void OpenAd_Show();
    void OpenAd_Hide();
    bool OpenAd_IsReady();
    void BannerAd_Show(AdPosition advpos);
    void BannerAd_Hide();
    bool BannerAd_IsReady();
    void Intersitial_Show();
    void Intersitial_Hide();
    bool Intersitial_IsReady();
    void Video_RewardedAd_Show(Action<bool> backcall);
    void Video_RewardedAd_Hide();
    bool Video_RewardedAd_IsReady();
    void Interstitial_RewardedAd_Show(Action<bool> backcall);
    void Interstitial_RewardedAd_Hide();
    bool Interstitial_RewardedAd_IsReady();
  }

}