using UnityEngine;
using UnityEngine.UI;
namespace lgu3d
{
  /// <summary>
  /// UI组件用于切换语言使用
  /// </summary>
  public class Language : MonoBehaviour
  {
    [lgu3d_SerializeName("远程服务器")] public string key;
    private Text text;
    private void Awake()
    {
      text = GetComponent<Text>();
      if (text != null)
      {
        text.text = LanguageModule.Instance.GetStatement(key);
        LanguageModule.Instance.chanagelanguage += ChangeLanguage;
      }
    }

    private void OnDestroy()
    {
      if (text != null)
      {
        LanguageModule.Instance.chanagelanguage -= ChangeLanguage;
      }
    }

    private void ChangeLanguage()
    {
      if (text != null)
      {
        text.text = LanguageModule.Instance.GetStatement(key);
      }
    }
  }
}