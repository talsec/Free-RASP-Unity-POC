#import <Foundation/Foundation.h>

extern "C" {
    void UnitySendMessage(const char* obj, const char* method, const char* msg);
    
    // Simple C function for Swift to call
    void send_message_to_unity(const char* threatType) {
        UnitySendMessage("TalsecPlugin", "scanResultIOS", threatType);
    }
}