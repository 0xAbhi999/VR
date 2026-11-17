# ONE WORLD Architecture

## Application flow

`Welcome scene -> Exhibition Hub -> selected world -> Hub`

The welcome experience introduces the exhibition and provides a clear enter action. The hub is the stable navigation center, with five destination points. Each destination loads its own scene and provides an obvious return-to-hub action. Transitions should preserve a shared player experience and avoid abrupt visual changes.

## Runtime responsibilities

- **Bootstrap / app flow:** startup scene, scene transitions, loading feedback, and return routing.
- **Shared XR player:** OpenXR camera rig, input actions, locomotion, interaction ray/direct interaction, and comfort options. Keep this independent of world-specific content.
- **Hub and world scenes:** environment, navigation points, locally authored interactive objects, and world-space information panels.
- **Interaction layer:** small reusable interactable contract and simple feedback; avoid coupling interactions to scene loading internals.
- **Information and narration:** world-space readable panels first; narration and audio are optional and must have captions or a visual equivalent.
- **Content data:** keep titles, descriptions, and optional audio references in data assets when the first scene proves the pattern useful.
- **Performance and build configuration:** Android/Quest settings, render pipeline settings, quality levels, and on-device profiling are explicit release tasks.

## Scene and navigation model

Recommended initial scenes: `Welcome`, `ExhibitionHub`, and a temporary `FoundationLab` for proving interaction and UI. The lab is a test space, not one of the five exhibition worlds. Add the five named destination scenes only after the foundation gate passes.

For the first implementation, use ordinary scene loading with a short fade/loading overlay. Move to asynchronous loading if measured scene load time or memory use warrants it. Avoid building a complex persistence or addressable-content system before the foundation is validated.

## Initial technical decisions to verify in Unity

- Use Unity 6 with URP and Android as the target platform.
- Enable OpenXR and configure the Quest-compatible interaction profile(s) supported by the installed package versions.
- Select one supported Meta XR integration path and avoid duplicating XR providers. Record installed package versions in the project.
- Use XR Interaction Toolkit only if compatible with the chosen Unity/OpenXR versions; verify locomotion and input APIs in the installed package before coding against them.
- Keep rendering settings conservative initially. Establish visual and performance baselines on Quest 3 before raising quality.

These are planning choices, not claims that the editor, packages, SDK, or headset have been configured.

