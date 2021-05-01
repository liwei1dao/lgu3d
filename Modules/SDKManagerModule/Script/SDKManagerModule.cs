using LuaInterface;
using UnityEngine;

namespace lgu3d {

    /// <summary>
    /// SDK消息代理
    /// </summary>
    /// <param name="data"></param>
    public delegate void SDKMessageAgent(string data);

    /// <summary>
    /// 模板sdk 管理 集成微信，闲聊，百度
    /// </summary>
    public class SDKManagerModule : LuaModelControlBase<SDKManagerModule>
    {
        private LuaFunction LuaMessageReceive;
        private SDKMessageReceiveComp messageComp;
        private WeChatComp wechatComp;
        private BaiDuComp bdComp;
        private PhoneComp phoneComp;

        public override void Load<Model>(ModelLoadBackCall<Model> _LoadBackCall, params object[] _Agr)
        {
            messageComp = AddComp<SDKMessageReceiveComp>();
            LuaMessageReceive = LuaManagerModule.Instance.GetFunction(ModuleName + ".MessageReceive");
            base.Load(_LoadBackCall, _Agr);
        }

        public void MessageReceive(string msgId, string data) {
            this.messageComp.MessageReceive(msgId, data);
            this.LuaMessageReceive?.Call(msgId,data);
        }

        /// <summary>
        /// 注册消息的处理函数
        /// </summary>
        /// <param name="msgId"></param>
        /// <param name="agent"></param>
        public void RegisterMessageDeal(string msgId, SDKMessageAgent agent)
        {
            this.messageComp.RegisterMessageDeal(msgId, agent);
        }

        #region 微信
        public WeChatComp GetWeChatComp() {
            return wechatComp;
        }
        public WeChatComp InitWeChat(string wxAppId, string wxAppSecret)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            wechatComp = AddComp<Wechat_AndroidComp>();
#elif UNITY_IPHONE && !UNITY_EDITOR
            wechatComp = AddComp<Wechat_IOSComp>();
#else
            Debug.LogError("初始化WeChat SDK 失败 平台异常");
#endif
            if (wechatComp != null) {
                wechatComp.InitSdk(wxAppId, wxAppSecret);
            }
            return wechatComp;
        }
        #endregion

        #region 百度
        public BaiDuComp GetBaiDuComp() {
            return bdComp;
        }
        public BaiDuComp InitBaiDu()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            bdComp = AddComp<BaiDu_AndroidComp>();
#elif UNITY_IPHONE && !UNITY_EDITOR
            bdComp = AddComp<BaiDu_IOSComp>();
#else
            Debug.LogError("初始化BaiDu SDK 失败 平台异常");
#endif
            if (bdComp != null) {
                bdComp.InitSdk();
            }
            return bdComp;
        }
        #endregion

        #region 公共接口
        public PhoneComp GetPhoneComp()
        {
            return phoneComp;
        }
        public PhoneComp InitPhoneSys()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            phoneComp = AddComp<Phone_AndroidComp>();
#elif UNITY_IOS && !UNITY_EDITOR
            phoneComp = AddComp<Phone_IOSComp>();
#else
            Debug.LogError("初始化公共SDK 失败 平台异常");
#endif
            if (phoneComp != null)
            {
                phoneComp.InitSdk();
            }
            return phoneComp;
        }
        #endregion
    }
}

