using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lgu3d
{
  public enum LanguageType : int
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
    public const string Localization = "Localization";
    public LanguageType SelectLanguage;
    public Language_Data_Comp Data_Comp;
    public LanguageModule_ChangeLanguage chanagelanguage;
    public override void Load(params object[] agrs)
    {
      base.Load(agrs);
      if (agrs.Length == 1)
      {
        SelectLanguage = (LanguageType)agrs[0];
        if (PlayerPrefs.HasKey(Localization))
        {
          SelectLanguage = (LanguageType)PlayerPrefs.GetInt(Localization);
        }
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
      PlayerPrefs.SetInt(Localization, ((int)SelectLanguage));
      this.chanagelanguage();
    }
  }
}