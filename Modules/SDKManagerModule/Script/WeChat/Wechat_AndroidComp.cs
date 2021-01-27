//#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace lgu3d {
    //微信接口组件 Android
    public class Wechat_AndroidComp : WeChatComp
    {
        private AndroidJavaClass wxclass;

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            wxclass = new AndroidJavaClass("com.liwei1dao.wechat");
        }

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
                return wxclass.CallStatic<bool>("InitSdk", wxAppId, wxAppSecret);
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
             return wxclass.CallStatic<bool>("IsWxAppInstalled");
        }

        //打开微信APP
        public override bool OpenWXApp(){
            return wxclass.CallStatic<bool>("OpenWXApp");
            // return __openapp_wx();
        }

        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reqID"></param>
        public override void Authorize()
        {
            wxclass.CallStatic("WxAuthorize");
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="orderJson">订单</param>
        public override void Pay(string orderJson)
        {
            wxclass.CallStatic("Pay", orderJson);
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="content">内容</param>
        public override void ShareContent(WeChatPlatformType type, ShareContent content)
        {
            wxclass.CallStatic("ShareContent",(int)type, content.GetShareParamsStr());
        }
    }
}
//#endif