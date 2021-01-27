//#if UNITY_IPHONE && !UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System;

namespace lgu3d {
    //微信接口组件 IOS
    public class Phone_IOSComp : PhoneComp
    {
        [DllImport("__Internal")]
        private static extern bool __initsdk_phone();
        [DllImport("__Internal")]
        private static extern void __callPhone(string phonenum);
        [DllImport("__Internal")]
        private static extern float __getBatteryLevel(); 
        [DllImport("__Internal")]
        private static extern string __getPastBoard();
        [DllImport("__Internal")]
        private static extern bool __copyText(string textStr);
        [DllImport("__Internal")]
        private static extern void __quitApp();
        [DllImport("__Internal")]
        private static extern void __shakeApp();
        [DllImport("__Internal")]
        private static extern void __startCaptureListener(bool start);
        [DllImport("__Internal")]
        private static extern void __openLocation(int reqID);
        /// <summary>
        /// 初始化sdk
        /// </summary>
        public override bool InitSdk()
        {
            return __initsdk_phone();
        }

        /// <summary>
        /// 打电话
        /// </summary>
        /// <returns></returns>
        public override void Callphone(string phonenum)
        {
            __callPhone(phonenum);
        }

        /// <summary>
        /// 获取电量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reqID"></param>
        public override float GetBatteryLevle()
        {
            return __getBatteryLevel();
        }

        /// <summary>
        /// 获取剪切板
        /// </summary>
        /// <param name="orderJson">订单</param>
        public override string GetPastBoard()
        {
            return __getPastBoard();
        }

        /// <summary>
        /// 复制文本
        /// </summary>
        /// <param name="textStr"></param>
        /// <returns></returns>
        public override bool CopyText(string textStr)
        {
            return __copyText(textStr);
        }

        /// <summary>
        ///退出APP
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="content">内容</param>
        public override void QuitApp()
        {
            __quitApp();
        }

        /// <summary>
        /// 震动APP
        /// </summary>
        public override void ShakeApp()
        {
            __shakeApp();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void StartCaptureListener(bool start)
        {
            __startCaptureListener(start);
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        public override void openUrlWebView(string url)
        {
            Application.OpenURL(url);
        }

        public override void OpenLocation(int reqID)
        {
            __openLocation(reqID);
        }

    }
}
//#endif
