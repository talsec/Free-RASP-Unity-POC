using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

/*
 * Example usage of the new struct-based Talsec initialization:
 * 
 * // For Android:
 * var androidConfig = new AndroidConfig
 * {
 *     packageName = "com.example.app",
 *     signingCertificateHashBase64 = new string[] { "hash1", "hash2" },
 *     supportedAlternativeStores = new string[] { "store1", "store2" }
 * };
 * var commonConfig = new CommonConfig
 * {
 *     watcherMailAddress = "security@example.com",
 *     isProd = true
 * };
 * TalsecPlugin.Instance.initTalsec(androidConfig, null, commonConfig);
 * 
 * // Or use the convenience method:
 * TalsecPlugin.Instance.initTalsecAndroid(androidConfig, "security@example.com", true);
 * 
 * // For iOS:
 * var iosConfig = new IOSConfig
 * {
 *     appBundleIds = new string[] { "com.example.app" },
 *     appTeamId = "TEAM123"
 * };
 * TalsecPlugin.Instance.initTalseciOS(iosConfig, "security@example.com", true);
 */

/// <summary>
/// Struct containing Android-specific parameters for Talsec initialization
/// </summary>
[System.Serializable]
public struct AndroidConfig
{
    public string packageName;
    public string[] signingCertificateHashBase64;
    public string[] supportedAlternativeStores;
}

/// <summary>
/// Struct containing iOS-specific parameters for Talsec initialization
/// </summary>
[System.Serializable]
public struct IOSConfig
{
    public string[] appBundleIds;
    public string appTeamId;
}

/// <summary>
/// Struct containing common parameters for both platforms
/// </summary>
[System.Serializable]
public struct CommonConfig
{
    public string watcherMailAddress;
    public bool isProd;
}

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

/// <summary>
/// Unity plugin for Talsec freeRASP security SDK, providing runtime application self-protection
/// against various threats on Android and iOS platforms.
/// </summary>
public class TalsecPlugin : MonoBehaviour
{
    #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _initTalsec(string[] appBundleIds, int appBundleIdsCount, string appTeamId, string watcherMailAddress, bool isProd);
    #endif

    private readonly string ControllerName = "com.unity.free.rasp.Controller";
    
    // Singleton instance
    private static TalsecPlugin _instance;
    private ThreatDetectedCallback threatDetectedCallback;
    private AndroidJavaObject javaControllerObject;

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

    /// <summary>
    /// Single point of entry for initializing Talsec SDK on both iOS and Android platforms
    /// </summary>
    /// <param name="androidConfig">Android-specific configuration (required for Android)</param>
    /// <param name="iosConfig">iOS-specific configuration (required for iOS)</param>
    /// <param name="commonConfig">Common configuration for both platforms</param>
    public void initTalsec(AndroidConfig? androidConfig = null, IOSConfig? iosConfig = null, 
                          CommonConfig commonConfig = default)
    {
        RuntimePlatform currentPlatform = Application.platform;
        
        if (currentPlatform == RuntimePlatform.Android)
        {
            // Validate Android-specific parameters
            var android = androidConfig.Value;
            initAndroidTalsec(android.packageName, android.signingCertificateHashBase64, android.supportedAlternativeStores, 
                             commonConfig.watcherMailAddress, commonConfig.isProd);
        }
        else if (currentPlatform == RuntimePlatform.IPhonePlayer)
        {
            // Validate iOS-specific parameters
            var ios = iosConfig.Value;
            initiOSTalsec(ios.appBundleIds, ios.appTeamId, commonConfig.watcherMailAddress, commonConfig.isProd);
        }
        else
        {
            Debug.LogWarning($"Talsec initialization skipped: Platform {currentPlatform} is not supported. Only Android and iOS are supported.");
        }
    }

    public void stopTalsec() {
        RuntimePlatform currentPlatform = Application.platform;
        
        if (currentPlatform == RuntimePlatform.Android)
        {
            if (javaControllerObject != null)
            {
                javaControllerObject.Call("stopTalsec");
            }
        }
    }

    /// <summary>
    /// Gets the current platform as a string for logging purposes
    /// </summary>
    /// <returns>Current platform name</returns>
    private string GetCurrentPlatformName()
    {
        return Application.platform.ToString();
    }

