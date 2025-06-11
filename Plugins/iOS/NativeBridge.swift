import Foundation

import TalsecRuntime

// Declare the C function using @_silgen_name
@_silgen_name("send_message_to_unity")
func send_message_to_unity(_ threatType: UnsafePointer<CChar>)

extension SecurityThreatCenter: SecurityThreatHandler {
    public func threatDetected(_ securityThreat: TalsecRuntime.SecurityThreat) {

        var message = "unknown";
        // Handle each threat type individually
        switch securityThreat {
            case .signature:
                message = "signature"
                // handleAppIntegrityThreat()
            case .jailbreak:
                message = "jailbreak"
                // handleJailbreakThreat()
            case .debugger:
                message = "debugger"
                // handleDebuggerThreat()
            case .runtimeManipulation:
                message = "runtimeManipulation"
                // handleRuntimeManipulationThreat()
            case .passcode:
                message = "passcode"
                // handlePasscodeThreat()
            case .passcodeChange:
                message = "passcodeChange"
            case .simulator:
                message = "simulator"
                // handleSimulatorThreat()
            case .missingSecureEnclave:
                message = "missingSecureEnclave"
            case .deviceChange:
                message = "deviceChange"
                // handleDeviceBindingThreat()
            case .deviceID:
                message = "deviceID"
                // handleDeviceIDThreat()
            case .unofficialStore:
                message = "unofficialStore"
            case .systemVPN:
                message = "systemVPN"
                // handleSystemVPNThreat()
            case .screenshot:
                message = "screenshot"
                // handleScreenshotThreat()
            case .screenRecording:
                message = "screenRecording"
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
