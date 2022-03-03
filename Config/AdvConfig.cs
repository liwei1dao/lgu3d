using System;

namespace lgu3d
{
  /// <summary>
  /// 广告初始化完毕
  /// </summary>
  /// <param name="adv"></param>
  public delegate void AdvInitializationEvent(IAdv adv,bool isinitialization,string message);
  /// <summary>
  /// 广告加载完毕
  /// </summary>
  /// <param name="atype"></param>
  /// <param name="isload"></param>
  public delegate void AdvLoadEvent(AdvType atype,bool isload);
  /// <summary>
  /// 广告奖励
  /// </summary>
  /// <param name="atype"></param>
  /// <param name="isload"></param>
  public delegate void AdvRewardEvent(AdvType atype,bool isload);
  public enum AdvvWeights : int
  {
    Low,
    Medium,
    High,
    VeryHigh,
  }
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
    AdvvWeights Weights { get; set; }
    AdvInitializationEvent InitializationEvent { get; set; }
    AdvLoadEvent LoadEvent { get; set; }
    AdvRewardEvent RewardEvent { get; set; }
    #region 开屏广告
    void OpenAd_Load();
    bool OpenAd_IsReady();
    void OpenAd_Show();
    void OpenAd_Hide();
    #endregion
    #region 横屏广告
    void BannerAd_Load(AdPosition advpos);
    bool BannerAd_IsReady();
    void BannerAd_Show(AdPosition advpos);
    void BannerAd_Hide();
    #endregion
    #region 插屏广告
    void Intersitial_Load();
    bool Intersitial_IsReady();
    void Intersitial_Show();
    void Intersitial_Hide();
    #endregion

    #region 视频激励广告
    void Video_RewardedAd_Load();
    bool Video_RewardedAd_IsReady();
    void Video_RewardedAd_Show();
    void Video_RewardedAd_Hide();
    #endregion

    #region 插屏激励广告
    void Interstitial_RewardedAd_Load();
    void Interstitial_RewardedAd_Show();
    void Interstitial_RewardedAd_Hide();
    bool Interstitial_RewardedAd_IsReady();
    #endregion
  }

}