package com.unity.free.rasp;

import android.content.Context;

import com.unity3d.player.UnityPlayer;
import com.aheaditec.talsec_security.security.api.SuspiciousAppInfo;
import com.aheaditec.talsec_security.security.api.Talsec;
import com.aheaditec.talsec_security.security.api.TalsecConfig;
import com.aheaditec.talsec_security.security.api.TalsecMode;
import com.aheaditec.talsec_security.security.api.ThreatListener;

import java.util.List;

public class Controller implements ThreatListener.ThreatDetected, ThreatListener.DeviceState
{
    private static final String TAG = Controller.class.getSimpleName();

    public static class AppRaspExecutionState extends ThreatListener.RaspExecutionState {
        private String gameObjectName;
        public void setGameObjectCallback(String gameObjectName) {
            this.gameObjectName = gameObjectName;
        }
        @Override
        public void onAllChecksFinished() {
            UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onAllChecksFinished");
        }
    }

    private boolean talSecInitialized;
    private String gameObjectName;
    private AppRaspExecutionState appRaspExecutionState;

    public Controller() {
        talSecInitialized = false;
        appRaspExecutionState = new AppRaspExecutionState();
    }

    public void initializeTalsec(Context context, String packageName,
                                   String [] signingCertificateBase64Hash,
                                   String [] supportedAlternativeStores,
                                   String watcherEmailAddress, boolean isProd) {
        if(!talSecInitialized) {
            TalsecConfig config = new TalsecConfig.Builder(packageName,
                    signingCertificateBase64Hash)
                    .supportedAlternativeStores(supportedAlternativeStores)
                    .watcherMail(watcherEmailAddress)
                    .prod(isProd)
                    .build();
            ThreatListener threatListener = new ThreatListener(this, this, appRaspExecutionState);
            threatListener.registerListener(context);
            Talsec.start(context, config, TalsecMode.BACKGROUND);
            talSecInitialized = true;
        }
    }

    public void stopTalsec() {
        if(talSecInitialized) {
            Talsec.stop();
            talSecInitialized = false;
        }
    }

    public void setUnityGameObjectCallback(String gameObjectName) {
        this.gameObjectName = gameObjectName;
        this.appRaspExecutionState.setGameObjectCallback(this.gameObjectName);
    }

    @Override
    public void onRootDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onPrivilegedAccess");
    }

    @Override
    public void onTamperDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onAppIntegrity");
    }

    @Override
    public void onMalwareDetected(List<SuspiciousAppInfo> list) {
        // not implemented yet
    }

    @Override
    public void onDebuggerDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onDebug");
    }

    @Override
    public void onEmulatorDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onSimulator");
    }

    @Override
    public void onUntrustedInstallationSourceDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onUnofficialStore");
    }

    @Override
    public void onHookDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onRuntimeManipulation");
    }

    @Override
    public void onDeviceBindingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onDeviceBinding");
    }

    @Override
    public void onObfuscationIssuesDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onObfuscationIssues");
    }

    @Override
    public void onScreenshotDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onScreenshot");
    }

    @Override
    public void onScreenRecordingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onScreenRecording");
    }

    @Override
    public void onUnlockedDeviceDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onPasscode");
    }

    @Override
    public void onHardwareBackedKeystoreNotAvailableDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onSecureHardwareNotAvailable");
    }

    @Override
    public void onDeveloperModeDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onDevMode");
    }

    @Override
    public void onADBEnabledDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onADBEnabled");
    }

    @Override
    public void onSystemVPNDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onSystemVPN");
    } 
    
    @Override
    public void onMultiInstanceDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onMultiInstance");
    }

    @Override
    public void onUnsecureWifiDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onUnsecureWiFi");
    }

    @Override
    public void onTimeSpoofingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onTimeSpoofing");
    }

    @Override
    public void onLocationSpoofingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResult", "onLocationSpoofing");
    }
}