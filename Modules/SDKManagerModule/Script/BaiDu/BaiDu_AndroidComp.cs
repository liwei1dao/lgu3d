//#if UNITY_ANDROID && !UNITY_EDITOR
using UnityEngine;

namespace lgu3d {
    //微信接口组件 Android
    public class BaiDu_AndroidComp : BaiDuComp
    {
        private AndroidJavaClass bdclass;

        public override void Load(ModelBase _ModelContorl, params object[] _Agr)
        {
            base.Load(_ModelContorl, _Agr);
            bdclass = new AndroidJavaClass("com.liwei1dao.baidu");
        }

        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="wxAppId">微信Id</param>
        /// <param name="wxAppSecret">微信密钥</param>
        public override bool InitSdk(params object[] agrs)
        {
            string bdAK;
            if(agrs.Length == 2){
                bdAK = (string)agrs[0];
                return bdclass.CallStatic<bool>("InitSdk", bdAK);
            }else{
                Debug.LogError("初始化微信sdk 错误:参数异常");
                return false;
            }
           
        }

        /// <summary>
        /// 开始定位
        /// </summary>
        /// <returns></returns>
        public override void StartLocation()
        {
            bdclass.CallStatic("StartLocation");
        }

        /// <summary>
        /// 结束定位
        /// </summary>
        /// <returns></returns>
        public override void StopLocation()
        {
            bdclass.CallStatic("StopLocation");
        }
    }
}
//#endif