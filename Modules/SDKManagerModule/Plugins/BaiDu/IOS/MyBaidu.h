#import <Foundation/Foundation.h>
#import <BMKLocationKit/BMKLocationComponent.h>

@interface MyBaidu : NSObject<BMKLocationManagerDelegate,BMKLocationAuthDelegate>{
    BMKLocationManager* locationManager;
}
+ (instancetype)Instance;
-(bool)initsdk:(NSString *)bdid;
-(void)startLocation;
-(void)stopLocation;
@end
