#import "MyBaidu.h"

@implementation MyBaidu
+(instancetype)Instance {
    static dispatch_once_t onceToken;
    static MyBaidu *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MyBaidu alloc] init];
    });
    return instance;
}
// - (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
// {
//     instance = self;
//     [super application:application didFinishLaunchingWithOptions:launchOptions];
//     return YES;
// }

extern "C"
{

    bool __initsdk_bd(char *bdAK){
        NSString *bdak = [NSString stringWithUTF8String:bdAK];
        NSLog(@"初始化百度sdk AK:%@",bdak);
        return [[MyBaidu Instance] initsdk:bdak];
    }

    void __startlocation_bd(){
        NSLog(@"开始定位");
        [[MyBaidu Instance] startLocation];
    }

    void __stoplocation_bd(){
        NSLog(@"结束定位");
        [[MyBaidu Instance] stopLocation];
    }
}
//授权回调
+(void)BaiDuLocationRespond:(NSDictionary *)data{
    NSString *jsonStr = [MyBaidu transformationToString:data];
    NSLog(@"BaiDuLocationRespond %@",jsonStr);
    NSString *str = [NSString stringWithFormat:@"BaiDuLocationRespond|%@",jsonStr];
    UnitySendMessage("SdkObject", "ReceiveMessage",  [str UTF8String]);
}
//-------------------------------------------------------------------------------------------------------------------
//初始化sdk
-(bool)initsdk:(NSString *)bdak
{
    [[BMKLocationAuth sharedInstance] checkPermisionWithKey:bdak authDelegate:self];
    if(!locationManager){
        locationManager = [[BMKLocationManager alloc] init];
        locationManager.delegate = self;
        //设置位置获取超时时间
        locationManager.locationTimeout = 10;
        //设置获取地址信息超时时间
        locationManager.reGeocodeTimeout = 10;
    }
    return true;
}

//普通态
-(void)startLocation
{
    [locationManager setLocatingWithReGeocode:YES];
    [locationManager startUpdatingLocation];
}

//停止定位
-(void)stopLocation
{
    [locationManager stopUpdatingLocation];
}

/**
 *在地图View将要启动定位时，会调用此函数
 *@param mapView 地图View
 */
- (void)willStartLocatingUser
{
    NSLog(@"start locate");
}

/**
 *@brief 返回授权验证错误
 *@param iError 错误号 : 为0时验证通过，具体参加BMKLocationAuthErrorCode
 *typedef NS_ENUM(NSInteger, BMKLocationAuthErrorCode) {
     BMKLocationAuthErrorUnknown = -1,                    ///< 未知错误
     BMKLocationAuthErrorSuccess = 0,           ///< 鉴权成功
     BMKLocationAuthErrorNetworkFailed = 1,          ///< 因网络鉴权失败
     BMKLocationAuthErrorFailed  = 2,               ///< KEY非法鉴权失败
 };
 */
- (void)onCheckPermissionState:(BMKLocationAuthErrorCode)iError{
    NSNumber* mtype=[NSNumber numberWithInt :1];
    NSNumber* ecode = [NSNumber numberWithInt :iError];
    NSDictionary *posDict=[NSDictionary dictionaryWithObjectsAndKeys:mtype,@"mtype",ecode,@"ecode",nil];
    [MyBaidu BaiDuLocationRespond:posDict];
}

