//
//  XianliaoApiObject.h
//  XianliaoApi
//
//  Created by bu88 on 2017/3/15.
//  Copyright © 2017年 HHJ. All rights reserved.
//

#import <UIKit/UIKit.h>

#pragma mark:-----闲聊Api分享和登录的基础部分----
/**
 闲聊分享和登录对象基类
 */
@interface XianliaoApiObject : NSObject

@end


#pragma mark:-----分享-----
/**
 分享回调的情景
 
 - XianliaoShareSuccesslType: 分享成功
 - XianliaoShareCancelType: 分享取消
 - XianliaoShareErrorType: 分享失败
 - XianliaoShareUnkonwType: 未知
 */
typedef NS_ENUM(NSInteger, XianliaoShareCallBackType) {
    XianliaoShareSuccesslType = 0,
    XianliaoShareCancelType,
    XianliaoShareErrorType,
    XianliaoShareUnkonwType,
};
/// 分享的回调block
typedef void (^XianliaoShareCallBackBlock)(XianliaoShareCallBackType callBackType);


/**
 分享类型
 
 - XianliaoShareTextObjectType: 文本分享类型
 - XianliaoShareImageObjectType: 图片分享类型
 - XianliaoShareAppObjectType: 应用分享类型
 - XianliaoShareLinkObjectType: 链接分享类型
 */
typedef NS_ENUM(NSInteger, XianliaoShareObjectType) {
    XianliaoShareTextObjectType = 0,
    XianliaoShareImageObjectType,
    XianliaoShareAppObjectType,
    XianliaoShareLinkObjectType = 10,
};


/**
 闲聊分享基类
 */
@interface XianliaoShareBaseObject : XianliaoApiObject
/// 分享类型
@property(nonatomic, assign, readonly) XianliaoShareObjectType type;

@end


/**
 文本类型的分型，文本分享必须传分享内容（如果不传分享内容则无法分享）
 */
@interface XianliaoShareTextObject : XianliaoShareBaseObject
/// 分享内容
@property(nonatomic, copy) NSString *text;

@end


/**
 图片类型的分型，图片分享必须传分享图片URL
 */
@interface XianliaoShareImageObject : XianliaoShareBaseObject
/// 分享图片URL
@property(nonatomic, copy) NSString *imageUrl;
/// 分享的图片本身
@property(nonatomic, strong) NSData *imageData;

@end


/**
 应用分享类型，应用分享必须传递应用标题，应用描述和应用缩略图
 */
@interface XianliaoShareAppObject : XianliaoShareBaseObject
/// 应用房间号
@property(nonatomic, copy) NSString *roomToken;
/// 应用房间标识
@property(nonatomic, copy) NSString *roomId;
/// 应用标题
@property(nonatomic, copy) NSString *title;
/// 应用描述
@property(nonatomic, copy) NSString *text;
/// 应用缩略图URL
@property(nonatomic, copy) NSString *imageUrl;
/// 应用缩略图本身
@property(nonatomic, strong) NSData *imageData;
/// 安卓下載地址
@property (nonatomic, copy) NSString *androidDownloadUrl;
/// iOS下載地址
@property (nonatomic, copy) NSString *iOSDownloadUrl;

@end

@interface XianliaoShareLinkObject : XianliaoShareBaseObject

/// 链接标题
@property(nonatomic, copy) NSString *title;
/// 链接描述
@property(nonatomic, copy) NSString *linkDescription;
/// 链接缩略图URL
@property(nonatomic, copy) NSString *imageUrl;
/// 链接缩略图本身
@property(nonatomic, strong) NSData *imageData;
/// 链接
@property(nonatomic, copy) NSString *url;

@end


#pragma mark:-----登录-----
/**
 登录的回调场景
 
 - XianliaoLoginSuccessType: 登录成功
 - XianliaoLoginCancelType: 登录取消
 - XianliaoLoginErrorType: 登录错误
 - XianliaoLoginUnkonwType: 未知
 */
typedef NS_ENUM(NSInteger, XianliaoLoginCallBackType) {
    XianliaoLoginSuccessType = 0,
    XianliaoLoginCancelType,
    XianliaoLoginErrorType,
    XianliaoLoginUnkonwType,
};
/// 登录的回调block
typedef void (^XianliaoLoginCallBackBlock)(XianliaoLoginCallBackType callBackType, NSString *code, NSString *state);


#pragma mark:-----应用-----
/// 登录的调用block
typedef void (^XianliaoAppBlock)(NSString *roomToken, NSString *roomId, NSNumber *openId);
