/**
 * 
 * var androidConfig = new AndroidConfig
 * {
 *     packageName = "com.example.app",
 *     signingCertificateHashBase64 = new string[] { "hash1", "hash2" },
 *     supportedAlternativeStores = new string[] { "store1", "store2" }
 * };
 * 
 **/
/// <summary>
/// Struct containing Android-specific parameters for Talsec initialization
/// </summary>
[System.Serializable]
public struct AndroidConfig
{
    public string packageName;
    public string[] signingCertificateHashBase64;
    public string[] supportedAlternativeStores;

    /// <summary>
    /// Constructor for AndroidConfig with required packageName
    /// </summary>
    /// <param name="packageName">Required Android package name</param>
    /// <param name="signingCertificateHashBase64">Optional array of signing certificate hashes</param>
    /// <param name="supportedAlternativeStores">Optional array of supported alternative stores</param>
    public AndroidConfig(string packageName, string[] signingCertificateHashBase64 = null, string[] supportedAlternativeStores = null)
    {
        this.packageName = packageName ?? throw new System.ArgumentNullException(nameof(packageName), "Package name is required");
        this.signingCertificateHashBase64 = signingCertificateHashBase64;
        this.supportedAlternativeStores = supportedAlternativeStores;
    }
}