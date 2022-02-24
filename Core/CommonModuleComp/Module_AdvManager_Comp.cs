using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 模块音频组件
  /// </summary>
  public class Module_AdvManager_Comp : ModelCompBase
  {
    private List<IAdv> advs;
    #region 框架构造
    public override void Load(ModelBase module, params object[] agr)
    {
      base.Load(module);
      advs = new List<IAdv>();
      base.LoadEnd();
    }
    #endregion

    public void LoadAdv(AdvType adv)
    {

    }

    public void ShowAdv(AdvType adv)
    {

    }
    public void ShowRewardedAdv(AdvType adv, Action<bool> call)
    {

    }
    public void HideAdv(AdvType adv)
    {

    }
  }
}