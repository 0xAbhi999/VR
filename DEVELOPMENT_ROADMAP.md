# ONE WORLD Development Roadmap

## Environment inspection (current)

- Workspace is an empty Git repository on branch `main`; no Unity project, scenes, scripts, packages, or prior commits were present at inspection.
- `git` is available. Unity, .NET, and `adb` were not found on the current command PATH. This does not prove they are absent from the computer; verify the Unity Hub/editor installation and Android modules with the owner before creating a project.
- No Quest device or installed package state has been verified.

## Milestones

1. **Environment and project setup:** verify Unity Hub and Unity 6 editor version, Android Build Support/OpenJDK/SDK/NDK modules, Quest developer mode/USB debugging, and source-control setup. Create a URP project and record the exact package versions and baseline settings.
2. **First working VR foundation:** create Welcome, Hub, and a temporary FoundationLab; configure OpenXR and one compatible Meta integration route; add the shared XR player; prove comfortable movement, one interactable, one information panel, and hub return. Build and run on Quest 3.
3. **Foundation performance gate:** check comfort, input, visual readability, scene transitions, Android build, and on-device performance. Fix issues and document the tested device/build settings. No detailed world production before this gate passes.
4. **Reusable exhibition framework:** polish common navigation, transitions/loading, information panels, optional narration/captions, and world-entry/return patterns.
5. **World production, one at a time:** implement Agriculture, Indian Army, Viksit Bharat 2047, Futuristic Pune, then Space. Review each against its brief and performance budget before starting the next.
6. **Release preparation:** accessibility and comfort review, Android/Quest build verification, device QA, asset/license review, versioning, and a repeatable release checklist.

## First VR scene implementation plan

Use a small, neutral **FoundationLab** as the first working VR scene. It validates the shared systems without prematurely creating any exhibition world.

1. In a verified Unity 6 URP project, add a Welcome scene, a minimal Hub scene, and FoundationLab. Add them to the build scene list in the intended startup order.
2. Install/configure only the verified OpenXR and Quest-compatible packages needed for Quest 3; capture package versions and XR settings.
3. Add one shared XR rig prefab with tracked head pose and controller input. Add teleport locomotion as the default movement experiment and snap turn as an optional setting; do not force continuous camera movement.
4. In FoundationLab, include a floor, simple bounds, one clearly labeled interactable, and one readable world-space information panel. Use simple geometry and minimal lighting.
5. Add a Welcome-to-Hub action and an obvious Hub return action. Use a brief fade or loading indicator during transitions.
6. Verify in editor using the available XR simulation/device workflow, then make an Android APK and test on the physical Quest 3. Check head tracking, controls, comfort, UI readability, interaction, return navigation, and stability.
7. Record what worked, package versions, build settings, and any issues. Continue to detailed worlds only after the foundation gate passes.

## Acceptance checklist for the first working scene

- App launches into a usable welcome screen and enters the hub.
- Head tracking is correct; the shared player does not move the camera independently of the headset.
- User can navigate comfortably and can reach the interaction and read the panel.
- The interactable gives clear feedback and the information panel is legible at a comfortable distance.
- User can return to the hub without losing control or becoming disoriented.
- Android build installs and runs on Quest 3; issues and measured performance are recorded rather than assumed.

