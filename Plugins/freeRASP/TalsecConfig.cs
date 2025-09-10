using UnityEngine;

/*
 * Example usage of Talsec initialization:
 * 
 * var config = new TalsecConfig
 * {
 *     watcherMailAddress = "security@example.com",
 *     isProd = true,
 *     androidConfig = new AndroidConfig
 *     {
 *         packageName = "com.example.app",
 *         signingCertificateHashBase64 = new string[] { "hash1", "hash2" },
 *         supportedAlternativeStores = new string[] { "store1", "store2" }
 *     },
 *     iosConfig = new IOSConfig
 *     {
 *         appBundleIds = new string[] { "com.example.app" },
 *         appTeamId = "TEAM123"
 *     }
 * };
 * TalsecPlugin.Instance.initTalsec(config);
 */
/// <summary>
/// Main configuration class for Talsec initialization containing all platform-specific and common settings
/// </summary>
[System.Serializable]
public class TalsecConfig
{
    [Header("Common Settings")]
    public string watcherMailAddress;
    public bool isProd;
    
    [Header("Android Settings")]
    public AndroidConfig androidConfig;
    
    [Header("iOS Settings")]
    public IOSConfig iosConfig;
    
    /// <summary>
    /// Constructor with sensible defaults
    /// </summary>
    public TalsecConfig()
    {
        isProd = true;
        watcherMailAddress = null;
        androidConfig = new AndroidConfig();
        iosConfig = new IOSConfig();
    }
}