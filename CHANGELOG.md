# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [0.2.4] - 2026-04-06

Android SDK version: 17.0.1
iOS SDK version: 6.14.2

### Unity

#### Added

- Added `onTimeSpoofing` callback to `ThreatDetectedCallback` for handling device time tampering.
- Added `onLocationSpoofing` callback to `ThreatDetectedCallback` for handling location spoofing (Android only).
- Added `onUnsecureWiFi` callback to `ThreatDetectedCallback` for detecting connections to unsecured Wi-Fi networks (Android only).
- Added `onMultiInstance` callback to `ThreatDetectedCallback` for detecting if the application is running in multi-instancing environments (Android only).
- Added `onAllChecksFinished` callback to `RASPStatusCallback` to notify when all security checks have been completed.
- Added `onDevMode` and `onADBEnabled` callbacks to `ThreatDetectedCallback` (Android only).

#### Changed

- Consolidated threat callbacks into a single interface for better consistency across platforms.

### Android

#### Added

- Added support for 16 KB memory page sizes.
- Capability to detect if another app has `REQUEST_INSTALL_PACKAGES` enabled.
- Added matched permissions to `SuspiciousAppInfo` object for malware detection.

#### Changed

- **Breaking Change**: Upgraded freeRASP Android SDK from 15.1.0 to 17.0.1.
- Refactored internal architecture to use Coroutines for better threading management.
- Improved root and emulator detection (including ADB service running as root).
- Update of internal security libraries and dependencies.

#### Fixed

- Fixed ANR issues related to screen capture registration.
- Fixed Keystore-related crashes on Android 7.
- Fixed `DeadApplicationException` and `TimeoutException` in various edge cases.
- Resolved false positives in root and multi-instance detection.
- Fixed issue with late initializers and `TalsecMode` coroutines scopes.

#### Removed

- Removed deprecated `Pbkdf2Native` functionality and related native libraries (`libpbkdf2_native.so`, `libpolarssl.so`).

### iOS

#### Added

- Added `timeSpoofing` detection for identifying inaccurate device clocks.
- Added `onAllChecksFinished()` method to `RaspExecutionState` (notified after all security checks are completed).

#### Changed

- **Breaking Change**: Upgraded freeRASP iOS SDK from 6.12.1 to 6.14.2.
- Improved `timeSpoofing` and jailbreak detection methods (v6.13.4, v6.14.1).
- Updated internal dependencies.

#### Fixed

- Fixed jailbreak detection false positives on iOS 14 and 13 (v6.14.2).

---

## [0.2.3] - 2025-10-20

Android SDK version: 15.1.0
iOS SDK version: 6.12.1

### Unity

#### Fixed
- Fixed ProGuard rules for release builds.

### Android

#### Added
- Added `externalId` to logs for custom integrator-specified identifiers.
- Added `eventId` to logs for unique log traceability.

#### Changed
- Upgraded freeRASP Android SDK from 15.0.0 to 15.1.0.
- New root detection checks added.

#### Fixed
- Resolved `SecurityException` caused by `getNetworkCapabilities()` (Android 11 specific).

### iOS

#### Fixed

- Resolved memory-related stability issues (v6.12.1).
