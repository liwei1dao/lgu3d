using UnityEditor;
using UnityEngine;

namespace lgu3d.Editor
{
  [CreateAssetMenu(fileName = "DemoData", menuName = "Data/demodata")]
  public class DemoData : ScriptableObject
  {
    public string UserName;                   //用户名称
    public int Level;                         //用户等级
  }
}

