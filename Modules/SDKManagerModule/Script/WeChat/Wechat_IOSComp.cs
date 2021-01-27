//#if UNITY_IPHONE && !UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace lgu3d {
    //微信接口组件 IOS
    public class Wechat_IOSComp : WeChatComp
    {
        [DllImport("__Internal")]
        private static extern bool __initsdk_wx(string wxAppId, string wxAppSecret);
        [DllImport("__Internal")]
        private static extern bool __isappinstalled_wx();
        [DllImport("__Internal")]
        private static extern bool __openapp_wx();
        [DllImport("__Internal")]
        private static extern void __authorize_wx();
        [DllImport("__Internal")]
        private static extern void __pay_wx(string orderJson);
        [DllImport("__Internal")]
        private static extern void __sharecontent_wx(int platformtype,string contentStr);

        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="wxAppId">微信Id</param>
        /// <param name="wxAppSecret">微信密钥</param>
        public override bool InitSdk(params object[] agrs)
        {
            string wxAppId,wxAppSecret;
            if(agrs.Length == 2){
                wxAppId = (string)agrs[0];
                wxAppSecret = (string)agrs[1];
                return __initsdk_wx(wxAppId, wxAppSecret);
            }else{
                Debug.LogError("初始化微信sdk 错误:参数异常");
                return false;
            }
        }

        /// <summary>
        /// 微信是否安装
        /// </summary>
        /// <returns></returns>
        public override bool IsWxAppInstalled()
        {
            return __isappinstalled_wx();
        }

        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reqID"></param>
        public override void Authorize()
        {
            __authorize_wx();
        }

        //打开微信APP
        public override bool OpenWXApp(){
            return __openapp_wx();
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="orderJson">订单</param>
        public override void Pay(string orderJson)
        {
            __pay_wx(orderJson);
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="content">内容</param>
        public override void ShareContent(WeChatPlatformType platformtype, ShareContent content)
        {
            __sharecontent_wx((int)platformtype, content.GetShareParamsStr());
        }
    }
}
//#endif
