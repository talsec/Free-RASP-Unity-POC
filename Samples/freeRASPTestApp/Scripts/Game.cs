using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Game : MonoBehaviour, ThreatDetectedCallback
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Create unified TalsecConfig with all settings
        var config = new TalsecConfig
        {
            watcherMailAddress = "security@example.com",
            isProd = true,
            androidConfig = new AndroidConfig
            {
                packageName = "com.unity.freeRASP",
                signingCertificateHashBase64 = new string[] { "Tmac/QIomCqEGS1jYqy9cMMrqaitVoZLpjXzCMnt55Q=" },
                supportedAlternativeStores = new string[] { "com.sec.android.app.samsungapps" }
            }
        };
        
        // set callback
        TalsecPlugin.Instance.setThreatDetectedCallback(this); 
        // initialize talsec with new unified config
        TalsecPlugin.Instance.initTalsec(config);
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

    public void onHooks() {
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

    public void onMultiInstance() {
        Debug.Log("Unity - Multi instance detected");
    }

    public void onUnsecureWiFi() {
        Debug.Log("Unity - Unsecure WiFi detected");
    }

    public void onTimeSpoofing() {
        Debug.Log("Unity - Time spoofing detected");
    }

    public void onLocationSpoofing() {
        Debug.Log("Unity - Location spoofing detected");
    }

}