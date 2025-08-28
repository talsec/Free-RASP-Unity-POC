using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour, ThreatDetectedCallback
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // common configs
        bool isProd = true;
        string watcherMailAddress = "security@example.com";

        // Android related configs
        string expectedPackageName = "com.unity.freeRASP";
        string[] expectedSigningCertificateHashBase64 = new string[] { "Tmac/QIomCqEGS1jYqy9cMMrqaitVoZLpjXzCMnt55Q=" };
        string[] blacklistedPackageNames = new string[] { "com.spotify.music", "com.leavjenn.hews2" };
        string[] supportedAlternativeStores = new string[] { "com.sec.android.app.samsungapps" };

        // iOS related configs
        string[] appBundleIds = new string[] { "com.unity.freeRASP" };
        string teamId = "TEAM ID";

        
        var androidConfig = new AndroidConfig
        {
            packageName = expectedPackageName,
            signingCertificateHashBase64 = expectedSigningCertificateHashBase64,
            supportedAlternativeStores = supportedAlternativeStores
        };
        
        // Create iOS config struct
        var iosConfig = new IOSConfig
        {
            appBundleIds = appBundleIds,
            appTeamId = teamId
        };

        var commonConfig = new CommonConfig
        {
            watcherMailAddress = watcherMailAddress,
            isProd = isProd
        };
        
        // set callback
        TalsecPlugin.Instance.setThreatDetectedCallback(this); 
        // initialize talsec
        TalsecPlugin.Instance.initTalsec(androidConfig, iosConfig, commonConfig);
    }

    // Implementation of ThreatDetectedCallback interface
    public void onPrivilegedAccess()
    {
        Debug.Log("Unity - Root detected");
    }

    public void onAppIntegrity()
    {
        Debug.Log("Unity - Tamper detected");
    }

    public void onDebug()
    {
        Debug.Log("Unity - Debugger detected");
    }

    public void onSimulator()
    {
        Debug.Log("Unity - Emulator detected");
    }

    public void onObfuscationIssues()
    {
        Debug.Log("Unity - Obfuscation issues detected");
    }
    public void onScreenshot()
    {
        Debug.Log("Unity - Screenshot detected");
    }

    public void onScreenRecording()
    {
        Debug.Log("Unity - Screen recording detected");
    }

    public void onUnofficialStore() {
        Debug.Log("Unity - Untrusted installation source detected");
    }

    public void onHook() {
        Debug.Log("Unity - Hook detected");
    }

    public void onDeviceBinding() {
        Debug.Log("Unity - Device binding detected");
    }

    public void onPasscode() {
        Debug.Log("Unity - Unlocked device detected");
    }

    public void onPasscodeChange() {
        Debug.Log("Unity - Passcode change detected");
    }

    public void onDeviceID() {
        Debug.Log("Unity - Device ID detected");
    }

    public void onSecureHardwareNotAvailable() {
        Debug.Log("Unity - Hardware backed keystore not available detected");
    }

    public void onDevMode() {
        Debug.Log("Unity - Developer mode detected");
    }

    public void onADBEnabled() {
        Debug.Log("Unity - ADB enabled detected");
    }

    public void onSystemVPN() {
        Debug.Log("Unity - System VPN detected");
    }

}