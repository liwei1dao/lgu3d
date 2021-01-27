//
//  XianliaoApiManager.h
//  XianliaoApi
//
//  Created by bu88 on 2017/3/15.
//  Copyright © 2017年 HHJ. All rights reserved.
//
// 这个类是用于设置闲聊配置信息的

#import <Foundation/Foundation.h>
#import "XianliaoApiObject.h"

@interface XianliaoApiManager : NSObject


/**
 设置出错的时候是否显示信息，默认是false，会直接跳过，在调试的时候应该打开。
 */
+ (void)showLog:(BOOL)show;

/**
 判断是否安装了闲聊
 */
+ (BOOL)isInstallXianliao;

/**
 获得闲聊Api的版本号
 */
+ (NSString *)getApiVersion;

/*
 向闲聊终端程序注册第三方应用，此方法只能执行一次。
 */
+ (void)registerApp:(NSString *)appid;

/**
 用户从闲聊调用你的APP时，需要从这个方法获得闲聊传递的内容。
 需要在AppDelegate的两个方法中执行
 （iOS 9或以上） application:openURL:options:
 （iOS 9以下） application:openURL:sourceApplication:annotation:
 
 @param url AppDelegate方法中的url
 @return 是否是Xianliao的调用
 */
+ (BOOL)handleOpenURL:(NSURL *)url;

/**
 分享接口
 
 @param object 分享的对象
 @param callBackBlock 分享结束后用户返回APP时会执行的回调block
 */
+ (void)share:(XianliaoShareBaseObject *)object fininshBlock:(XianliaoShareCallBackBlock)callBackBlock;

/**
 登录接口
 
 @param state 用于保持请求和回调的状态，授权请求后原样带回给第三方。
 @param callBackBlock 分享结束后用户返回APP时会执行的回调block
 */
+ (void)loginState:(NSString *)state fininshBlock:(XianliaoLoginCallBackBlock)callBackBlock;


/**
 注册从闲聊跳转过来的应用调用，注册了以后从闲聊调起你的APP时会以下的block，如果重复执行这个方法，会执行最后注册的block
 */
+ (void)getAppFromXianliao:(XianliaoAppBlock)block;


@end
