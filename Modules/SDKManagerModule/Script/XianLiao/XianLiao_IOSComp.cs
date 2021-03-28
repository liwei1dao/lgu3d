//#if UNITY_IPHONE && !UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace lgu3d {
    #if UNITY_IOS
    //微信接口组件 IOS
    public class XianLiao_IOSComp : XianLiaoComp
    {
        [DllImport("__Internal")]
        private static extern bool __initsdk_xl(string xlAppId, string xlAppSecret);
        [DllImport("__Internal")]
        private static extern bool __isappinstalled_xl();
        [DllImport("__Internal")]
        private static extern bool __openApp_xl();
        [DllImport("__Internal")]
        private static extern void __authorize_xl();
        [DllImport("__Internal")]
        private static extern void __pay_xl(string orderJson);
        [DllImport("__Internal")]
        private static extern void __sharecontent_xl(string contentStr);

        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="xlAppId">闲聊Id</param>
        /// <param name="xlAppSecret">闲聊密钥</param>
        public override bool InitSdk(params object[] agrs)
        {
            string xlAppId,xlAppSecret;
            if(agrs.Length == 2){
                xlAppId = (string)agrs[0];
                xlAppSecret = (string)agrs[1];
                return __initsdk_xl(xlAppId, xlAppSecret);
            }else{
                Debug.LogError("初始化微信sdk 错误:参数异常");
                return false;
            }
        }

        /// <summary>
        /// 闲聊是否安装
        /// </summary>
        /// <returns></returns>
        public override bool IsAppInstalled()
        {
            return __isappinstalled_xl();
        }

        /// <summary>
        /// 闲聊授权
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reqID"></param>
        public override void Authorize()
        {
            __authorize_xl();
        }

        /// <summary>
        /// 闲聊支付
        /// </summary>
        /// <param name="orderJson">订单</param>
        public override void Pay(string orderJson)
        {
            __pay_xl(orderJson);
        }

        public override bool openXlApp()
        {
            return __openApp_xl();
        }

        /// <summary>
        /// 闲聊分享
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="content">内容</param>
        public override void ShareContent(ShareContent content)
        {
            __sharecontent_xl(content.GetShareParamsStr());
        }
    }
    #endif
}
//#endif
