
#import "MyXianLiao.h"
@implementation MyXianLiao

+(instancetype)Instance {
    static dispatch_once_t onceToken;
    static MyXianLiao *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MyXianLiao alloc] init];
    });
    return instance;
}
// - (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
// {
//     instance = self;
//     [super application:application didFinishLaunchingWithOptions:launchOptions];
//     return YES;
// }

//  - (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options: (NSDictionary<UIApplicationOpenURLOptionsKey,id> *)options {
//     if ([XianliaoApiManager handleOpenURL:url]) {
//         return YES;
//     }
//     return YES;
//  }
// - (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
// {
//      if ([XianliaoApiManager handleOpenURL:url]) {
//          return YES;
//      }
// }

// - (BOOL)application:(UIApplication *)application openURL:(NSURL *)url sourceApplication:(NSString *)sourceApplication annotation:(id)annotation {
//     if ([XianliaoApiManager handleOpenURL:url]) {
//         return YES;
// }
//     return true;
// }

extern "C"
{
//#pragma mark - c
    //初始化sdk
    bool __initsdk_xl(char *xlAppId, char *xlAppSecret)
    {
        NSString *xlid = [NSString stringWithUTF8String:xlAppId];
        NSString *xlsecret = [NSString stringWithUTF8String:xlAppSecret];
        NSLog(@"初始化微信闲聊sdk xlid:%@ xlsecret:%@",xlid,xlsecret);
        return [[MyXianLiao Instance] initsdk:xlid :xlsecret];
    }
    
    //检测是否安装客户端
    bool __isappinstalled_xl()
    {
        return [[MyXianLiao Instance] isxlappinstalled];
    }
    //获取授权
    void __authorize_xl()
    {
        NSLog(@"启动闲聊授权");
        [[MyXianLiao Instance] authorize];
    }
    
    bool __openApp_xl()
    {
        NSLog(@"打开闲聊");
        return [[MyXianLiao Instance] openxlapp];
    }
    
    void __pay_xl(char *orderJson)
    {
        NSLog(@"启动闲聊购买");
    }
    
    void __sharecontent_xl(char *contentStr)
    {
        NSLog(@"启动闲聊分享,contentStr = %s",contentStr);

       NSString *contentString = [NSString stringWithUTF8String:contentStr];
       NSData *contentData = [contentString dataUsingEncoding:NSUTF8StringEncoding];
       NSError *error;

       NSMutableDictionary *dict  = [NSJSONSerialization JSONObjectWithData:contentData options:NSJSONReadingMutableLeaves error:&error];

       if(dict !=nil)
       {
           NSString *shareType = [dict objectForKey:@"shareType"];
           if(shareType.intValue ==1)//文本
           { // text
               NSString *text = [dict objectForKey:@"text"];
               if ([XianliaoApiManager isInstallXianliao])
               {
                   XianliaoShareTextObject *textObject = [[XianliaoShareTextObject alloc] init];
                   textObject.text = text;
                   [XianliaoApiManager share:textObject fininshBlock:^(XianliaoShareCallBackType callBackType)
                   {
                       NSLog(@"textShareCode:%ld", (long)callBackType);
                        NSDictionary* returedata =  @{@"code":[NSNumber numberWithInt: callBackType]};
                        [MyXianLiao XianLiaoShareRespond: returedata];
                   }];
               }
           }
           else if(shareType.intValue ==2)//图片
           {
               NSString *imagePath = [dict objectForKey:@"imageUrl"];
               if(imagePath == nil || imagePath == NULL || [imagePath isEqualToString:@""])
               {
                   imagePath = [dict objectForKey:@"imagePath"];
               }
               UIImage * thumbImage = [UIImage imageWithContentsOfFile:imagePath];

               NSData * imageData = UIImageJPEGRepresentation(thumbImage,0.5f);
               if ([XianliaoApiManager isInstallXianliao]) {
                   XianliaoShareImageObject *imageObject = [[XianliaoShareImageObject alloc] init];
                   imageObject.imageData = imageData;
                   [XianliaoApiManager share:imageObject fininshBlock:^(XianliaoShareCallBackType callBackType) {
                       NSLog(@"imageShareCode:%ld", (long)callBackType);
                         NSDictionary* returedata =  @{@"code":[NSNumber numberWithInt: callBackType]};
                         [MyXianLiao XianLiaoShareRespond: returedata];
                   }];
               }
           }
           else if(shareType.intValue ==4)
           {//链接分享
               NSString *title = [dict objectForKey:@"title"];
               NSString *description = [dict objectForKey:@"text"];
               NSString *url = [dict objectForKey:@"url"];
               UIImage *thumbImage = [UIImage imageNamed:@"AppIcon57x57"];
              if([XianliaoApiManager isInstallXianliao]) {
                  NSString * timeStr = [NSString stringWithFormat:@"%.f",(double)[[NSDate date] timeIntervalSince1970]];
                  NSString *roomToken = [NSString stringWithFormat:@"%@%@",@"xianliao",timeStr];
                  NSData * imageData = UIImageJPEGRepresentation(thumbImage,1);
                  XianliaoShareAppObject *game = [[XianliaoShareAppObject alloc] init];
                  game.roomToken = roomToken;
                  game.roomId = @"";
                  game.title = title;
                  game.text = description;
                  game.imageData = imageData;
                  if ([dict objectForKey:@"roomId"]){
                      NSString *roomId =[dict objectForKey:@"roomId"];
                      game.roomId =roomId;
                      game.iOSDownloadUrl =url;
                      game.androidDownloadUrl =url;
                  }
                  [XianliaoApiManager share:game fininshBlock:^(XianliaoShareCallBackType callBackType) {
                      NSLog(@"appShareCode:%ld", (long)callBackType);
                      NSDictionary* returedata =  @{@"code":[NSNumber numberWithInt: callBackType]};
                      [MyXianLiao XianLiaoShareRespond: returedata];
                  }];
              }
           }
           else if (shareType.intValue == 10)
           {
                NSString *title = [dict objectForKey:@"title"];
                NSString *description = [dict objectForKey:@"text"];
                NSString *imageUrl = [dict objectForKey:@"imageUrl"];
                NSString *url = [dict objectForKey:@"url"];
                UIImage *thumbImage = [UIImage imageNamed:@"AppIcon57x57"];
                if([XianliaoApiManager isInstallXianliao])
                {
                    NSString * timeStr = [NSString stringWithFormat:@"%.f",(double)[[NSDate date] timeIntervalSince1970]];
                    NSString *roomToken = [NSString stringWithFormat:@"%@%@",@"xianliao",timeStr];
                    NSData * imageData = UIImageJPEGRepresentation(thumbImage,1);
                    XianliaoShareLinkObject *linkObj = [[XianliaoShareLinkObject alloc] init];

                    linkObj.title = title;
                    linkObj.linkDescription = description;
                    linkObj.imageData = imageData;
                    linkObj.imageUrl =imageUrl;
                    linkObj.url = url;
                    [XianliaoApiManager share:linkObj fininshBlock:^(XianliaoShareCallBackType callBackType) {
                        NSLog(@"appShareCode:%ld", (long)callBackType);
                        NSDictionary* returedata =  @{@"code":[NSNumber numberWithInt: callBackType]};
                                               [MyXianLiao XianLiaoShareRespond: returedata];
                    }];
                }
               
           }
       }
       else
       {
           NSLog(@"分享数据错误，未获得jSON数据");
       }
    }
}


