using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Talsec.freeRASPTestApp
{
    public class AndroidGame : MonoBehaviour, AndroidThreatDetectedCallback
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // common configs
            bool isProd = true;
            string watcherMailAddress = "WATCHER EMAIL ADDRESS";

            // Android related configs
            string expectedPackageName = "com.unity.freeRASP";
            string[] expectedSigningCertificateHashBase64 = new string[] { "Tmac/QIomCqEGS1jYqy9cMMrqaitVoZLpjXzCMnt55Q=" };
            string[] supportedAlternativeStores = new string[] { "com.sec.android.app.samsungapps" };

            // initialize talsec
            TalsecPlugin.Instance.initAndroidTalsec(expectedPackageName, expectedSigningCertificateHashBase64, 
                supportedAlternativeStores, watcherMailAddress, isProd);
            TalsecPlugin.Instance.setAndroidCallback(this); // set Android callback
        }

        // Implementation of IAndroidCallback interface
        public void onRootDetected()
        {
            Debug.Log("Unity - Root detected");
        }

        public void onTamperDetected()
        {
            Debug.Log("Unity - Tamper detected");
        }

        public void onDebuggerDetected()
        {
            Debug.Log("Unity - Debugger detected");
        }

        public void onEmulatorDetected()
        {
            Debug.Log("Unity - Emulator detected");
        }

        public void onObfuscationIssuesDetected()
        {
            Debug.Log("Unity - Obfuscation issues detected");
        }
        public void onScreenshotDetected()
        {
            Debug.Log("Unity - Screenshot detected");
        }

        public void onScreenRecordingDetected()
        {
            Debug.Log("Unity - Screen recording detected");
        }

        public void onUntrustedInstallationSourceDetected() {
            Debug.Log("Unity - Untrusted installation source detected");
        }

        public void onHookDetected() {
            Debug.Log("Unity - Hook detected");
        }

        public void onDeviceBindingDetected() {
            Debug.Log("Unity - Device binding detected");
        }

        public void onMalwareDetected(List<SuspiciousAppInfo> malwareList)
        {
            Debug.Log("Unity - Malware detected " + malwareList);
        }
    }
}