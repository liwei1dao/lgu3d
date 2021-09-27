using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  public class CommonModule : ManagerContorBase<CommonModule>
  {
    private LoadingViewComp LoadingViewComp;
    private MessageBoxViewComp MessageBoxComp;
    private TipsViewComp TipsViewComp;
    public override void Load(params object[] _Agr)
    {
      ResourceComp = AddComp<Module_ResourceComp>();
      MessageBoxComp = AddComp<MessageBoxViewComp>();
      TipsViewComp = AddComp<TipsViewComp>();
      LoadingViewComp = AddComp<LoadingViewComp>();
      base.Load(_Agr);
    }
    public void ShowBox(string msg, Action confirmFun, Action cancelFun)
    {
      MessageBoxComp.ShowBox(msg, confirmFun, cancelFun);
    }

    public void ShowTips(string message, float time)
    {
      TipsViewComp.ShowTips(message, time);
    }

    public void ShowLoadingView()
    {
      LoadingViewComp.Show();
    }
    public void HideLoadingView()
    {
      LoadingViewComp.Hide();
    }
  }
}

