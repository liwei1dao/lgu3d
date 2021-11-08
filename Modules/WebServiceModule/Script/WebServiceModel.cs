using System.Collections;
using UnityEngine.Networking;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace lgu3d
{
    public enum HttpMethod {
        Get,
        Post,
    }

    public delegate void MsgRequestBack(Hashtable data);

    /// <summary>
    /// web服务器 模块控制器
    /// </summary>
    public class WebServiceModule : ManagerContorBase<WebServiceModule>
    {
        public override void Load(params object[] _Agr)
        {
            CoroutineComp = AddComp<Module_CoroutineComp>();
            base.Load(_Agr);
        }

        /// <summary>
        /// 检查网络环境
        /// </summary>
        /// <returns></returns>
        public bool DetectNetwork()
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)//没有网络
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="action"></param>
        public void Get(string url, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(_Get(url, actionResult));
        }

        /// <summary>
        /// 向服务器提交post请求
        /// </summary>
        /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
        /// <param name="lstformData">
        /// form表单参数
        /// List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
        /// formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
        /// formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
        /// </param>
        /// <param name="lstformData">处理返回结果的委托,处理请求对象</param>
        /// <returns></returns>
        public void Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(_Post(serverURL, lstformData, actionResult));
        }
        /// <summary>
        /// 向服务器提交post请求
        /// </summary>
        /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
        /// <param name="lstformData"> form表单参数</param>
        /// <param name="lstformData">处理返回结果的委托,处理请求对象</param>
        /// <returns></returns>
        public void Post(string serverURL, byte[] databyte, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(_Post(serverURL, databyte, actionResult));
        }
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
        /// <returns></returns>
        public void DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            StartCoroutine(_DownloadFile(url, downloadFilePathAndName, actionResult));
        }

        /// <summary>
        /// 请求图片
        /// </summary>
        /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
        /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
        /// <returns></returns>
        public void GetTexture(string url, Action<Texture2D> actionResult)
        {
            StartCoroutine(_GetTexture(url, actionResult));
        }

        /// <summary>
        /// 请求AssetBundle
        /// </summary>
        /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
        /// <returns></returns>
        public void GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            StartCoroutine(_GetAssetBundle(url, actionResult));
        }

        /// <summary>
        /// 请求服务器地址上的音效
        /// </summary>
        /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
        /// <param name="audioType">音效类型</param>
        /// <returns></returns>
        public void GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            StartCoroutine(_GetAudioClip(url, actionResult, audioType));
        }

        /// <summary>
        /// 通过PUT方式将字节流传到服务器
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="contentBytes">需要上传的字节流</param>
        /// <param name="resultAction">处理返回结果的委托</param>
        /// <returns></returns>
        public void UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult)
        {
            StartCoroutine(_UploadByPut(url, contentBytes, actionResult, ""));
        }

        #region 请求
        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="url">请求地址,like 'http://www.my-server.com/ '</param>
        /// <param name="action">请求发起后处理回调结果的委托</param>
        /// <returns></returns>
        IEnumerator _Get(string url, Action<UnityWebRequest> actionResult)
        {
            using (UnityWebRequest uwr = UnityWebRequest.Get(url))
            {
                yield return uwr.SendWebRequest();
                if (actionResult != null)
                {
                    actionResult(uwr);
                }
            }
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="downloadFilePathAndName">储存文件的路径和文件名 like 'Application.persistentDataPath+"/unity3d.html"'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求对象</param>
        /// <returns></returns>
        IEnumerator _DownloadFile(string url, string downloadFilePathAndName, Action<UnityWebRequest> actionResult)
        {
            var uwr = new UnityWebRequest(url, UnityWebRequest.kHttpVerbGET);
            uwr.downloadHandler = new DownloadHandlerFile(downloadFilePathAndName);
            yield return uwr.SendWebRequest();
            if (actionResult != null)
            {
                actionResult(uwr);
            }
        }

        /// <summary>
        /// 请求图片
        /// </summary>
        /// <param name="url">图片地址,like 'http://www.my-server.com/image.png '</param>
        /// <param name="action">请求发起后处理回调结果的委托,处理请求结果的图片</param>
        /// <returns></returns>
        IEnumerator _GetTexture(string url, Action<Texture2D> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(url);
            DownloadHandlerTexture downloadTexture = new DownloadHandlerTexture(true);
            uwr.downloadHandler = downloadTexture;
            yield return uwr.SendWebRequest();
            Texture2D t = null;
            if (!(uwr.isNetworkError || uwr.isHttpError))
            {
                t = downloadTexture.texture;
            }
            if (actionResult != null)
            {
                actionResult(t);
            }
        }

        /// <summary>
        /// 请求AssetBundle
        /// </summary>
        /// <param name="url">AssetBundle地址,like 'http://www.my-server.com/myData.unity3d'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AssetBundle</param>
        /// <returns></returns>
        IEnumerator _GetAssetBundle(string url, Action<AssetBundle> actionResult)
        {
            UnityWebRequest www = new UnityWebRequest(url);
            DownloadHandlerAssetBundle handler = new DownloadHandlerAssetBundle(www.url, uint.MaxValue);
            www.downloadHandler = handler;
            yield return www.SendWebRequest();
            AssetBundle bundle = null;
            if (!(www.isNetworkError || www.isHttpError))
            {
                bundle = handler.assetBundle;
            }
            if (actionResult != null)
            {
                actionResult(bundle);
            }
        }

        /// <summary>
        /// 请求服务器地址上的音效
        /// </summary>
        /// <param name="url">没有音效地址,like 'http://myserver.com/mysound.wav'</param>
        /// <param name="actionResult">请求发起后处理回调结果的委托,处理请求结果的AudioClip</param>
        /// <param name="audioType">音效类型</param>
        /// <returns></returns>
        IEnumerator _GetAudioClip(string url, Action<AudioClip> actionResult, AudioType audioType = AudioType.WAV)
        {
            using (var uwr = UnityWebRequestMultimedia.GetAudioClip(url, audioType))
            {
                yield return uwr.SendWebRequest();
                if (!(uwr.isNetworkError || uwr.isHttpError))
                {
                    if (actionResult != null)
                    {
                        actionResult(DownloadHandlerAudioClip.GetContent(uwr));
                    }
                }
            }
        }

        /// <summary>
        /// 向服务器提交post请求
        /// </summary>
        /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
        /// <param name="lstformData">form表单参数</param>
        /// <param name="lstformData">处理返回结果的委托</param>
        /// <returns></returns>
        IEnumerator _Post(string serverURL, List<IMultipartFormSection> lstformData, Action<UnityWebRequest> actionResult)
        {
            //List<IMultipartFormSection> formData = new List<IMultipartFormSection>();
            //formData.Add(new MultipartFormDataSection("field1=foo&field2=bar"));
            //formData.Add(new MultipartFormFileSection("my file data", "myfile.txt"));
            UnityWebRequest uwr = UnityWebRequest.Post(serverURL, lstformData);
            yield return uwr.SendWebRequest();
            if (actionResult != null)
            {
                actionResult(uwr);
            }
        }

        /// <summary>
        /// 向服务器提交post请求
        /// </summary>
        /// <param name="serverURL">服务器请求目标地址,like "http://www.my-server.com/myform"</param>
        /// <param name="lstformData">form表单参数</param>
        /// <param name="lstformData">处理返回结果的委托</param>
        /// <returns></returns>
        IEnumerator _Post(string serverURL, byte[] databyte, Action<UnityWebRequest> actionResult)
        {
            UnityWebRequest uwr = new UnityWebRequest(serverURL, UnityWebRequest.kHttpVerbPOST);
            uwr.uploadHandler = new UploadHandlerRaw(databyte);
            uwr.downloadHandler = new DownloadHandlerBuffer();
            uwr.SetRequestHeader("Content-Type", "application/json;charset=utf-8");
            yield return uwr.SendWebRequest();
            if (actionResult != null)
            {
                actionResult(uwr);
            }
        }

        /// <summary>
        /// 通过PUT方式将字节流传到服务器
        /// </summary>
        /// <param name="url">服务器目标地址 like 'http://www.my-server.com/upload' </param>
        /// <param name="contentBytes">需要上传的字节流</param>
        /// <param name="resultAction">处理返回结果的委托</param>
        /// <param name="resultAction">设置header文件中的Content-Type属性</param>
        /// <returns></returns>
        IEnumerator _UploadByPut(string url, byte[] contentBytes, Action<bool> actionResult, string contentType = "application/octet-stream")
        {
            UnityWebRequest uwr = new UnityWebRequest();
            UploadHandler uploader = new UploadHandlerRaw(contentBytes);

            // Sends header: "Content-Type: custom/content-type";
            uploader.contentType = contentType;

            uwr.uploadHandler = uploader;

            yield return uwr.SendWebRequest();

            bool res = true;
            if (uwr.isNetworkError || uwr.isHttpError)
            {
                res = false;
            }
            if (actionResult != null)
            {
                actionResult(res);
            }
        }
        #endregion
    }

}