void __iosXianliaoAuthorize(NSString *xlId,NSString *xlSecret,NSString *basecode){
    NSString *getAccessCodeUrl = @"https://ssgw.updrips.com/oauth2/accessToken";
    NSString *getAccessParams= [NSString stringWithFormat:@"appid=%@&appsecret=%@&grant_type=authorization_code&code=%@",
        xlId,xlSecret,basecode];
    NSLog(@"getAccessParams:%@",getAccessParams);

    NSMutableURLRequest *request = [NSMutableURLRequest requestWithURL:[NSURL URLWithString:getAccessCodeUrl]];
    request.HTTPMethod = @"POST";
    request.HTTPBody = [getAccessParams dataUsingEncoding:NSUTF8StringEncoding];

    NSData *response = [NSURLConnection sendSynchronousRequest:request returningResponse:nil error:nil];
    if (response != nil) {
        NSError *error;
        NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:response options:NSJSONReadingMutableContainers error:&error];
        if(dict != nil){

            NSString *errmsg = [dict objectForKey:@"err_msg"];
            NSString *errcode = [dict objectForKey:@"err_code"];
            if([errcode intValue] == 0){
                NSDictionary *datadict = [dict objectForKey:@"data"];
                NSString *accessToken = [datadict objectForKey:@"access_token"];
                NSString *getInfoUrl   = @"https://ssgw.updrips.com/resource/user/getUserInfo";
                NSString *getUserInfoParams = [NSString stringWithFormat:@"access_token=%@",accessToken];
                NSLog(@"getUserInfoParams:%@",getUserInfoParams);
                NSMutableURLRequest *request =
                [NSMutableURLRequest requestWithURL:[NSURL URLWithString:getInfoUrl]];
                request.HTTPMethod = @"POST";
                request.HTTPBody = [getUserInfoParams dataUsingEncoding:NSUTF8StringEncoding];
                NSData *response = [NSURLConnection sendSynchronousRequest:request returningResponse:nil error:nil];
                if (response != nil) {
                    NSDictionary *dict = [NSJSONSerialization JSONObjectWithData:response options:NSJSONReadingMutableContainers error:&error];
                    if(dict !=NULL){
                        NSString *errmsg = [dict objectForKey:@"err_msg"];
                        NSString *errcode = [dict objectForKey:@"err_code"];
                        if([errcode intValue] == 0){
                            NSDictionary *datadict = [dict objectForKey:@"data"];
                            NSString *nickName=[datadict objectForKey:@"nickName"];
                            NSData *nsdataNick =[nickName dataUsingEncoding:NSUTF8StringEncoding];
                            NSString *base64Nick= [nsdataNick base64EncodedStringWithOptions:0];

                            NSMutableDictionary *tmpdict =[NSMutableDictionary dictionary];
                            [tmpdict setValue:@0 forKey:@"code"];//授权成功
                            [tmpdict setValue:base64Nick forKey:@"nickname"];
                            [tmpdict setValue:[datadict objectForKey:@"openId"] forKey:@"openid"];
                            [tmpdict setValue:[datadict objectForKey:@"gender"] forKey:@"sex"];
                            [tmpdict setValue:[datadict objectForKey:@"originalAvatar"] forKey:@"headimgurl"];
                            [tmpdict setValue:[datadict objectForKey:@"smallAvatar"] forKey:@"smallhead"];
                            [MyXianLiao XianLiaoAuthorizeRespond:tmpdict];
                        }else{
                            NSDictionary* returedata =  @{@"code":@2};//响应错误
                            [MyXianLiao XianLiaoAuthorizeRespond: returedata];
                            NSLog(@"response Error:errormsg=%@",errmsg);
                        }
                    }else{
                        NSDictionary* returedata =  @{@"code":@3};//获取个人信息数据响应错误
                        [MyXianLiao XianLiaoAuthorizeRespond: returedata];
                        NSLog(@"GetUserInfoResponseData Error");
                    }
                }
                else{
                    NSDictionary* returedata =  @{@"code":@4};//获取个人信息数据响应超时
                    [MyXianLiao XianLiaoAuthorizeRespond: returedata];
                    NSLog(@"requestUserInfo timeout:%@",getInfoUrl);
                }
            }
            else{
                NSDictionary* returedata =  @{@"code":@5};//个人信息响应错误
                [MyXianLiao XianLiaoAuthorizeRespond: returedata];
                NSLog(@"response Error:errormsg=%@",errmsg);
            }
        }
        else{
            NSDictionary* returedata =  @{@"code":@6};//getCodeResponseData
            [MyXianLiao XianLiaoAuthorizeRespond: returedata];
            NSLog(@"getCodeResponseData Error");
        }
    }
    else{
        NSLog(@"requestCode timeout:%@",getAccessCodeUrl);
    }
}



