
#import <Foundation/Foundation.h>
#import <CoreLocation/CoreLocation.h>//引入Corelocation框架

@interface BdLocationManager :  NSObject<CLLocationManagerDelegate>{
    
    CLLocationManager* locationManager;
	int currReqId;
}


+ (instancetype)sharedManager;
-(bool)initsdk:(NSString *)bdid;
-(void)startLocation:(int)reqid;

+(void)IOSlocalInitRespond:(NSDictionary *)data;
@end

