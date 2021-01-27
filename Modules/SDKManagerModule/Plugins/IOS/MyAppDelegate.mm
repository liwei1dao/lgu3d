#import "MyAppDelegate.h"

IMPL_APP_CONTROLLER_SUBCLASS(MyAppDelegate)

@implementation MyAppDelegate
- (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
{
    NSLog(@"走进来了么，公共脚本");
    [super application:application didFinishLaunchingWithOptions:launchOptions];
    return YES;
}

-(void) applicationWillTerminate:(UIApplication *)application{
    [super applicationWillTerminate:application];
}

- (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
{
    //闲聊
    if([XianliaoApiManager handleOpenURL:url]){
        return YES;
    }
    //微信
    return [WXApi handleOpenURL:url delegate:[MyWeChat Instance]];
}

- (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(nonnull NSDictionary *)options
{
    //闲聊
    if([XianliaoApiManager handleOpenURL:url]){
        return YES;
    }
    //微信
    NSString *str = [url absoluteString];
    if([str hasPrefix:[MyWeChat Instance].wxId]){
        return [WXApi handleOpenURL:url delegate:[MyWeChat Instance]];
    }
    return [super application:app openURL:url options:options];
}

- (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
{
    [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
    //闲聊
    if([XianliaoApiManager handleOpenURL:url]){
        return YES;
    }
    //微信
    NSString *str = [url absoluteString];
    if([str hasPrefix:[MyWeChat Instance].wxId]){
         return [WXApi handleOpenURL:url delegate:[MyWeChat Instance]];
    }
    return [super application:application openURL:url sourceApplication:sourceApplication annotation:annotation];
}

@end
