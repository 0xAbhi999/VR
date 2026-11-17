# Milestone 2 — Unity setup and test guide

## What is in this repository

This repository did not contain a Unity project when work began. It now has a Unity 6.0 project shell, a Unity editor scene generator, and runtime scripts. The generated scenes and prefab are created by the editor after packages finish importing; they are not checked into the repository until that menu action has run.

The selected editor baseline is Unity `6000.0.44f1`. The manifest pins URP `17.0.3`, XR Plug-in Management `4.5.3`, OpenXR `1.14.3`, and Unity OpenXR Meta `2.4.0`. Unity's package documentation lists XR Interaction Toolkit `3.0.9` and Unity OpenXR Meta `2.4.0` as released for Unity 6000.0; this foundation does not require XRI and therefore does not add it. Package versions are pinned so the project imports consistently.

## Open and generate

1. In Unity Hub, install Unity `6000.0.44f1` with **Android Build Support**, **Android SDK & NDK Tools**, and **OpenJDK** modules.
2. In Unity Hub choose **Add > Add project from disk**, then select this repository folder. Wait until the Package Manager finishes resolving packages and the Console has no compile errors.
3. In the editor choose **ONE WORLD > Generate Foundation Scenes**. The generator stops without overwriting if any of its target scenes or the portal prefab already exists.
4. The editor creates `Assets/ONEWORLD/Scenes/Welcome.unity`, `ExhibitionHub.unity`, five `World_*.unity` placeholders, and `Assets/ONEWORLD/Prefabs/WorldPortal.prefab`; it adds all seven scenes to Build Settings and opens Welcome.
5. Press Play in the editor. Without an XR runtime, the generated camera remains at its default standing viewpoint. This checks scene wiring only; it is not a VR input test.

## Configure Quest 3 XR (manual)

1. Open **File > Build Profiles** (or **Build Settings** in the editor version) and switch the active platform to **Android**.
2. Open **Edit > Project Settings > XR Plug-in Management** and install it if prompted. Under Android, enable **OpenXR**. Enable **Initialize XR on Startup**.
3. Under **XR Plug-in Management > OpenXR > Android**, enable the **Meta Quest Support** feature from Unity OpenXR Meta and add the **Oculus Touch Controller Profile**. If this editor shows a Quest-specific feature group, enable its required dependencies as indicated by the validation panel.
4. Resolve every OpenXR validation warning shown for Android. Keep a single OpenXR provider enabled.
5. In **Player Settings > Android**, set a unique package name, **ARM64**, and **IL2CPP**. Accept the platform package installation prompts and let Unity finish.
6. Enable Developer Mode and USB debugging on Quest 3, connect it to the Mac, and accept the headset's USB debugging prompt.

## Controls implemented

- Aim the right controller ray at one of the five portal frames and press the right trigger to load its placeholder scene.
- In each placeholder, aim at **RETURN TO HUB** and press the right trigger.
- Aim at the floor and press the right trigger to teleport.
- Push the right thumbstick left/right past its threshold for a 30-degree snap turn.
- Smooth movement is off by default. To try it, enable **Allow Smooth Move** on `ONE WORLD XR Player` in the scene Inspector; left thumbstick then moves at a gentle walking speed.

## Build and test on Quest 3

1. Confirm Welcome is the first enabled scene in the Build Settings list.
2. Use **Build And Run** for Android and choose a writable APK location outside `Library/`.
3. In-headset verify: headset tracking; right controller ray and trigger; welcome entry; all five individually colored and labeled portals; teleport on the floor; snap turning; each placeholder loads; return-to-hub works; no missing scene errors or black screens.
4. Check comfort and readability while standing and seated. Keep snap turns and teleportation available; do not enable smooth move by default.
5. For performance checks, use Unity Profiler / Android Logcat on device and record sustained frame timing, thermal behavior, and any dropped frames in the intended venue conditions.

## Current limitations

- Unity Editor, Android SDK/NDK, `adb`, and Quest 3 are not available to this coding workspace, so editor compilation, generated-scene review, APK build, and headset testing have not been run.
- `ProjectSettings/` has no XR Plug-in Management or OpenXR Android settings yet. They are editor/device-specific configuration and must be completed as above.
- The scene generator creates lightweight primitive placeholder rooms only. This milestone does not create detailed exhibition environments.
- Input is through Unity's legacy `UnityEngine.XR.InputDevice` API via OpenXR. Controller trigger feature reporting and tracking must be verified on device; if the runtime does not expose it, switch to an Input System action-based rig after confirming the exact package setup.
- The transition overlay uses Unity's immediate-mode GUI and should be checked in stereo on Quest. Replace with a world-space/XR-safe fade if it does not render correctly in the headset.

