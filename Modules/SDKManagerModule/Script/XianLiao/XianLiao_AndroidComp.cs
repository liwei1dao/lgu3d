//#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace lgu3d {
    //微信接口组件 Android
    public class XianLiao_AndroidComp : XianLiaoComp
    {
        private AndroidJavaClass xlclass;

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            xlclass = new AndroidJavaClass("com.liwei1dao.xianliao");
        }

        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="wxAppId">微信Id</param>
        /// <param name="wxAppSecret">微信密钥</param>
        public override bool InitSdk(params object[] agrs)
        {
            string xlAppId,xlAppSecret;
            if(agrs.Length == 2){
                xlAppId = (string)agrs[0];
                xlAppSecret = (string)agrs[1];
                return xlclass.CallStatic<bool>("InitSdk", xlAppId, xlAppSecret);
            }else{
                Debug.LogError("初始化微信sdk 错误:参数异常");
                return false;
            }
           
        }

        /// <summary>
        /// 微信是否安装
        /// </summary>
        /// <returns></returns>
        public override bool IsAppInstalled()
        {
             return xlclass.CallStatic<bool>("IsAppInstalled");
        }

        /// <summary>
        /// 微信授权
        /// </summary>
        /// <param name="type"></param>
        /// <param name="reqID"></param>
        public override void Authorize()
        {
            xlclass.CallStatic("Authorize");
        }

        public override bool openXlApp()
        {
            return xlclass.CallStatic<bool>("openXlApp");
            //return __openApp_xl();
        }

        /// <summary>
        /// 微信支付
        /// </summary>
        /// <param name="orderJson">订单</param>
        public override void Pay(string orderJson)
        {
            xlclass.CallStatic("Pay", orderJson);
        }

        /// <summary>
        /// 分享
        /// </summary>
        /// <param name="type">平台</param>
        /// <param name="content">内容</param>
        public override void ShareContent(ShareContent content)
        {
            xlclass.CallStatic("ShareContent", content.GetShareParamsStr());
        }
    }
}
//#endif