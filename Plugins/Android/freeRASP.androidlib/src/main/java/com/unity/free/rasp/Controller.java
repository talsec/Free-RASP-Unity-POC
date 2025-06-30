package com.unity.free.rasp;

import android.content.Context;
import android.util.Log;

import com.unity3d.player.UnityPlayer;
import com.aheaditec.talsec_security.security.api.SuspiciousAppInfo;
import com.aheaditec.talsec_security.security.api.Talsec;
import com.aheaditec.talsec_security.security.api.TalsecConfig;
import com.aheaditec.talsec_security.security.api.ThreatListener;

import java.util.List;

public class Controller implements ThreatListener.ThreatDetected
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
            ThreatListener threatListener = new ThreatListener(this);
            threatListener.registerListener(context);
            Talsec.start(context, config);
            talSecInitialized = true;
        }
    }

    public void setUnityGameObjectCallback(String gameObjectName) {
        this.gameObjectName = gameObjectName;
    }

    @Override
    public void onRootDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onRootDetected");
    }

    @Override
    public void onTamperDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onTamperDetected");
    }

    @Override
    public void onMalwareDetected(List<SuspiciousAppInfo> list) {
        // not implemented yet
    }

    @Override
    public void onDebuggerDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onDebuggerDetected");
    }

    @Override
    public void onEmulatorDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onEmulatorDetected");
    }

    @Override
    public void onUntrustedInstallationSourceDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onUntrustedInstallationSourceDetected");
    }

    @Override
    public void onHookDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onHookDetected");
    }

    @Override
    public void onDeviceBindingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onDeviceBindingDetected");
    }

    @Override
    public void onObfuscationIssuesDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onObfuscationIssuesDetected");
    }

    @Override
    public void onScreenshotDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onScreenshotDetected");
    }

    @Override
    public void onScreenRecordingDetected() {
        UnityPlayer.UnitySendMessage(this.gameObjectName, "scanResultAndroid", "onScreenRecordingDetected");
    }
}