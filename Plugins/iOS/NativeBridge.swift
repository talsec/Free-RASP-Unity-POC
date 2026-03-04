import Foundation

import TalsecRuntime

// Declare the C function using @_silgen_name
@_silgen_name("send_message_to_unity")
func send_message_to_unity(_ threatType: UnsafePointer<CChar>)

extension SecurityThreatCenter: SecurityThreatHandler, RaspExecutionState {
    
    public func onAllChecksFinished() {
        "onAllChecksFinished".withCString { messagePtr in
            send_message_to_unity(messagePtr)
        }
    }

    public func threatDetected(_ securityThreat: TalsecRuntime.SecurityThreat) {

        var message = "unknown";
        // Handle each threat type individually
        switch securityThreat {
            case .signature:
                message = "onAppIntegrity"
            case .jailbreak:
                message = "onPrivilegedAccess"
            case .debugger:
                message = "onDebug"
            case .runtimeManipulation:
                message = "onRuntimeManipulation"
            case .passcode:
                message = "onPasscode"
            case .passcodeChange:
                message = "onPasscodeChange"
            case .simulator:
                message = "onSimulator"
            case .missingSecureEnclave:
                message = "onSecureHardwareNotAvailable"
            case .deviceChange:
                message = "onDeviceBinding"
            case .deviceID:
                message = "onDeviceID"
            case .unofficialStore:
                message = "onUnofficialStore"
            case .systemVPN:
                message = "onSystemVPN"
            case .screenshot:
                message = "onScreenshot"
            case .screenRecording:
                message = "onScreenRecording"
            case .timeSpoofing:
                message = "onTimeSpoofing"
        }

        message.withCString { messagePtr in
            send_message_to_unity(messagePtr)
        }
    }
}

@objc public class NativeBridge: NSObject {

    @objc public static func initTalsec(_ appBundleIds: [String], _ appTeamId: String, _ watcherMailAddress: String, _ isProd: Bool) {
        let config = TalsecConfig(
            appBundleIds: appBundleIds,
            appTeamId: appTeamId,
            watcherMailAddress: watcherMailAddress,
            isProd: isProd
        )
        Talsec.start(config: config)
    }
}
