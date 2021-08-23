using System;
using UnityEngine.UI;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 提示窗组件
  /// </summary>
  public class TipsViewComp : Model_BaseViewComp<CommonModule>
  {
    public override void Load(ModelBase module, params object[] agr)
    {
      base.Load(module, "MessageTipls", UILevel.HightUI);
    }


    public void ShowTips(string message, float time)
    {
      GameObject tips = GameObject.Instantiate<GameObject>(MyModule.LoadAsset<GameObject>("Prefab", "Tips"));
      tips.SetParent(UIGameobject);
      RectTransform rectTrans = tips.GetComponent<RectTransform>();
      rectTrans.sizeDelta = new Vector2(0, 0);
      rectTrans.anchoredPosition = new Vector2(0, 0);
      tips.OnSubmit<Text>("msg").text = message;
      GameObject.Destroy(tips, time);
    }
  }
}