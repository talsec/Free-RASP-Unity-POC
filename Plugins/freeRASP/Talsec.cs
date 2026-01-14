using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
    /// Single point of entry for initializing Talsec SDK using TalsecConfig
    /// </summary>
    /// <param name="config">Complete Talsec configuration containing all platform-specific and common settings</param>
    public void initTalsec(TalsecConfig config)
    {
        if (config == null)
        {
            throw new System.ArgumentNullException("config", "TalsecConfig cannot be null");
        }
        
        RuntimePlatform currentPlatform = Application.platform;
        
        if (currentPlatform == RuntimePlatform.Android)
        {
            // this will throw an exception if the configuration is not valid
            initAndroidTalsec(config.androidConfig, config.watcherMailAddress, config.isProd);
            return;
        }
        else if (currentPlatform == RuntimePlatform.IPhonePlayer)
        {
            // this will throw an exception if the configuration is not valid
            initiOSTalsec(config.iosConfig, config.watcherMailAddress, config.isProd);
            return;
        }
        Debug.LogWarning($"Talsec initialization skipped: Platform {currentPlatform} is not supported. Only Android and iOS are supported.");
        
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

    private void initAndroidTalsec(AndroidConfig androidConfig, string watcherMailAddress, bool isProd)
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
            javaControllerObject.Call("initializeTalsec", currentActivity, androidConfig.packageName, androidConfig.signingCertificateHashBase64, androidConfig.supportedAlternativeStores, watcherMailAddress, isProd);
        }
    }

    private void initiOSTalsec(IOSConfig iosConfig, string watcherMailAddress, bool isProd)
    {
        #if UNITY_IOS && !UNITY_EDITOR // without this there will be compilation error
            _initTalsec(iosConfig.appBundleIds, iosConfig.appBundleIds.Length, iosConfig.appTeamId, watcherMailAddress, isProd);
        #endif
    }

    public void setThreatDetectedCallback(ThreatDetectedCallback callback) {
        this.threatDetectedCallback = callback;
    }

    // This method will be called from the native side of the code
    // both iOS & Android will use this method
    // hence all the threat types for both platforms are handled here
    private void scanResult(string talsecScanResultCallback) 
    {
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
                    case "onObfuscationIssues":
                        this.threatDetectedCallback.onObfuscationIssues();
                        break;
                    case "onRuntimeManipulation":
                        this.threatDetectedCallback.onHooks();
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
                    case "onDevMode": 
                        this.threatDetectedCallback.onDevMode();
                        break;
                    case "onADBEnabled":
                        this.threatDetectedCallback.onADBEnabled();
                        break;
                    case "onMultiInstance":
                        this.threatDetectedCallback.onMultiInstance();
                        break;
                    case "onUnsecureWiFi":
                        this.threatDetectedCallback.onUnsecureWiFi();
                        break;
                    case "onTimeSpoofing":
                        this.threatDetectedCallback.onTimeSpoofing();
                        break;
                    case "onLocationSpoofing":
                        this.threatDetectedCallback.onLocationSpoofing();
                        break;
                }
            }
    }
}