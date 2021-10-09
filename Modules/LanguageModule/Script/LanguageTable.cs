using UnityEngine;

namespace lgu3d
{
  [System.Serializable]
  public class Statement : ConfigDataBase<string>
  {
    public string ZH;   //中文
    public string EN;   //中文
    public string HI;   //印地语言
    public string DI;   //印度尼西亚语
    public string VI;   //越南文
    public string RU;   //俄文
  }

  [CreateAssetMenu(fileName = "LanguageTable", menuName = "Data/LanguageTable", order = 1)]
  public class LanguageTable : ConfigTableDataBase<string, Statement>
  {

  }
}