using System;
using System.Collections.Generic;
using UnityEngine;

namespace lgu3d
{
  /// <summary>
  /// 语言数据模块
  /// </summary>
  public class Language_Data_Comp : ModelCompBase<LanguageModule>
  {

    public LanguageTable Languages;

    #region 框架构造
    public override void Load(ModuleBase module, params object[] _Agr)
    {
      base.Load(module);
      Languages = MyModule.LoadAsset<LanguageTable>("Data", "LanguageTable");
      base.LoadEnd();
    }
    #endregion

    public virtual string GetStatement(string key)
    {
      Statement item = Languages.GetData(key);
      switch (MyModule.SelectLanguage)
      {
        case LanguageType.ZH:
          return item.ZH;
        case LanguageType.EN:
          return item.EN;
        case LanguageType.ES:
          return item.ES;
        case LanguageType.HI:
          return item.HI;
        case LanguageType.ID:
          return item.ID;
        case LanguageType.VI:
          return item.VI;
        case LanguageType.RU:
          return item.RU;
        case LanguageType.JA:
          return item.JA;
        default:
          return "";
      }
    }
  }
}