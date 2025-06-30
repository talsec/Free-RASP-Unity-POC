using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Talsec.freeRASPTestApp
{
    public class IOSGame : MonoBehaviour, IOSThreatDetectedCallback
    {
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            // common configs
            bool isProd = true;
            string watcherMailAddress = "WATCHER EMAIL ADDRESS";

            // iOS related configs
            string[] appBundleIds = new string[] { "com.unity.freeRASP" };
            string teamId = "TEAM ID";

            // initialize talsec
            TalsecPlugin.Instance.setiOSCallback(this); // set callback
            TalsecPlugin.Instance.initiOSTalsec(appBundleIds, teamId, watcherMailAddress, isProd);
        }

        // Implementation of IOSThreatDetectedCallback interface
        public void signatureDetected() {
            Debug.Log("Unity - Signature detected");
        }

        public void jailbreakDetected() {
            Debug.Log("Unity - Jailbreak detected");
        }

        public void debuggerDetected() {
            Debug.Log("Unity - Debugger detected");
        }

        public void runtimeManipulationDetected() {
            Debug.Log("Unity - Runtime manipulation detected");
        }

        public void passcodeDetected() {
            Debug.Log("Unity - Passcode detected");
        }

        public void passcodeChangeDetected() {
            Debug.Log("Unity - Passcode change detected");
        }

        public void simulatorDetected() {
            Debug.Log("Unity - Simulator detected");
        }

        public void missingSecureEnclaveDetected() {
            Debug.Log("Unity - Missing secure enclave detected");
        }

        public void deviceBindingDetected() {
            Debug.Log("Unity - Device binding detected");
        }

        public void unofficialStoreDetected() {
            Debug.Log("Unity - Unofficial store detected");
        }

        public void systemVPNDetected() {
            Debug.Log("Unity - System VPN detected");
        }

        public void screenshotDetected() {
            Debug.Log("Unity - Screenshot detected");
        }

        public void screenRecordingDetected() {
            Debug.Log("Unity - Screen recording detected");
        }

        public void deviceIDDetected() {
            Debug.Log("Unity - Device ID detected");
        }   
    }
}



