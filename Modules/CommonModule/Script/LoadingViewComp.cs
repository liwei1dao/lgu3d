using System;
using UnityEngine.UI;

namespace lgu3d
{
  public class LoadingViewComp : Model_BaseViewComp<CommonModule>
  {
    public override void Load(ModuleBase module, params object[] agr)
    {
      base.Load(module, "LoadingView", UILevel.HightUI);
      Hide();
      LoadEnd();
    }
  }
}