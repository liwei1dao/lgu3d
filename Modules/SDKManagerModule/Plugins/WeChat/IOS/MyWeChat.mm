
#import "MyWeChat.h"

@implementation MyWeChat
+(instancetype)Instance {
    static dispatch_once_t onceToken;
    static MyWeChat *instance;
    dispatch_once(&onceToken, ^{
        instance = [[MyWeChat alloc] init];
    });
    return instance;
}
// - (BOOL)application:(UIApplication*)application didFinishLaunchingWithOptions:(NSDictionary*)launchOptions
// {
//     instance = self;
//     [super application:application didFinishLaunchingWithOptions:launchOptions];
//     return YES;
// }

// - (BOOL)application:(UIApplication *)application handleOpenURL:(NSURL *)url
// {
//     return [WXApi handleOpenURL:url delegate:self];
// }

// - (BOOL)application:(UIApplication *)app openURL:(NSURL *)url options:(nonnull NSDictionary *)options
// {
//     return [WXApi handleOpenURL:url delegate:self];
// }

// - (BOOL)application:(UIApplication*)application openURL:(NSURL*)url sourceApplication:(NSString*)sourceApplication annotation:(id)annotation
// {
//     return [WXApi handleOpenURL:url delegate:self];
// }

extern "C"
{
    //初始化sdk
    bool __initsdk_wx(char *wxAppId, char *wxAppSecret){
        NSString *wxid = [NSString stringWithUTF8String:wxAppId];
        NSString *wxsecret = [NSString stringWithUTF8String:wxAppSecret];
        NSLog(@"初始化微信sdk wxid:%@ wxsecret:%@",wxid,wxsecret);
        return [[MyWeChat Instance] initsdk:wxid :wxsecret];
    }
    
    //检测是否安装客户端
    bool __isappinstalled_wx(){
        return [[MyWeChat Instance] iswxappinstalled];
    }
    //获取授权
    void __authorize_wx(){
        NSLog(@"启动微信授权");
        [[MyWeChat Instance] authorize];
    }
    
    bool __openapp_wx(){
        NSLog(@"打开微信");
        return [[MyWeChat Instance] openwxapp];
    }
    
    void __pay_wx(char *orderJson){
        NSLog(@"启动微信购买");
    }
    
    void __sharecontent_wx(int platformType, char *contentStr){
        NSLog(@"启动微信分享 platform= %d ,contentStr = %s",platformType,contentStr);

       NSString *contentString = [NSString stringWithUTF8String:contentStr];
       NSData *contentData = [contentString dataUsingEncoding:NSUTF8StringEncoding];
       NSError *error;

       NSMutableDictionary *dict  = [NSJSONSerialization JSONObjectWithData:contentData options:NSJSONReadingMutableLeaves error:&error];

       if(dict !=nil){

           NSString *shareType = [dict objectForKey:@"shareType"];

           if(shareType.intValue ==1){ // text
               NSString *text = [dict objectForKey:@"text"];
               WXScene inScene =platformType==1 ? WXSceneTimeline : WXSceneSession;
               [MyWeChat shareText:text InScene:inScene];
               
           }
           else if(shareType.intValue ==2)
           {
               //图片
               NSString *imagePath = [dict objectForKey:@"imageUrl"];
               if(imagePath == nil || imagePath == NULL || [imagePath isEqualToString:@""])
               {
                   imagePath = [dict objectForKey:@"imagePath"];
               }
               WXScene inScene =platformType==1 ? WXSceneTimeline : WXSceneSession;
               UIImage * thumbImage = [UIImage imageWithContentsOfFile:imagePath];

               NSData * imageData = UIImageJPEGRepresentation(thumbImage,0.5f);
               [MyWeChat shareImageData:imageData
                                           TagName:nil
                                       MessageExt:nil
                                           Action:nil
                                       ThumbImage:thumbImage
                                       InScene:inScene];
           }
           else if(shareType.intValue ==4){//链接分享

               NSString *title = [dict objectForKey:@"title"];
               NSString *description = [dict objectForKey:@"text"];
               NSString *url = [dict objectForKey:@"url"];

               WXScene inScene =platformType==1 ? WXSceneTimeline : WXSceneSession;
               UIImage *thumbImage = [UIImage imageNamed:@"AppIcon57x57"];
              
               [MyWeChat shareLinkURL:url
                                           TagName:nil
                                           Title:title
                                       Description:description
                                       ThumbImage:thumbImage
                                           InScene:inScene];
           }
          
       }
       else{
           NSLog(@"分享数据错误，未获得jSON数据");
       }
    }
}

