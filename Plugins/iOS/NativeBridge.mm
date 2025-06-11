#import <Foundation/Foundation.h>
#import "UnityFramework/UnityFramework-Swift.h"

extern "C" {

    void _initTalsec(const char** appBundleIds, int appBundleIdsCount, const char* appTeamId, const char* watcherMailAddress, bool isProd) {
        // Convert C string array to NSArray
        NSMutableArray* nsAppBundleIds = [[NSMutableArray alloc] init];
        for (int i = 0; i < appBundleIdsCount; i++) {
            NSString* bundleId = [NSString stringWithUTF8String:appBundleIds[i]];
            [nsAppBundleIds addObject:bundleId];
        }
        
        NSString* nsAppTeamId = [NSString stringWithUTF8String:appTeamId];
        NSString* nsWatcherMailAddress = [NSString stringWithUTF8String:watcherMailAddress];
        [NativeBridge initTalsec:nsAppBundleIds :nsAppTeamId :nsWatcherMailAddress :isProd];
    }
}