/**
*  @brief 当定位发生错误时，会调用代理的此方法。
*  @param manager 定位 BMKLocationManager 类。
*  @param error 返回的错误，参考 CLError 。
 在回调的NSError *error里查看具体的错误信息。各错误返回码的具体说明如下：

 状态码    返回值    说明    排查方案
 0    BMKLocationErrorUnknown 未知错误    不明原因错误，请在线反馈给我们处理
 1    BMKLocationErrorLocFailed 定位错误    位置未知，持续定位中
 2    BMKLocationErrorDenied 定位错误 手机不允许定位，请确认用户授予定位权限或者手机是否打开定位开关
 3    BMKLocationErrorNetWork 超时    网络环境不稳定，稍后重试
 4      BMKLocationErrorHeadingFailed 获取方向失败    iOS系统返回方向错误，需要依赖苹果解决，或您稍后重试
 5    BMKLocationErrorGetExtraNetworkFailed 连接异常    网络原因导致获取额外信息（地址、网络状态等信息）失败
 6    BMKLocationErrorGetExtraParseFailed 连接异常    网络返回数据解析失败导致获取额外信息（地址、网络状态等信息）失败
 7     BMKLocationErrorFailureAuth 鉴权失败    鉴权失败导致无法返回定位、地址等信息
*/
- (void)BMKLocationManager:(BMKLocationManager * _Nonnull)manager didFailWithError:(NSError * _Nullable)error{
    NSNumber* mtype=[NSNumber numberWithInt :2];
    NSNumber* ecode = [NSNumber numberWithInt :error.code];
    NSString* emsg = error.description;
    NSDictionary *posDict=[NSDictionary dictionaryWithObjectsAndKeys:mtype,@"mtype",ecode,@"ecode",emsg,@"emsg",nil];
    [MyBaidu BaiDuLocationRespond:posDict];
}
/**
*  @brief 连续定位回调函数。
*  @param manager 定位 BMKLocationManager 类。
*  @param location 定位结果，参考BMKLocation。
*  @param error 错误信息。
*/
- (void)BMKLocationManager:(BMKLocationManager * _Nonnull)manager didUpdateLocation:(BMKLocation * _Nullable)location orError:(NSError * _Nullable)error
{
    if (error)
    {
        NSLog(@"locError:{%ld - %@};", (long)error.code, error.localizedDescription);
    } if (location) {//得到定位信息，添加annotation
        if (location.location && location.rgcData && location.rgcData.poiList) {
            NSLog(@"LOC = %@",location.location);
            NSNumber *mtype=[NSNumber numberWithInt :3];
            BMKLocationPoi *pl = [location.rgcData.poiList firstObject];
            NSString *city = pl.addr;
            float latitude =location.location.coordinate.latitude;
            float longitude =location.location.coordinate.longitude;
            NSString *latitudeStr=[NSString stringWithFormat:@"%f",latitude];
            NSString *longitudeStr=[NSString stringWithFormat:@"%f",longitude];
            NSDictionary *posDict=[NSDictionary dictionaryWithObjectsAndKeys:mtype,@"mtype",latitudeStr,@"latitude",longitudeStr,@"longitude",city,@"city",nil];
            [MyBaidu BaiDuLocationRespond:posDict];
        }
    }
}

/**
 *  @brief 定位权限状态改变时回调函数
 *  @param manager 定位 BMKLocationManager 类。
 *  @param status 定位权限状态。
 *   // User has not yet made a choice with regards to this application
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
 };
 */
- (void)BMKLocationManager:(BMKLocationManager * _Nonnull)manager didChangeAuthorizationStatus:(CLAuthorizationStatus)status{
    NSNumber* mtype=[NSNumber numberWithInt :4];
    NSNumber* state = [NSNumber numberWithInt :status];
    NSDictionary *posDict=[NSDictionary dictionaryWithObjectsAndKeys:mtype,@"mtype",state,@"state",nil];
    [MyBaidu BaiDuLocationRespond:posDict];
}

/**
 *在地图View停止定位后，会调用此函数
 *@param mapView 地图View
 */
- (void)didStopLocatingUser
{
    NSLog(@"xcode: stop locate");
}


///**字典或者数组转化成json串*/
+ (NSString *)transformationToString:(id )transition{
    NSString *jsonString = nil;
    NSError *error;
    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:transition
                                                       options:NSJSONWritingPrettyPrinted // Pass 0 if you don't care about the readability of the generated string
                                                         error:&error];
    if (!jsonData) {
        NSLog(@"Got an error: %@", error);
        return @"转化失败";
    } else {
        jsonString = [[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
        return jsonString;
    }
}
@end
