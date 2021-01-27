using UnityEngine;
using UnityEditor;

namespace lgu3d {

    public abstract class XianLiaoComp : ModelCompBase<SDKManagerModule>
    {
        public abstract bool InitSdk(params object[] agrs);
        public abstract bool IsAppInstalled();
        public abstract bool openXlApp();
        public abstract void Authorize();
        public abstract void Pay(string orderJson);
        public abstract void ShareContent(ShareContent content);
    }


    public static class XianLiaoMsgId {
        /// <summary>
        /// 微信SDK 初始化回应
        /// </summary>
        public const string XianLiaoInitRespond = "XianLiaoInitRespond";
        /// <summary>
        /// 微信SDK 授权回应
        /// </summary>
        public const string XianLiaoAuthorizeRespond = "XianLiaoAuthorizeRespond";
        /// <summary>
        /// 微信SDK 支付回应
        /// </summary>
        public const string XianLiaoPayRespond = "XianLiaoPayRespond";
        /// <summary>
        /// 微信SDK 分享回应
        /// </summary>
        public const string XianLiaoShareRespond = "XianLiaoShareRespond";
    }
}
