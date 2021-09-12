using System;
using UnityEngine;
using System.Collections.Generic;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
namespace lgu3d
{
  public class AdmobModule : ManagerContorBase<AdmobModule>, IAdv
  {
    public Dictionary<AdvType, string> advs;
    private Admob_OpenAd_Comp OpenAd_Comp;
    private Admob_BannerAd_Comp BannerAd_Comp;  //横幅广告
    private Admob_InterstitialAd_Comp InterstitialAd_Comp; //插屏广告
    private Admob_Video_RewardedAd_Comp Video_RewardedAd_Comp;     //激励广
    private Admob_Interstitial_RewardedAd_Comp Interstitial_RewardedAd_Comp; //插屏奖励广告
    public override void Load(params object[] agrs)
    {
      if (agrs.Length == 1 && agrs[0] is Dictionary<AdvType, string>)
      {
        advs = agrs[0] as Dictionary<AdvType, string>;
        OpenAd_Comp = AddComp<Admob_OpenAd_Comp>();
        BannerAd_Comp = AddComp<Admob_BannerAd_Comp>();
        InterstitialAd_Comp = AddComp<Admob_InterstitialAd_Comp>();
        Video_RewardedAd_Comp = AddComp<Admob_Video_RewardedAd_Comp>();
        Interstitial_RewardedAd_Comp = AddComp<Admob_Interstitial_RewardedAd_Comp>();
        MobileAds.Initialize(initStatus =>
        {
          base.Load(agrs);
        });
      }
      else
      {
        Debug.LogError("AdmobModule 启动参数错误，请检查代码");
      }
    }

    #region 接口
    public void OpenAd_Show()
    {
      OpenAd_Comp.Show();
    }
    public void OpenAd_Hide()
    {
      OpenAd_Comp.Hide();
    }
    public void BannerAd_Show()
    {
      BannerAd_Comp.Show();
    }
    public void BannerAd_Hide()
    {
      BannerAd_Comp.Hide();
    }
    public void Intersitial_Show()
    {
      InterstitialAd_Comp.Show();
    }
    public void Intersitial_Hide()
    {
      InterstitialAd_Comp.Hide();
    }
    public void Video_RewardedAd_Show(Action<bool> backcall)
    {
      Video_RewardedAd_Comp.Show(backcall);
    }
    public void Video_RewardedAd_Hide()
    {
      Video_RewardedAd_Comp.Hide();
    }
    public void Interstitial_RewardedAd_Show(Action<bool> backcall)
    {
      Interstitial_RewardedAd_Comp.Show(backcall);
    }
    public void Interstitial_RewardedAd_Hide()
    {
      Interstitial_RewardedAd_Comp.Hide();
    }
    #endregion
  }
}