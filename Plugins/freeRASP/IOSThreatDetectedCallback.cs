using System.Collections.Generic;
using UnityEngine;

public interface IOSThreatDetectedCallback
{
    void signatureDetected();
    void jailbreakDetected();
    void debuggerDetected();
    void runtimeManipulationDetected();
    void passcodeDetected();
    void passcodeChangeDetected();
    void simulatorDetected();
    void missingSecureEnclaveDetected();
    void deviceBindingDetected();
    void unofficialStoreDetected();
    void systemVPNDetected();
    void screenshotDetected();
    void screenRecordingDetected();
    void deviceIDDetected();
} 