    private void initAndroidTalsec(string packageName, string [] signingCertificateHashBase64, string [] supportedAlternativeStores,  string watcherMailAddress, bool isProd)
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            // Get the current activity
            using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            {
                AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
                javaControllerObject = new AndroidJavaObject(ControllerName);

                // Register this GameObject to receive callbacks
                // We pass the name of this GameObject and the callback method names
                javaControllerObject.Call("setUnityGameObjectCallback", gameObject.name);

                // Call a method with parameters
                javaControllerObject.Call("initializeTalsec", currentActivity, packageName, signingCertificateHashBase64, supportedAlternativeStores, watcherMailAddress, isProd);
            }
        }
    }

    private void initiOSTalsec(string[] appBundleIds, string appTeamId, string watcherMailAddress, bool isProd)
    {
        #if UNITY_IOS && !UNITY_EDITOR
            _initTalsec(appBundleIds, appBundleIds.Length, appTeamId, watcherMailAddress, isProd);
            Debug.Log("Talsec initalized on iOS");
        #endif
    }

    public void setThreatDetectedCallback(ThreatDetectedCallback callback) {
        this.threatDetectedCallback = callback;
    }

    // This method will be called from native iOS
    private void scanResultIOS(string talsecScanResultCallback) 
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer) {
            if(this.threatDetectedCallback != null) {
                switch(talsecScanResultCallback) {
                    case "onAppIntegrity":
                        this.threatDetectedCallback.onAppIntegrity();
                        break;
                    case "onPrivilegedAccess":
                        this.threatDetectedCallback.onPrivilegedAccess();
                        break;
                    case "onDebug":
                        this.threatDetectedCallback.onDebug();
                        break;
                    case "onRuntimeManipulation":
                        this.threatDetectedCallback.onHook();
                        break;
                    case "onPasscode":
                        this.threatDetectedCallback.onPasscode();
                        break;
                    case "onPasscodeChange":
                        this.threatDetectedCallback.onPasscodeChange();
                        break;
                    case "onSimulator":
                        this.threatDetectedCallback.onSimulator();
                        break;
                    case "onSecureHardwareNotAvailable":
                        this.threatDetectedCallback.onSecureHardwareNotAvailable();
                        break;
                    case "onDeviceBinding":
                        this.threatDetectedCallback.onDeviceBinding();
                        break;
                    case "onDeviceID":
                        this.threatDetectedCallback.onDeviceID();
                        break;
                    case "onUnofficialStore":
                        this.threatDetectedCallback.onUnofficialStore();
                        break;
                    case "onSystemVPN":
                        this.threatDetectedCallback.onSystemVPN();
                        break;
                    case "onScreenshot":
                        this.threatDetectedCallback.onScreenshot();
                        break;
                    case "onScreenRecording":
                        this.threatDetectedCallback.onScreenRecording();
                        break;
                }
            }
        }
    }

    // this method is called by the Android Java module
    private void scanResultAndroid(string talsecScanResultCallback) {
        if (Application.platform == RuntimePlatform.Android)
        {
            if(this.threatDetectedCallback != null) {
                switch(talsecScanResultCallback) {
                    case "onPrivilegedAccess":
                        this.threatDetectedCallback.onPrivilegedAccess();
                        break;
                    case "onAppIntegrity":
                        this.threatDetectedCallback.onAppIntegrity();
                        break;
                    case "onDebug":
                        this.threatDetectedCallback.onDebug();
                        break;
                    case "onSimulator":
                        this.threatDetectedCallback.onSimulator();
                        break;
                    case "onObfuscationIssues":
                        this.threatDetectedCallback.onObfuscationIssues();
                        break;
                    case "onScreenshot":
                        this.threatDetectedCallback.onScreenshot();
                        break;
                    case "onScreenRecording":
                        this.threatDetectedCallback.onScreenRecording();
                        break;
                    case "onUnofficialStore":
                        this.threatDetectedCallback.onUnofficialStore();
                        break;
                    case "onHook":
                        this.threatDetectedCallback.onHook();
                        break;
                    case "onDeviceBinding":
                        this.threatDetectedCallback.onDeviceBinding();
                        break;
                    case "onPasscode":
                        this.threatDetectedCallback.onPasscode();
                        break;
                    case "onSecureHardwareNotAvailable":
                        this.threatDetectedCallback.onSecureHardwareNotAvailable();
                        break;
                    case "onDevMode": 
                        this.threatDetectedCallback.onDevMode();
                        break;
                    case "onADBEnabled":
                        this.threatDetectedCallback.onADBEnabled();
                        break;
                    case "onSystemVPN":
                        this.threatDetectedCallback.onSystemVPN();
                        break;
                }
            }
        }
    }
}