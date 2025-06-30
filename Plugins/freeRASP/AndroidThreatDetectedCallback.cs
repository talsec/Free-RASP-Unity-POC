using System.Collections.Generic;
using UnityEngine;

public interface AndroidThreatDetectedCallback
{
    void onRootDetected();
    void onDebuggerDetected();
    void onEmulatorDetected();
    void onTamperDetected();
    void onUntrustedInstallationSourceDetected();
    void onHookDetected();
    void onDeviceBindingDetected();
    void onObfuscationIssuesDetected();
    void onScreenshotDetected();
    void onScreenRecordingDetected();
} 