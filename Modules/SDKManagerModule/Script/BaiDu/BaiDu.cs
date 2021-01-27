using UnityEngine;
using UnityEditor;

namespace lgu3d {

    public abstract class BaiDuComp : ModelCompBase<SDKManagerModule>
    {
        /// <summary>
        /// 初始化sdk
        /// </summary>
        /// <param name="agrs"></param>
        /// <returns></returns>
        public abstract bool InitSdk(params object[] agrs);
        /// <summary>
        /// 启动定位
        /// </summary>
        /// <returns></returns>
        public abstract void StartLocation(); 

        /// <summary>
        /// 停止定位
        /// </summary>
        /// <returns></returns>
        public abstract void StopLocation();
    }


    public static class BaiDuMsgId {
        /*
        百度定位回调 
        mtype = 1 定位授权回调 
            参数 : ecode  
            说明
            BMKLocationAuthErrorUnknown = -1,                    ///< 未知错误
            BMKLocationAuthErrorSuccess = 0,           ///< 鉴权成功
            BMKLocationAuthErrorNetworkFailed = 1,          ///< 因网络鉴权失败
            BMKLocationAuthErrorFailed  = 2,               ///< KEY非法鉴权失败
        mtype = 2 当定位发生错误时
            参数 : ecode,emsg 
            说明
            状态码    返回值    说明    排查方案
            0    BMKLocationErrorUnknown 未知错误    不明原因错误，请在线反馈给我们处理
            1    BMKLocationErrorLocFailed 定位错误    位置未知，持续定位中
            2    BMKLocationErrorDenied 定位错误 手机不允许定位，请确认用户授予定位权限或者手机是否打开定位开关
            3    BMKLocationErrorNetWork 超时    网络环境不稳定，稍后重试
            4    BMKLocationErrorHeadingFailed 获取方向失败    iOS系统返回方向错误，需要依赖苹果解决，或您稍后重试
            5    BMKLocationErrorGetExtraNetworkFailed 连接异常    网络原因导致获取额外信息（地址、网络状态等信息）失败
            6    BMKLocationErrorGetExtraParseFailed 连接异常    网络返回数据解析失败导致获取额外信息（地址、网络状态等信息）失败
            7    BMKLocationErrorFailureAuth 鉴权失败    鉴权失败导致无法返回定位、地址等信息 
        mtype = 3 连续定位回调
            参数 : 
            city: 城市地址
            latitudeStr：纬度
            longitude：经度
        mtype = 4 定位权限状态改变时回调
            参数 : state
            说明
            * // User has not yet made a choice with regards to this application
            kCLAuthorizationStatusNotDetermined = 0,
            // This application is not authorized to use location services.  Due
            // to active restrictions on location services, the user cannot change
            // this status, and may not have personally denied authorization
            kCLAuthorizationStatusRestricted,
            // User has explicitly denied authorization for this application, or
            // location services are disabled in Settings.
            kCLAuthorizationStatusDenied,
            // User has granted authorization to use their location at any
            // time.  Your app may be launched into the background by
            // monitoring APIs such as visit monitoring, region monitoring,
            // and significant location change monitoring.
            //
            // This value should be used on iOS, tvOS and watchOS.  It is available on
            // MacOS, but kCLAuthorizationStatusAuthorized is synonymous and preferred.
            kCLAuthorizationStatusAuthorizedAlways
            // User has granted authorization to use their location only while
            // they are using your app.  Note: You can reflect the user's
            // continued engagement with your app using
            // -allowsBackgroundLocationUpdates.
            //
            // This value is not available on MacOS.  It should be used on iOS, tvOS and
            // watchOS.
            kCLAuthorizationStatusAuthorizedWhenInUse
            // User has authorized this application to use location services.
            //
            // This value is deprecated or prohibited on iOS, tvOS and watchOS.
            // It should be used on MacOS.
            kCLAuthorizationStatusAuthorized
        */
        public const string BaiDuLocationRespond = "BaiDuLocationRespond";
    }
}

