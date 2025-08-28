
using System.Collections.Generic;
using UnityEngine;

public interface ThreatDetectedCallback
{
    void onPrivilegedAccess();
    void onAppIntegrity();
    void onDebug();
    void onDeviceID();
    void onDeviceBinding();
    void onSimulator();
    void onUnofficialStore();
    void onHook();
    void onObfuscationIssues();
    void onScreenshot();
    void onScreenRecording();
    void onPasscode();
    void onPasscodeChange();
    void onSecureHardwareNotAvailable();
    void onDevMode();
    void onADBEnabled();
    void onSystemVPN();
} 