//授权回调
+(void)WechatAuthorizeRespond:(NSDictionary *)data{
    NSString *jsonStr = [MyWeChat transformationToString:data];
    NSLog(@"WechatAuthorizeRespond %@",jsonStr);
    NSString *str = [NSString stringWithFormat:@"WechatAuthorizeRespond|%@",jsonStr];
    UnitySendMessage("SdkObject", "ReceiveMessage",  [str UTF8String]);
}


// 分享回调
+(void)WechatShareRespond:(NSDictionary *)data{
    NSString *jsonStr = [MyWeChat transformationToString:data];
    NSLog(@"WechatAuthorizeRespond %@",jsonStr);
    NSString *str = [NSString stringWithFormat:@"WechatShareRespond|%@",jsonStr];
    UnitySendMessage("SdkObject", "ReceiveMessage",  [str UTF8String]);
}
//---------------------------------------------------------------------------------------------------------------------
//微信统一回调消息
-(void) onResp:(BaseResp *)resp
{
    if([resp isKindOfClass:[SendAuthResp class]])
    {//授权回调
        SendAuthResp *temp = (SendAuthResp*)resp;
        int errorCode = temp.errCode;
        NSDictionary *returedata;
        switch (errorCode) {
            case 0:
                {
                    printf("登录成功-xcode");
                    NSString *code = temp.code;
                    [self getWeiXinOpenId:code];
                    break;
                }
            case -2:
                printf("用户取消");
                returedata =  @{@"code":@1};
                [MyWeChat WechatAuthorizeRespond: returedata];
                break;
            case -4:
                printf("用户拒绝");
                returedata =  @{@"code":@2};
                [MyWeChat WechatAuthorizeRespond: returedata];
                break;
            default:
                printf("授权失败");
                returedata =  @{@"code":@3};
                [MyWeChat WechatAuthorizeRespond: returedata];
                break;
        }
    }else if ([resp isKindOfClass:[PayResp class]])
    {//支付会回调
        switch (resp.errCode) {
            case WXSuccess:
                printf("支付成功");
//                UnitySendMessage("[_WechatHelper]", "LoginFail", "登录失败");
                break;
                
            default:
                printf("支付失败");
//                UnitySendMessage("[_WechatHelper]", "LoginFail", "登录失败");
                break;
        }
    }else if ([resp isKindOfClass:[SendMessageToWXResp class]]) {
        //  分享返回结果
        NSLog(@"分享返回结果，retcode = %d, retstr = %@", resp.errCode,resp.errStr);
        int statecode = 3;
        if (resp.errCode == 0)
        {
            statecode = 1;
        }
        NSDictionary* returedata =  @{@"code":[NSNumber numberWithInt: statecode]};
        [MyWeChat WechatShareRespond: returedata];
    }
}


//---------------------------------------------内部函数-----------------------------------------------------------------
-(bool)initsdk :(NSString *)wxid :(NSString *)wxsecret {
    self.wxId = wxid;
    self.wxSecret = wxsecret;
    return [WXApi registerApp:wxid universalLink:@"https://www.xxx.com/iosuniversallink/"];
}

- (bool)iswxappinstalled{
    return [WXApi isWXAppInstalled];
}

-(bool)openwxapp{
    return [WXApi openWXApp];
}

-(void) authorize {
    printf("测试成功哦");
    SendAuthReq *req = [[SendAuthReq alloc] init];
    req.state = @"wx_oauth_authorization_state";//用于保持请求和回调的状态，授权请求或原样带回
    req.scope = @"snsapi_userinfo";//授权作用域：获取用户个人信息
    //唤起微信
    [WXApi sendReq:req completion:^(BOOL success) {}];
}

