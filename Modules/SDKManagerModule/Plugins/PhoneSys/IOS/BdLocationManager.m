
#import "BdLocationManager.h"
@implementation BdLocationManager

//#pragma mark - LifeCycle
+(instancetype)sharedManager {
    static dispatch_once_t onceToken;
    static BdLocationManager *instance;
    dispatch_once(&onceToken, ^{
        instance = [[BdLocationManager alloc] init];
    });
    return instance;
}

//普通态
-(void)startLocation:(int) reqid
{   
	currReqId = reqid;
    if(!locationManager){
        locationManager =[[CLLocationManager alloc] init];
        locationManager.delegate = self;
        locationManager.desiredAccuracy = kCLLocationAccuracyBest;//设置精度
        locationManager.distanceFilter = 300.0f;//距离过滤
    }
    NSLog(@"~~~~~~~~~~~初始化定位_locService: %@",locationManager);
    [locationManager requestWhenInUseAuthorization];
	[locationManager startUpdatingLocation];

}

-(void)locationManager:(CLLocationManager *)manager didFailWithError:(NSError *)error {
    
    NSLog(@"location error");
    NSString *error_code= @"167";
    NSString *error_msg = @"用户拒绝授权";
    NSString *battery = @"1";
    NSDictionary *posDict=[NSDictionary dictionaryWithObjectsAndKeys:error_code,@"error_code",error_msg,@"error_msg",battery,@"batteryLevel",nil];

    NSNumber *reqid=[NSNumber numberWithInt: currReqId];
    NSNumber *action=[NSNumber numberWithInt :4];
    NSNumber *state=[NSNumber numberWithInt: 2];

    NSDictionary *respDict=[NSDictionary dictionaryWithObjectsAndKeys:reqid,@"reqID",action,@"action",state,@"status",posDict,@"res",nil];

//    NSData *jsonData = [NSJSONSerialization dataWithJSONObject:respDict options:NSJSONWritingPrettyPrinted error:&error];
//    NSString *userinfoJson=[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];

    [BdLocationManager IOSlocalInitRespond:respDict];
    // UnitySendMessage([[Constant sharedInstance].u3dGameObj UTF8String],[[Constant sharedInstance].u3dMethod_Callback UTF8String],[userinfoJson UTF8String]);
}

-(void)locationManager:(CLLocationManager *)manager didUpdateLocations:(NSArray<CLLocation *> *)locations {
   // [self.locationManager stopUpdatingLocation];//停止定位
    NSLog(@"~~~~~~~~~~~定位返回");
    //地理反编码
    CLLocation *currentLocation = [locations lastObject];
    CLGeocoder *geoCoder = [[CLGeocoder alloc]init];
//    //当系统设置为其他语言时，可利用此方法获得中文地理名称
//    NSMutableArray
//    *userDefaultLanguages = [[NSUserDefaults standardUserDefaults]objectForKey:@"AppleLanguages"];
//    // 强制 成 简体中文
//    [[NSUserDefaults standardUserDefaults] setObject:[NSArray arrayWithObjects:@"zh-hans", nil]forKey:@"AppleLanguages"];
    [geoCoder reverseGeocodeLocation:currentLocation completionHandler:^(NSArray<CLPlacemark *> * _Nullable placemarks, NSError * _Nullable error) {
        if (placemarks.count > 0) {
            CLPlacemark *placeMark = placemarks[0];
            NSString *city = placeMark.locality;
            if (!city) {
                 NSLog(@"⟳定位获取失败,点击重试null");
            } else {
                NSString *issimulater = @"0";
                NSString *battery = @"1";
                float latitude =placeMark.location.coordinate.latitude;
                float longitude =placeMark.location.coordinate.longitude;
                NSLog(@"location:%f,%f",latitude,longitude);
                NSString *latitudeStr=[NSString stringWithFormat:@"%f",latitude];
                NSString *longitudeStr=[NSString stringWithFormat:@"%f",longitude];
                NSDictionary *posDict=[NSDictionary dictionaryWithObjectsAndKeys:latitudeStr,@"latitude",longitudeStr,@"longitude",city,@"city",issimulater,@"issimulater",battery,@"batteryLevel",nil];
                
                NSNumber *reqid=[NSNumber numberWithInt: currReqId];
                NSNumber *action=[NSNumber numberWithInt :4];
                NSNumber *state=[NSNumber numberWithInt: 1];

                if(currReqId == -1){
                    currReqId = 0;
                }

                NSDictionary *respDict=[NSDictionary dictionaryWithObjectsAndKeys:reqid,@"reqID",action
                 ,@"action",state,@"status",posDict,@"res",nil];

//                NSData *jsonData = [NSJSONSerialization dataWithJSONObject:respDict options:NSJSONWritingPrettyPrinted error:&error];
//                NSString *userinfoJson=[[NSString alloc] initWithData:jsonData encoding:NSUTF8StringEncoding];
                [BdLocationManager IOSlocalInitRespond: respDict];
                // UnitySendMessage([[Constant sharedInstance].u3dGameObj UTF8String],[[Constant sharedInstance].u3dMethod_Callback UTF8String],[userinfoJson UTF8String]);
            }
            
        } else if (error == nil && placemarks.count == 0 ) {
             NSLog(@"⟳定位获取失败,点击重试0");
        } else if (error) {
             NSLog(@"⟳定位获取失败,点击重试error");
        }
     
//        // 还原Device 的语言
//        [[NSUserDefaults
//          standardUserDefaults] setObject:userDefaultLanguages
//         forKey:@"AppleLanguages"];
    }];
}

// 分享回调
+(void)IOSlocalInitRespond:(NSDictionary *)data
{
    NSString *jsonStr = [BdLocationManager transformationToString:data];
    NSLog(@"IOSlocalInitRespond %@",jsonStr);
    NSString *str = [NSString stringWithFormat:@"IOSlocalInitRespond|%@",jsonStr];
    UnitySendMessage("SdkObject", "ReceiveMessage",  [str UTF8String]);
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

@end

