/*
 *
 * var iosConfig = new IOSConfig
 * {
 *     appBundleIds = new string[] { "com.example.app" },
 *     appTeamId = "TEAM123"
 * };
 *
 * 
 **/
/// <summary>
/// Struct containing iOS-specific parameters for Talsec initialization
/// </summary>
[System.Serializable]
public struct IOSConfig
{
    public string[] appBundleIds;
    public string appTeamId;
    
    /// <summary>
    /// Creates an iOS configuration with bundle IDs and team ID.
    /// </summary>
    /// <param name="bundleIds">Required iOS app bundle identifiers</param>
    /// <param name="teamId">Apple Developer Team ID</param>
    public IOSConfig(string[] bundleIds, string teamId)
    {
        if (bundleIds == null || bundleIds.Length == 0)
            throw new System.ArgumentException("At least one bundle ID is required");
        
        appBundleIds = bundleIds;
        appTeamId = teamId;
    }
}