//授权回调
+(void)XianLiaoAuthorizeRespond:(NSDictionary *)data
{
    NSString *jsonStr = [MyXianLiao transformationToString:data];
    NSLog(@"XianLiaoAuthorizeRespond %@",jsonStr);
    NSString *str = [NSString stringWithFormat:@"XianLiaoAuthorizeRespond|%@",jsonStr];
    UnitySendMessage("SdkObject", "ReceiveMessage",  [str UTF8String]);
}


// 分享回调
+(void)XianLiaoShareRespond:(NSDictionary *)data
{
    NSString *jsonStr = [MyXianLiao transformationToString:data];
    NSLog(@"XianLiaoShareRespond %@",jsonStr);
    NSString *str = [NSString stringWithFormat:@"XianLiaoShareRespond|%@",jsonStr];
    UnitySendMessage("SdkObject", "ReceiveMessage",  [str UTF8String]);
}
//---------------------------------------------------------------------------------------------------------------------
//闲聊统一回调消息


//---------------------------------------------内部函数-----------------------------------------------------------------
-(bool)initsdk :(NSString *)xlid :(NSString *)xlsecret {
    self.xlId = xlid;
    self.xlSecret = xlsecret;
    [XianliaoApiManager registerApp:xlid];
    return true;
}

- (bool)isxlappinstalled{
    return [XianliaoApiManager isInstallXianliao];
}

