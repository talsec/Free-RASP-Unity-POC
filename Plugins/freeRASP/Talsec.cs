using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

[System.Serializable]
public class SuspiciousAppInfo
{
    public string reason;
    public string packageName;
}

// Helper class for JsonUtility to properly deserialize arrays
[System.Serializable]
public class SuspiciousAppInfoList
{
    public List<SuspiciousAppInfo> items;
}

public class TalsecPlugin : MonoBehaviour
{
    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _initTalsec(string[] appBundleIds, int appBundleIdsCount, string appTeamId, string watcherMailAddress, bool isProd);
    #endif

    private readonly string ControllerName = "com.unity.free.rasp.Controller";
    
    // Singleton instance
    private static TalsecPlugin _instance;
    private AndroidThreatDetectedCallback androidCallback;
    private IOSThreatDetectedCallback iosCallback;
    private AndroidJavaObject javaControllerObject;
    private AndroidJavaObject currentActivity = null;

    // Public accessor for the instance
    public static TalsecPlugin Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("TalsecPlugin");
                _instance = go.AddComponent<TalsecPlugin>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    // Initialize the plugin
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (_instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void initAndroidTalsec(string packageName, string [] signingCertificateHashBase64, string [] supportedAlternativeStores,  string watcherMailAddress, bool isProd)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Get the current activity
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                javaControllerObject = new AndroidJavaObject(ControllerName);

                // Register this GameObject to receive callbacks
                // We pass the name of this GameObject and the callback method names
                javaControllerObject.Call("setUnityGameObjectCallback", gameObject.name);
            }

            // Call a method with parameters
            javaControllerObject.Call("initializeTalsec", currentActivity, packageName, signingCertificateHashBase64, supportedAlternativeStores, watcherMailAddress, isProd);
            Debug.Log("Done initializing Talsec");
        }
    }

    public void initiOSTalsec(string[] appBundleIds, string appTeamId, string watcherMailAddress, bool isProd)
    {
        #if UNITY_IOS && !UNITY_EDITOR
            _initTalsec(appBundleIds, appBundleIds.Length, appTeamId, watcherMailAddress, isProd);
            Debug.Log("Talsec initalized on iOS");
        #else
            Debug.Log("Sorry, This only works on iOS device");
        #endif
    }

    public void setAndroidCallback(AndroidThreatDetectedCallback callback) {
        this.androidCallback = callback;
    }

    public void setiOSCallback(IOSThreatDetectedCallback callback) {
        this.iosCallback = callback;
    }

    // This method will be called from native iOS
    public void scanResultIOS(string threatType) 
    {
        if(this.iosCallback != null) {
            switch(threatType) {
                case "signature":
                    this.iosCallback.signatureDetected();
                    break;
                case "jailbreak":
                    this.iosCallback.jailbreakDetected();
                    break;
                case "debugger":
                    this.iosCallback.debuggerDetected();
                    break;
                case "runtimeManipulation":
                    this.iosCallback.runtimeManipulationDetected();
                    break;
                case "passcode":
                    this.iosCallback.passcodeDetected();
                    break;
                case "passcodeChange":
                    this.iosCallback.passcodeChangeDetected();
                    break;
                case "simulator":
                    this.iosCallback.simulatorDetected();
                    break;
                case "missingSecureEnclave":
                    this.iosCallback.missingSecureEnclaveDetected();
                    break;
                case "deviceChange":
                    this.iosCallback.deviceBindingDetected();
                    break;
                case "deviceID":
                    this.iosCallback.deviceIDDetected();
                    break;
                case "unofficialStore":
                    this.iosCallback.unofficialStoreDetected();
                    break;
                case "systemVPN":
                    this.iosCallback.systemVPNDetected();
                    break;
                case "screenshot":
                    this.iosCallback.screenshotDetected();
                    break;
                case "screenRecording":
                    this.iosCallback.screenRecordingDetected();
                    break;
            }
        }
    }

    // this method is called by the java side module
    public void scanResultAndroid(string talsecScanResultCallbackName) {
        if (Application.platform == RuntimePlatform.Android)
        {
            switch(talsecScanResultCallbackName) {
                case "onRootDetected":
                    this.androidCallback.onRootDetected();
                    break;
                case "onTamperDetected":
                    this.androidCallback.onTamperDetected();
                    break;
                case "onDebuggerDetected":
                    this.androidCallback.onDebuggerDetected();
                    break;
                case "onEmulatorDetected":
                    this.androidCallback.onEmulatorDetected();
                    break;
                case "onObfuscationIssuesDetected":
                    this.androidCallback.onObfuscationIssuesDetected();
                    break;
                case "onScreenshotDetected":
                    this.androidCallback.onScreenshotDetected();
                    break;
                case "onScreenRecordingDetected":
                    this.androidCallback.onScreenRecordingDetected();
                    break;
                case "onUntrustedInstallationSourceDetected":
                    this.androidCallback.onUntrustedInstallationSourceDetected();
                    break;
                case "onHookDetected":
                    this.androidCallback.onHookDetected();
                    break;
                case "onDeviceBindingDetected":
                    this.androidCallback.onDeviceBindingDetected();
                    break;
                case "onUnlockedDeviceDetected":
                    this.androidCallback.onUnlockedDeviceDetected();
                    break;
                case "onHardwareBackedKeystoreNotAvailableDetected":
                    this.androidCallback.onHardwareBackedKeystoreNotAvailableDetected();
                    break;
                case "onDeveloperModeDetected": 
                    this.androidCallback.onDeveloperModeDetected();
                    break;
                case "onADBEnabledDetected":
                    this.androidCallback.onADBEnabledDetected();
                    break;
                case "onSystemVPNDetected":
                    this.androidCallback.onSystemVPNDetected();
                    break;
            }
        }
    }
}