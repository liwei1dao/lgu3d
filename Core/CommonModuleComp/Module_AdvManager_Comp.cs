using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 模块音频组件
  /// </summary>
  public class Module_AdvManager_Comp<C> : ModelCompBase<C> where C : ModelBase, new()
  {
    protected List<IAdv> advs;
    #region 框架构造
    public override void Load(ModelBase module, params object[] agrs)
    {
      advs = new List<IAdv>();
      base.Load(module, agrs);
    }
    #endregion

    /// <summary>
    /// 加载广告
    /// </summary>
    /// <param name="adv"></param>
    /// <param name="advpos"></param>
    /// <param name="backcall"></param>
    public void LoadAdv(AdvType adv, AdPosition advpos, Action<bool> backcall = null)
    {
      switch (adv)
      {
        case AdvType.AppOpenAd:
          for (var i = 0; i < advs.Count; i++)
          {
            advs[i].OpenAd_Load(backcall);
          }
          break;
        case AdvType.BannerAd:
          for (var i = 0; i < advs.Count; i++)
          {
            advs[i].BannerAd_Load(advpos, backcall);
          }
          break;
        case AdvType.IntersitialAd:
          for (var i = 0; i < advs.Count; i++)
          {
            advs[i].Intersitial_Load(backcall);
          }
          break;
        case AdvType.Video_RewardedAd:
          for (var i = 0; i < advs.Count; i++)
          {
            advs[i].Video_RewardedAd_Load(backcall);
          }
          break;
        case AdvType.IntersitialAd_RewardedAd:
          for (var i = 0; i < advs.Count; i++)
          {
            advs[i].Interstitial_RewardedAd_Load(backcall);
          }
          break;
      }
    }

    /// <summary>
    /// 广告是否准备好
    /// </summary>
    /// <param name="adv"></param>
    /// <returns></returns>
    public bool IsReady(AdvType adv)
    {
      bool isReady = false;
      for (var i = 0; i < advs.Count; i++)
      {
        switch (adv)
        {
          case AdvType.AppOpenAd:
            isReady = advs[i].OpenAd_IsReady();
            break;
          case AdvType.BannerAd:
            isReady = advs[i].BannerAd_IsReady();
            break;
          case AdvType.IntersitialAd:
            isReady = advs[i].Intersitial_IsReady();
            break;
          case AdvType.Video_RewardedAd:
            isReady = advs[i].Video_RewardedAd_IsReady();
            break;
          case AdvType.IntersitialAd_RewardedAd:
            isReady = advs[i].Interstitial_RewardedAd_IsReady();
            break;
        }
        if (isReady)
        {
          return isReady;
        }
      }
      return isReady;
    }


    public void ShowAdv(AdvType adv)
    {
      switch (adv)
      {
        case AdvType.AppOpenAd:
          for (var i = 0; i < advs.Count; i++)
          {
            if (advs[i].OpenAd_IsReady())
            {
              advs[i].OpenAd_Show();
              break;
            }
          }
          break;
        case AdvType.IntersitialAd:
          for (var i = 0; i < advs.Count; i++)
          {
            if (advs[i].Intersitial_IsReady())
            {
              advs[i].Intersitial_Show();
              break;
            }
          }
          break;
      }
    }

    /// <summary>
    /// 显示横幅广告
    /// </summary>
    /// <param name="advpos"></param>
    public void ShowBanner(AdPosition advpos)
    {
      for (var i = 0; i < advs.Count; i++)
      {
        if (advs[i].BannerAd_IsReady())
        {
          advs[i].BannerAd_Show(advpos);
          break;
        }
      }
    }

    /// <summary>
    /// 显示激励广告
    /// </summary>
    /// <param name="adv"></param>
    /// <param name="call"></param>
    public void ShowRewardedAdv(AdvType adv, Action<bool> call)
    {
      switch (adv)
      {
        case AdvType.Video_RewardedAd:
          for (var i = 0; i < advs.Count; i++)
          {
            if (advs[i].Video_RewardedAd_IsReady())
            {
              advs[i].Video_RewardedAd_Show(call);
              break;
            }
          }
          break;
        case AdvType.IntersitialAd:
          for (var i = 0; i < advs.Count; i++)
          {
            if (advs[i].Interstitial_RewardedAd_IsReady())
            {
              advs[i].Interstitial_RewardedAd_Show(call);
              break;
            }
          }
          break;
      }
    }

    /// <summary>
    /// 关闭广告
    /// </summary>
    /// <param name="adv"></param>
    public void HideAdv(AdvType adv)
    {
      for (var i = 0; i < advs.Count; i++)
      {
        switch (adv)
        {
          case AdvType.AppOpenAd:
            advs[i].OpenAd_Hide();
            break;
          case AdvType.BannerAd:
            advs[i].BannerAd_Hide();
            break;
          case AdvType.IntersitialAd:
            advs[i].Intersitial_Hide();
            break;
          case AdvType.Video_RewardedAd:
            advs[i].Video_RewardedAd_Hide();
            break;
          case AdvType.IntersitialAd_RewardedAd:
            advs[i].Interstitial_RewardedAd_Hide();
            break;
        }
      }
    }
  }
}