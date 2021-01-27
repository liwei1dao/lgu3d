using UnityEngine;
using UnityEditor;

namespace lgu3d {
    /// <summary>
    /// 手机系统接口组件
    /// </summary>
    public abstract class PhoneComp : ModelCompBase<SDKManagerModule>
    {
        public abstract bool InitSdk();
        public abstract void Callphone(string phonenum);    //打电话
        public abstract float GetBatteryLevle();
        public abstract string GetPastBoard();
        public abstract bool CopyText(string textStr);
        public abstract void QuitApp();
        public abstract void ShakeApp();
        public abstract void StartCaptureListener(bool start);
        public abstract void openUrlWebView(string url);
        public abstract void OpenLocation(int reqID);
    }

   public static class PhoneMsgId {
        public const string IOSlocalInitRespond = "IOSlocalInitRespond";
    }
}