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
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
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
        case LanguageType.HI:
          return item.HI;
        case LanguageType.DI:
          return item.DI;
        case LanguageType.VI:
          return item.ZH;
        case LanguageType.RU:
          return item.RU;
        default:
          return "";
      }
    }
  }
}