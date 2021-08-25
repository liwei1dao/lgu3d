using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace lgu3d
{
  public delegate void LanguageModule_ChangeLanguage();
  /// <summary>
  /// 语言模块
  /// </summary>
  public class LanguageModule : ManagerContorBase<LanguageModule>
  {
    public string[] Language;
    public string SelectLanguage;
    public Language_Data_Comp Data_Comp;
    public LanguageModule_ChangeLanguage chanagelanguage;
    public override void Load(params object[] agr)
    {
      base.Load(agr);
      if (agr.Length == 2)
      {
        Language = (string[])agr[0];
        SelectLanguage = (string)agr[1];
        ResourceComp = AddComp<Module_ResourceComp>();
        Data_Comp = AddComp<Language_Data_Comp>();
      }
      else
      {
        Debug.LogError("LanguageModule 启动参数错误，请检查代码");
      }
    }

    public virtual void RegisterChangeLanguage(LanguageModule_ChangeLanguage _delegate)
    {
      this.chanagelanguage += _delegate;
    }
    public virtual void UnRegisterChangeLanguage(LanguageModule_ChangeLanguage _delegate)
    {
      this.chanagelanguage -= _delegate;
    }


    public virtual void ChangeLanguage(string selectLanguage)
    {
      if (Language.Contains(selectLanguage))
      {
        SelectLanguage = selectLanguage;
        this.chanagelanguage();
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