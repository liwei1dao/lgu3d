#import <Foundation/Foundation.h>
#import "XianliaoApiManager.h"
#import "XianliaoApiObject.h"
@interface MyXianLiao : NSObject
    @property (strong,nonatomic) NSString *xlId;
    @property (strong,nonatomic) NSString *xlSecret;

+ (instancetype)Instance;
- (bool)initsdk :(NSString *)xlid :(NSString *)xlsecret; //初始化sdk
- (bool)isxlappinstalled;                                //微信app是否安装
- (bool)openxlapp;                                       //打开应用
- (void)authorize;                                       //微信授权
+ (void)shareText:(NSString *)text;                      //分享文字
+ (void)shareImageData:(NSData *)imageData               //分享图片
                TagName:(NSString *)tagName
                MessageExt:(NSString *)messageExt
                Action:(NSString *)action
                ThumbImage:(UIImage *)thumbImage;

+ (void)shareLinkURL:(NSString *)urlString              //分享链接
    TagName:(NSString *)tagName
    Title:(NSString *)title
    Description:(NSString *)description
    ThumbImage:(UIImage *)thumbImage;
    
+(void)XianLiaoShareRespond:(NSDictionary *)data;
+(void)XianLiaoAuthorizeRespond:(NSDictionary *)data;
@end
