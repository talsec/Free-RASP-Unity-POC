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

    readonly string ControllerName = "com.unity.free.rasp.Controller";
    
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

            if(threatType == "signature") {
                this.iosCallback.signatureDetected();
            }
            if(threatType == "jailbreak") {
                this.iosCallback.jailbreakDetected();
            }
            if(threatType == "debugger") {
                this.iosCallback.debuggerDetected();
            }
            if(threatType == "runtimeManipulation") {
                this.iosCallback.runtimeManipulationDetected();
            }
            if(threatType == "passcode") {
                this.iosCallback.passcodeDetected();
            }
            if(threatType == "passcodeChange") {
                this.iosCallback.passcodeChangeDetected();
            }
            if(threatType == "simulator") {
                this.iosCallback.simulatorDetected();
            }
            if(threatType == "missingSecureEnclave") {
                this.iosCallback.missingSecureEnclaveDetected();
            }
            if(threatType == "deviceChange") {
                this.iosCallback.deviceBindingDetected();
            }
            if(threatType == "deviceID") {
                this.iosCallback.deviceIDDetected();
            }
            if(threatType == "unofficialStore") {
                this.iosCallback.unofficialStoreDetected();
            }
            if(threatType == "systemVPN") {
                this.iosCallback.systemVPNDetected();
            }
            if(threatType == "screenshot") {
                this.iosCallback.screenshotDetected();
            }
            if(threatType == "screenRecording") {
                this.iosCallback.screenRecordingDetected();
            }
        }
        
    }

    // this method is called by the java side module
    public void scanResultAndroid(string talsecScanResultCallbackName) {
        // Debug.Log("Scan Result Callback Name: " + talsecScanResultCallbackName);
        if (Application.platform == RuntimePlatform.Android)
        {
            if(talsecScanResultCallbackName == "onRootDetected") {
                this.androidCallback.onRootDetected();
            }
            if(talsecScanResultCallbackName == "onTamperDetected") {
                this.androidCallback.onTamperDetected();
            }
            if(talsecScanResultCallbackName == "onDebuggerDetected") {
                this.androidCallback.onDebuggerDetected();
            }
            if(talsecScanResultCallbackName == "onEmulatorDetected") {
                this.androidCallback.onEmulatorDetected();
            }
            if(talsecScanResultCallbackName == "onObfuscationIssuesDetected") {
                this.androidCallback.onObfuscationIssuesDetected();
            }
            if(talsecScanResultCallbackName == "onScreenshotDetected") {
                this.androidCallback.onScreenshotDetected();
            }
            if(talsecScanResultCallbackName == "onScreenRecordingDetected") {
                this.androidCallback.onScreenRecordingDetected();
            }
            if(talsecScanResultCallbackName == "onUntrustedInstallationSourceDetected") {
                this.androidCallback.onUntrustedInstallationSourceDetected();
            }
        }
    }

    // this method is called by the java side module
    public void onMalwareDetected(string result) {
        // Debug.Log("C# onMalwareDetected " + result);
        List<SuspiciousAppInfo> malwareList = JsonUtility.FromJson<SuspiciousAppInfoList>(
            "{ \"items\": " + result + "}"
        ).items;
        this.androidCallback.onMalwareDetected(malwareList);
    }
}