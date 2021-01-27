using UnityEngine;
using UnityEditor;

namespace lgu3d {

    public abstract class WeChatComp : ModelCompBase<SDKManagerModule>
    {
        public abstract bool InitSdk(params object[] agrs);
        public abstract bool IsWxAppInstalled();
        public abstract bool OpenWXApp();
        public abstract void Authorize();
        public abstract void Pay(string orderJson);
        public abstract void ShareContent(WeChatPlatformType type, ShareContent content);
    }


    public static class WeChatMsgId {
        /// <summary>
        /// 微信SDK 初始化回应
        /// </summary>
        public const string WechatInitRespond = "WechatInitRespond";
        /// <summary>
        /// 微信SDK 授权回应
        /// </summary>
        public const string WechatAuthorizeRespond = "WechatAuthorizeRespond";
        /// <summary>
        /// 微信SDK 支付回应
        /// </summary>
        public const string WechatPayRespond = "WechatPayRespond";
        /// <summary>
        /// 微信SDK 分享回应
        /// </summary>
        public const string WechatShareRespond = "WechatShareRespond";
    }

    /// <summary>
    /// 微信分享平台
    /// </summary>
    public enum WeChatPlatformType {
        WeChat = 0,    //好友
        WeChatMoments = 1,    //朋友圈
    }
}

