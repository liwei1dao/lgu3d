using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class CaptureCamera : MonoBehaviour
{
    public Camera camera;
    public Button butt;

    // 相机截图
    RenderTexture CameraRT;

    private void Start()
    {
        butt.onClick.AddListener(() =>
        {
            RenderTexture tex = captureCamera();
            saveRenderTexture(tex);
        });
    }

    /// <summary>
    /// 抓取相机截图
    /// </summary>
    /// <returns></returns>
    public RenderTexture captureCamera()
    {
        if (CameraRT != null)
        {
            //释放缓冲区
            RenderTexture.ReleaseTemporary(CameraRT);
        }

        int rtW = Screen.width;
        int rtH = Screen.height;

        // 建一个RenderTexture对象 
        CameraRT = RenderTexture.GetTemporary(rtW, rtH, 512);
        // 临时设置相关相机的targetTexture为rt, 并手动渲染相关相机 
        camera.targetTexture = CameraRT;
        camera.Render();
        // 重置相关参数，以使用 camera 继续在屏幕上显示 
        camera.targetTexture = null;

        return CameraRT;
    }
    private void saveRenderTexture(RenderTexture rt)
    {
        RenderTexture active = RenderTexture.active;
        RenderTexture.active = rt;
        Texture2D png = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        png.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        png.Apply();
        RenderTexture.active = active;
        byte[] bytes = png.EncodeToPNG();
        string path = string.Format("Assets/rt_{0}_{1}_{2}.png", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
        FileStream fs = File.Open(path, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(fs);
        writer.Write(bytes);
        writer.Flush();
        writer.Close();
        fs.Close();
        Destroy(png);
        png = null;
        Debug.Log("保存成功！" + path);
    }
}
