using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lgu3d
{
  public enum LanguageType
  {
    ZH, //中文
    EN,//英文
    ES,//西班牙文
    HI,//印地语言
    ID,//印度尼西亚
    VI,//越南文
    RU,//俄罗斯
    JA,//日语
  }
  public delegate void LanguageModule_ChangeLanguage();
  /// <summary>
  /// 语言模块
  /// </summary>
  public class LanguageModule : ManagerContorBase<LanguageModule>
  {
    public LanguageType SelectLanguage;
    public Language_Data_Comp Data_Comp;
    public LanguageModule_ChangeLanguage chanagelanguage;
    public override void Load(params object[] agr)
    {
      base.Load(agr);
      if (agr.Length == 1)
      {
        SelectLanguage = (LanguageType)agr[0];
        ResourceComp = AddComp<Module_ResourceComp>();
        Data_Comp = AddComp<Language_Data_Comp>();
      }
      else
      {
        Debug.LogError("LanguageModule 启动参数错误，请检查代码");
      }
    }

    public virtual string GetStatement(string key)
    {
      return Data_Comp.GetStatement(key);
    }

    public virtual void ChanageLanguage(LanguageType selectLanguage)
    {
      SelectLanguage = selectLanguage;
      this.chanagelanguage();
    }
  }
}