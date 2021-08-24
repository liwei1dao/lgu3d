using UnityEngine;

namespace lgu3d
{
  [System.Serializable]
  public class Statement : ConfigDataBase<string>
  {
    public string Value;
  }

  [CreateAssetMenu(fileName = "LanguageTable", menuName = "Data/LanguageTable", order = 1)]
  public class LanguageTable : ConfigTableDataBase<string, Statement>
  {

  }
}