//通过code获取access_token，openid，unionid
- (void)getWeiXinOpenId:(NSString *)code{
    NSString *url =[NSString stringWithFormat:@"https://api.weixin.qq.com/sns/oauth2/access_token?appid=%@&secret=%@&code=%@&grant_type=authorization_code",self.wxId,self.wxSecret,code];
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        NSURL *zoneUrl = [NSURL URLWithString:url];
        NSString *zoneStr = [NSString stringWithContentsOfURL:zoneUrl encoding:NSUTF8StringEncoding error:nil];
        NSData *data1 = [zoneStr dataUsingEncoding:NSUTF8StringEncoding];
        
        if (!data1) {
//            [self showError:@"微信授权失败"];
            NSDictionary* returedata =  @{@"code":@3};
            [MyWeChat WechatAuthorizeRespond: returedata];
            return ;
        }
        
        // 授权成功，获取token、openID字典
        NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:data1 options:NSJSONReadingMutableContainers error:nil];
        NSLog(@"token、openID字典===%@",dic);
        NSString *access_token = dic[@"access_token"];
        NSString *openid= dic[@"openid"];
        
        //获取微信用户信息
        [self getUserInfoWithAccessToken:access_token WithOpenid:openid];
    });
}

//通过access_token 和 openid 获取用户信息
-(void)getUserInfoWithAccessToken:(NSString *)access_token WithOpenid:(NSString *)openid
{
    NSString *url =[NSString stringWithFormat:@"https://api.weixin.qq.com/sns/userinfo?access_token=%@&openid=%@",access_token,openid];
    
    dispatch_async(dispatch_get_global_queue(DISPATCH_QUEUE_PRIORITY_DEFAULT, 0), ^{
        NSURL *zoneUrl = [NSURL URLWithString:url];
        NSString *zoneStr = [NSString stringWithContentsOfURL:zoneUrl encoding:NSUTF8StringEncoding error:nil];
        NSData *data = [zoneStr dataUsingEncoding:NSUTF8StringEncoding];
        dispatch_async(dispatch_get_main_queue(), ^{
            
            // 获取用户信息失败
            if (!data) {
                NSDictionary* returedata =  @{@"code":@3};
                [MyWeChat WechatAuthorizeRespond: returedata];
                return;
            }
            
            // 获取用户信息字典
            NSDictionary *dic = [NSJSONSerialization JSONObjectWithData:data options:NSJSONReadingMutableContainers error:nil];
            //用户信息中没有access_token 我将其添加在字典中
            [dic setValue:@0 forKey:@"code"];
            [dic setValue:access_token forKey:@"token"];
            NSLog(@"用户信息字典:===%@",dic);
            //保存改用户信息(我用单例保存)
            [MyWeChat WechatAuthorizeRespond: dic];
        });
    });
}

+(void) shareText:(NSString *)text InScene:(enum WXScene)scene{
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = YES;
    req.scene = scene;
    req.text = text;
    [WXApi sendReq:req completion:^(BOOL success) {}];
}

+(void) shareImageData:(NSData *)imageData
              TagName:(NSString *)tagName
           MessageExt:(NSString *)messageExt
               Action:(NSString *)action
           ThumbImage:(UIImage *)thumbImage
              InScene:(enum WXScene)scene {
    
    WXImageObject *imgObject = [WXImageObject object];
    imgObject.imageData = imageData;
    
    WXMediaMessage *message = [WXMediaMessage message];
    message.mediaObject = imgObject;
    
    UIImage *minImage=[self thumbnailWithImageWithoutScale:thumbImage size:CGSizeMake(160, 90)];
    [message setThumbImage:minImage];
    
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.scene = scene;
    req.message = message;
    [WXApi sendReq:req completion:^(BOOL success) {}];
}


+(void) shareLinkURL:(NSString *)urlString
            TagName:(NSString *)tagName
              Title:(NSString *)title
        Description:(NSString *)description
         ThumbImage:(UIImage *)thumbImage
            InScene:(enum WXScene)scene {
    
    WXWebpageObject *webpageObj = [WXWebpageObject object];
    webpageObj.webpageUrl = urlString;

    WXMediaMessage *message = [WXMediaMessage message];
    message.mediaObject = webpageObj;
    message.title = title;
    message.description = description;
    [message setThumbImage:thumbImage];
    
    SendMessageToWXReq *req = [[SendMessageToWXReq alloc] init];
    req.bText = NO;
    req.scene = scene;
    req.message = message;
    [WXApi sendReq:req completion:^(BOOL success) {}];
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
