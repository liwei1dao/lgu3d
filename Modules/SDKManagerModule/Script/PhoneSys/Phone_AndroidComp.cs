//#if UNITY_IPHONE && !UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;
using System;

namespace lgu3d {
    //微信接口组件 IOS
    public class Phone_AndroidComp : PhoneComp
    {
        /// <summary>
        /// 初始化sdk
        /// </summary>
        public override bool InitSdk()
        {
            return false;
        }

        /// <summary>
        /// 打电话
        /// </summary>
        /// <returns></returns>
        public override void Callphone(string phonenum)
        {
            
        }

        /// <summary>
        /// 获取电量
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reqID"></param>
        public override float GetBatteryLevle()
        {
            return 0;
        }

        /// <summary>
        /// 获取剪切板
        /// </summary>
        /// <param name="orderJson">订单</param>
        public override string GetPastBoard()
        {
            return "";
        }

        /// <summary>
        /// 复制文本
        /// </summary>
        /// <param name="textStr"></param>
        /// <returns></returns>
        public override bool CopyText(string textStr)
        {
            return false;
        }

        /// <summary>
        ///退出APP
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="content">内容</param>
        public override void QuitApp()
        {

        }

        /// <summary>
        /// 震动APP
        /// </summary>
        public override void ShakeApp()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public override void StartCaptureListener(bool start)
        {
           
        }

        /// <summary>
        /// 打开链接
        /// </summary>
        public override void openUrlWebView(string url)
        {

        }

        public override void OpenLocation(int reqID)
        {

        }

    }
}
//#endif
