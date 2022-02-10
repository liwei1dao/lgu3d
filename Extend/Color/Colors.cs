using System.IO;
using UnityEngine;

namespace lgu3d
{
  public class Colors
  {
    /// <summary>
    /// 随机颜色
    /// </summary>
    /// <returns></returns>
    public static Color RandomColor()
    {
      //随机颜色的HSV值,饱和度不变，只改变H值
      //H、S、V三个值的范围都是在0~1之间
      float h = Random.Range(0f, 1f);//随机值
      float s = 0.3f;//设置饱和度为定值
      Color color = Color.HSVToRGB(h, s, 1);
      return color;
    }

    /// <summary>
    /// 随机文字颜色
    /// </summary>
    /// <returns></returns>
    public static string RandomColorString()
    {
      string[] colors = new string[] { "black", "blue", "brown", "darkblue", "fuchsia", "green", "grey", "lightblue", "lime", "maroon", "navy", "olive", "orange", "purple", "red", "silver", "teal", "white", "yellow" };
      return colors[Random.Range(0, colors.Length - 1)];
    }

  }
}