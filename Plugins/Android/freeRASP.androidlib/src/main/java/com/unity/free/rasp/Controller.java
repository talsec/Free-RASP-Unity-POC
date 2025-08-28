package com.unity.free.rasp;

import android.content.Context;
import android.util.Log;

import com.unity3d.player.UnityPlayer;
import com.aheaditec.talsec_security.security.api.SuspiciousAppInfo;
import com.aheaditec.talsec_security.security.api.Talsec;
import com.aheaditec.talsec_security.security.api.TalsecConfig;
import com.aheaditec.talsec_security.security.api.ThreatListener;

import java.util.List;

public class Controller implements ThreatListener.ThreatDetected, ThreatListener.DeviceState
{
    private static final String TAG = Controller.class.getSimpleName();
    private boolean talSecInitialized;
    private String gameObjectName;
    public Controller() {
        talSecInitialized = false;
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
            ThreatListener threatListener = new ThreatListener(this, this);
            threatListener.registerListener(context);
            Talsec.start(context, config);
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
    }

    @Override
    public void onRootDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onPrivilegedAccess");
    }

    @Override
    public void onTamperDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onAppIntegrity");
    }

    @Override
    public void onMalwareDetected(List<SuspiciousAppInfo> list) {
        // not implemented yet
    }

    @Override
    public void onDebuggerDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onDebug");
    }

    @Override
    public void onEmulatorDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onSimulator");
    }

    @Override
    public void onUntrustedInstallationSourceDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onUnofficialStore");
    }

    @Override
    public void onHookDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onHook");
    }

    @Override
    public void onDeviceBindingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onDeviceBinding");
    }

    @Override
    public void onObfuscationIssuesDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onObfuscationIssues");
    }

    @Override
    public void onScreenshotDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onScreenshot");
    }

    @Override
    public void onScreenRecordingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onScreenRecording");
    }

    @Override
    public void onUnlockedDeviceDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onPasscode");
    }

    @Override
    public void onHardwareBackedKeystoreNotAvailableDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onSecureHardwareNotAvailable");
    }

    @Override
    public void onDeveloperModeDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onDevMode");
    }

    @Override
    public void onADBEnabledDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onADBEnabled");
    }

    @Override
    public void onSystemVPNDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onSystemVPN");
    }       
}