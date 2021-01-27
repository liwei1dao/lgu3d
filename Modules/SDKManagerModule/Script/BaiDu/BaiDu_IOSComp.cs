//#if UNITY_IPHONE && !UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices;

namespace lgu3d {
    //微信接口组件 IOS
    public class BaiDu_IOSComp : BaiDuComp
    {
        [DllImport("__Internal")]
        private static extern bool __initsdk_bd(string bdAK);
        [DllImport("__Internal")]
        private static extern void __startlocation_bd();
        [DllImport("__Internal")]
        private static extern void __stoplocation_bd();

        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="wxAppId">微信Id</param>
        /// <param name="wxAppSecret">微信密钥</param>
        public override bool InitSdk(params object[] agrs)
        {
            string bdAK;
            if(agrs.Length == 1){
                bdAK = (string)agrs[0];
                return __initsdk_bd(bdAK);
            }else{
                Debug.LogError("初始化微信sdk 错误:参数异常");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="wxAppId">微信Id</param>
        /// <param name="wxAppSecret">微信密钥</param>
        public override void StartLocation()
        {
            __startlocation_bd();
        }

        /// <summary>
        /// 微信是否安装
        /// </summary>
        /// <returns></returns>
        public override void StopLocation()
        {
            __stoplocation_bd();
        }
    }
}
//#endif