-(bool) openxlapp{
    if([XianliaoApiManager isInstallXianliao]){
        XianliaoShareTextObject *textObject = [[XianliaoShareTextObject alloc]init];
        NSString *text = [NSString stringWithUTF8String:__iosCommSdkGetPastboard()];
        textObject.text = text;
        [XianliaoApiManager share:textObject fininshBlock:^(XianliaoShareCallBackType callBackType) {
            NSLog(@"textShareCode:%ld",(long)callBackType);
            NSDictionary* returedata =  @{@"code":[NSNumber numberWithInt: callBackType]};
            [MyXianLiao XianLiaoShareRespond: returedata];
        }];
    }
    return true;
}
//获取剪切板
char* __iosCommSdkGetPastboard(){
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

-(void) authorize {
    [XianliaoApiManager loginState:nil fininshBlock:^(XianliaoLoginCallBackType
    callBackType, NSString *code, NSString *state) {
        NSLog(@"callBackType:%lu, code:%@, state:%@", (unsigned
    long)callBackType, code, state);
        long retcode = (unsigned long)callBackType;
        if(retcode == 0){
            __iosXianliaoAuthorize(self.xlId,self.xlSecret,code);
        }
        else{
            NSDictionary* returedata =  @{@"code":@1};//取消登录
            [MyXianLiao XianLiaoAuthorizeRespond: returedata];
        }
    }];
}

/**字典或者数组转化成json串*/
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

+ (UIImage *)thumbnailWithImageWithoutScale:(UIImage *)image size:(CGSize)asize
{
    UIImage *newimage;
    if (nil == image) {
        newimage = nil;
    }
    else{
        CGSize oldsize = image.size;
        CGRect rect;
        if (asize.width/asize.height > oldsize.width/oldsize.height) {
            rect.size.width = asize.height*oldsize.width/oldsize.height;
            rect.size.height = asize.height;
            rect.origin.x = (asize.width - rect.size.width)/2;
            rect.origin.y = 0;
        }
        else{
            rect.size.width = asize.width;
            rect.size.height = asize.width*oldsize.height/oldsize.width;
            rect.origin.x = 0;
            rect.origin.y = (asize.height - rect.size.height)/2;
        }
        UIGraphicsBeginImageContext(asize);
        CGContextRef context = UIGraphicsGetCurrentContext();
        CGContextSetFillColorWithColor(context, [[UIColor clearColor] CGColor]);
        UIRectFill(CGRectMake(0, 0, asize.width, asize.height));//clear background
        [image drawInRect:rect];
        newimage = UIGraphicsGetImageFromCurrentImageContext();
        UIGraphicsEndImageContext();
    }
    return newimage;
}

@end
