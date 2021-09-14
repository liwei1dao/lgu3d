using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;

namespace lgu3d
{

  public static class ImageExtend
  {
    public static IEnumerator DownloadImage(this Image image, string url)
    {
      UnityWebRequest www = new UnityWebRequest(url);
      DownloadHandlerTexture dowle = new DownloadHandlerTexture();
      www.downloadHandler = dowle;
      yield return www.SendWebRequest();

      if (www.result == UnityWebRequest.Result.Success)
      {
        image.sprite = Sprite.Create(dowle.texture, new Rect(0, 0, dowle.texture.width, dowle.texture.height), new Vector2(0, 0));
      }
      else
      {
        Debug.LogError("DownloadImage err" + www.error);
      }
    }
  }
}