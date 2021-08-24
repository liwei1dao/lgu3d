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

    public Dictionary<string, LanguageTable> Languages;

    #region 框架构造
    public override void Load(ModelBase _ModelContorl, params object[] _Agr)
    {
      base.Load(_ModelContorl);
      Languages = new Dictionary<string, LanguageTable>();
      foreach (var item in MyModule.Language)
      {
        Languages[item] = MyModule.LoadAsset<LanguageTable>("Data", item);
      }
      base.LoadEnd();
    }
    #endregion

    public virtual string GetStatement(string key)
    {
      if (Languages.ContainsKey(MyModule.SelectLanguage))
      {
        return Languages[MyModule.SelectLanguage].GetData(key).Value;
      }
      else
      {
        Debug.LogError(string.Format("LanguageModule GetStatement {0} found", MyModule.SelectLanguage));
      }
      return "";
    }
  }
}