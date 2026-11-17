# Proposed Unity Folder Structure

```text
Assets/
  ONEWORLD/
    Audio/
      Music/
      SFX/
      Narration/
    Art/
      Materials/
      Models/
      Textures/
    Data/
      Worlds/
    Prefabs/
      Player/
      UI/
      Interactables/
      Navigation/
    Scenes/
      App/
      Hub/
      Foundation/
      Worlds/
        Agriculture/
        IndianArmy/
        ViksitBharat2047/
        FuturisticPune/
        Space/
    Scripts/
      App/
      Player/
      Interaction/
      Navigation/
      UI/
      Audio/
      Worlds/
    Settings/
    UI/
  XR/
Packages/
ProjectSettings/
```

Keep Unity package and project configuration in the standard root `Packages/` and `ProjectSettings/` folders. Keep imported third-party assets in clearly named vendor folders if they are added later; do not mix them into `Assets/ONEWORLD` authored content. Use version control for source assets and settings, and configure Git LFS only if large binary assets are introduced.

The folders are a target layout, not proof that a Unity project has been created. Create only folders needed by the active milestone to keep the repository useful and uncluttered.

