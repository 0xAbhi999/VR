# ONE WORLD Development Instructions

## Project goal

Build ONE WORLD, a single connected VR exhibition for Meta Quest 3 with a welcome experience, a shared central hub, and five destinations: Agriculture, Indian Army, Viksit Bharat 2047, Futuristic Pune, and Space.

## Working rules

- The project owner is a beginner. Explain technical steps in plain language and work one milestone at a time.
- Inspect the current files and Git status before making changes. Never overwrite or delete existing user files without approval.
- Keep the five worlds consistent with the project master plan. Do not build detailed world content until the foundation scene has been tested successfully.
- Prefer complete, usable, focused C# scripts over disconnected snippets. Keep systems small and easy to inspect.
- Do not assume Unity, packages, SDKs, devices, or build tools are installed. Check availability and report gaps.
- Target Unity 6, URP, C#, OpenXR with a compatible Meta XR integration, and standalone Android/Quest 3. Verify exact editor/package versions before relying on APIs or project settings.
- Prioritise comfortable VR: stable head-locked camera, seated/standing accessibility where practical, snap turning or teleport as initial options, no forced camera motion, readable world-space UI, and safe interaction reach.
- Prioritise Quest performance: avoid unnecessary real-time lights, expensive post-processing, excessive transparent layers, and unbounded scene content. Profile on device before tuning targets.
- Verify changes with the available editor/compiler/device workflow. Do not claim a Unity build or headset test succeeded unless it actually ran.
- Keep generated assets and scripts in the agreed project structure. Do not add packages or external assets without checking compatibility and licensing.

## Milestone gate

The foundation milestone is complete only after the welcome-to-hub flow, shared XR rig, basic locomotion, one interaction, and one information panel have been exercised in Unity and validated on Quest 3 (or limitations are explicitly recorded). Only then proceed to detailed exhibition worlds.

