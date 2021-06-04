using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineController : MonoBehaviour
{
  [SerializeField] private TextMeshProUGUI baseline;
  [SerializeField] private TextMeshProUGUI colorline;

  public void TextLineOnUpDate(string line, float progress)
  {
    baseline.text = line;
    colorline.text = line;
    float x = colorline.preferredWidth * progress;
    colorline.rectTransform.sizeDelta = new Vector2(x, colorline.rectTransform.sizeDelta.y);
  }
}
