#import <Foundation/Foundation.h>
#import "WXApi.h"

@interface MyWeChat : NSObject<WXApiDelegate>
    @property (strong,nonatomic) NSString *wxId;
    @property (strong,nonatomic) NSString *wxSecret;
+ (instancetype)Instance;
- (bool)initsdk :(NSString *)wxid :(NSString *)wxsecret; //初始化sdk
- (bool)iswxappinstalled;                                //微信app是否安装
- (bool)openwxapp;                                       //打开微信
- (void)authorize;                                       //微信授权
+ (void)shareText:(NSString *)text                       //分享文字
    InScene:(enum WXScene)scene;
+ (void)shareImageData:(NSData *)imageData               //分享图片
    TagName:(NSString *)tagName
    MessageExt:(NSString *)messageExt
    Action:(NSString *)action
    ThumbImage:(UIImage *)thumbImage
    InScene:(enum WXScene)scene;
+ (void)shareLinkURL:(NSString *)urlString              //分享链接
    TagName:(NSString *)tagName
    Title:(NSString *)title
    Description:(NSString *)description
    ThumbImage:(UIImage *)thumbImage
    InScene:(enum WXScene)scene;
@end
