using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 语言模块
  /// </summary>
  public class LanguageModule : ManagerContorBase<EventModule>
  {
    public string[] Language;
    public string SelectLanguage;
    public Language_Data_Comp Data_Comp;
    public override void Load(params object[] agr)
    {
      base.Load(agr);
      if (agr.Length == 2)
      {
        Language = (string[])agr[0];
        SelectLanguage = (string)agr[1];
        Data_Comp = AddComp<Language_Data_Comp>();
      }
      else
      {
        Debug.LogError("LanguageModule 启动参数错误，请检查代码");
      }
    }

    public virtual void ChangeLanguage(string selectLanguage)
    {
      if (Language.Contains(selectLanguage))
      {
        SelectLanguage = selectLanguage;
      }
      else
      {
        Debug.LogError(string.Format("LanguageModule ChangeLanguage {0} found", selectLanguage));
      }
    }

    public virtual string GetStatement(string key)
    {
      return Data_Comp.GetStatement(key);
    }
  }
}