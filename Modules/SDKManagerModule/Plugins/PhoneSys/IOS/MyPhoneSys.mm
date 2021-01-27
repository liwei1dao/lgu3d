#import "MyPhoneSys.h"

@implementation MyPhoneSys

+(instancetype)Instance {
    static dispatch_once_t onceToken;
    static MyPhoneSys *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MyPhoneSys alloc] init];
    });
    return instance;
}

extern "C"
{
    //初始化SDK
    bool __initsdk_phone()
    {
        return true;
    }
    
    //拨打电话
    void __callPhone(char *phonenum){
        NSString *phoneStr =[NSString stringWithUTF8String:phonenum];
        NSString *formatPhoneStr = [NSString stringWithFormat:@"tel://%@",phoneStr];
        [[UIApplication sharedApplication]openURL:[NSURL URLWithString:formatPhoneStr]];
    }
    
    //获取电量
    float __getBatteryLevel(){
        [UIDevice currentDevice].batteryMonitoringEnabled = true;
        float t =[[UIDevice currentDevice] batteryLevel];
        [UIDevice currentDevice].batteryMonitoringEnabled = false;

        return t;
    }
    
    //获取剪切板
    char* __getPastBoard(){
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        NSString *content = pasteboard.string ;
        if(content == NULL || content ==nil){
            char * m = new char[1];
            return m;
        }
        char * x = (char *)malloc(strlen([content UTF8String]) + 1);
        strcpy(x, [content UTF8String]);
        return x;
    }
    
    bool __copyText(char *textStr){
        UIPasteboard *pasteboard = [UIPasteboard generalPasteboard];
        pasteboard.string = [NSString stringWithUTF8String:textStr];
        return true;
    }
    
    //退出APP
    void __quitApp(){
            UIWindow *window = [[UIApplication sharedApplication].windows objectAtIndex:0];

        [UIView animateWithDuration:0.5 animations:^{
            [UIView setAnimationTransition:UIViewAnimationTransitionCurlDown forView:window cache:NO];
            window.bounds = CGRectZero;
        } completion:^(BOOL finished) {
            exit(0);
        }];
    }
    
    //震动
    void __shakeApp(){
        AudioServicesPlaySystemSound(kSystemSoundID_Vibrate);
    }
    
    //
    void __startCaptureListener(bool start){
        
    }

    void __openLocation(int reqID)
    {
        if(![CLLocationManager locationServicesEnabled])
        {
            UIAlertView *alert = [[UIAlertView alloc] initWithTitle:@"打开定位开关" message:@"定位服务未开启，请进入系统［设置］> [隐私] > [定位服务]中打开开关，并允许使用定位服务" delegate:nil cancelButtonTitle:@"知道了" otherButtonTitles:nil];
            [alert show];
            return;
        }
        CLAuthorizationStatus type  = [CLLocationManager authorizationStatus];
        if(type == kCLAuthorizationStatusDenied)
        {
            [[UIApplication sharedApplication] openURL:[NSURL URLWithString:UIApplicationOpenSettingsURLString]];
            return;
        }
        [[BdLocationManager sharedManager] startLocation:reqID];
    }
}


@end
