using System.IO;
using OneWorld;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

namespace OneWorld.Editor
{
    /// <summary>Creates the editable, primitive-based foundation scenes and a reusable portal prefab.</summary>
    public static class OneWorldFoundationBuilder
    {
        private const string Root = "Assets/ONEWORLD";
        private const string SceneFolder = Root + "/Scenes";
        private const string PrefabFolder = Root + "/Prefabs";
        private const string MaterialFolder = Root + "/Materials";

        private struct WorldInfo
        {
            public string title, description, scene;
            public Color color;
            public WorldInfo(string title, string description, string scene, Color color)
            { this.title = title; this.description = description; this.scene = scene; this.color = color; }
        }

        private static readonly WorldInfo[] Worlds =
        {
            new WorldInfo("AGRICULTURE", "Modern Indian farming, smart tractors, drones and precision irrigation.", "World_Agriculture", new Color(0.20f, 0.78f, 0.32f)),
            new WorldInfo("INDIAN ARMY", "Explore army bases, soldiers, vehicles and defence technology.", "World_IndianArmy", new Color(0.89f, 0.53f, 0.18f)),
            new WorldInfo("VIKSIT BHARAT 2047", "Discover India's vision for inclusive growth, innovation and opportunity.", "World_ViksitBharat2047", new Color(0.98f, 0.72f, 0.20f)),
            new WorldInfo("FUTURISTIC PUNE", "See smart roads, transit, connected services and tomorrow's city.", "World_FuturisticPune", new Color(0.20f, 0.68f, 0.96f)),
            new WorldInfo("SPACE", "Journey through ISRO missions, the Moon, space stations and Mars.", "World_Space", new Color(0.67f, 0.43f, 1.00f))
        };

        [MenuItem("ONE WORLD/Generate Foundation Scenes")]
        public static void Generate()
        {
            EnsureFolders();
            var hubPath = SceneFolder + "/ExhibitionHub.unity";
            bool targetExists = File.Exists(hubPath) || File.Exists(SceneFolder + "/Welcome.unity") || File.Exists(PrefabFolder + "/WorldPortal.prefab");
            foreach (var world in Worlds)
                targetExists |= File.Exists(SceneFolder + "/" + world.scene + ".unity");
            if (targetExists)
            {
                EditorUtility.DisplayDialog("ONE WORLD", "A target scene or portal prefab already exists. Nothing was overwritten. Rename or move the existing asset before generating again.", "OK");
                return;
            }
            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo()) return;

            ConfigureUrpIfNeeded();
            var scenes = new System.Collections.Generic.List<EditorBuildSettingsScene>();
            CreateWelcome(); scenes.Add(new EditorBuildSettingsScene(SceneFolder + "/Welcome.unity", true));
            SavePortalPrefab(Worlds[0]);
            CreateHub(); scenes.Add(new EditorBuildSettingsScene(hubPath, true));
            foreach (var world in Worlds)
            {
                var path = SceneFolder + "/" + world.scene + ".unity";
                CreateDestination(world);
                scenes.Add(new EditorBuildSettingsScene(path, true));
            }
            EditorBuildSettings.scenes = scenes.ToArray();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            EditorSceneManager.OpenScene(SceneFolder + "/Welcome.unity");
            EditorUtility.DisplayDialog("ONE WORLD", "Created Welcome, ExhibitionHub, five placeholder scenes, and Build Settings entries. Open Welcome and press Play.", "OK");
        }

        private static void ConfigureUrpIfNeeded()
        {
            if (GraphicsSettings.defaultRenderPipeline != null) return;
            var renderer = ScriptableObject.CreateInstance<UniversalRendererData>();
            var pipeline = UniversalRenderPipelineAsset.Create(renderer);
            pipeline.name = "ONE WORLD URP - Quest Baseline";
            pipeline.supportsHDR = false;
            pipeline.supportsCameraDepthTexture = false;
            pipeline.supportsCameraOpaqueTexture = false;
            pipeline.msaaSampleCount = 2;
            pipeline.shadowDistance = 0f;
            pipeline.maxAdditionalLightsCount = 0;
            AssetDatabase.CreateAsset(renderer, MaterialFolder + "/ONE WORLD Renderer.asset");
            AssetDatabase.CreateAsset(pipeline, MaterialFolder + "/ONE WORLD URP.asset");
            GraphicsSettings.defaultRenderPipeline = pipeline;
            QualitySettings.renderPipeline = pipeline;
        }

        private static void EnsureFolders()
        {
            foreach (var folder in new[] { SceneFolder, PrefabFolder, MaterialFolder })
                if (!AssetDatabase.IsValidFolder(folder)) AssetDatabase.CreateFolder(Root, folder.Substring((Root + "/").Length));
        }

        private static Material MakeMaterial(string name, Color color, bool emission = false)
        {
            var path = MaterialFolder + "/" + name + ".mat";
            var material = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (material == null)
            {
                var shader = Shader.Find("Universal Render Pipeline/Lit") ?? Shader.Find("Standard");
                material = new Material(shader);
                AssetDatabase.CreateAsset(material, path);
            }
            material.color = color;
            if (emission && material.HasProperty("_EmissionColor"))
            {
                material.EnableKeyword("_EMISSION");
                material.SetColor("_EmissionColor", color * 1.5f);
            }
            EditorUtility.SetDirty(material);
            return material;
        }

        private static void CreateWelcome()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddLighting(); AddFloor(new Color(0.025f, 0.04f, 0.09f));
            AddText("ONE WORLD", new Vector3(0, 2.0f, 3), 0.65f, Color.white, "Welcome Title");
            AddText("AN IMMERSIVE VR EXHIBITION\nFive worlds. One journey.", new Vector3(0, 1.2f, 3), 0.22f, new Color(0.72f, 0.83f, 1), "Welcome Subtitle");
            var button = GameObject.CreatePrimitive(PrimitiveType.Cube);
            button.name = "Enter Exhibition"; button.transform.position = new Vector3(0, 0.5f, 2.6f); button.transform.localScale = new Vector3(1.6f, 0.35f, 0.25f);
            button.GetComponent<Renderer>().sharedMaterial = MakeMaterial("WelcomeButton", new Color(0.05f, 0.55f, 0.8f), true);
            var action = button.AddComponent<SceneEntryButton>(); action.Configure("ExhibitionHub");
            AddText("ENTER EXHIBITION", new Vector3(0, 0.5f, 2.44f), 0.13f, Color.white, "Enter Label");
            AddPlayerRig(new Vector3(0, 0, 0)); AddTransition();
            SaveScene(scene, SceneFolder + "/Welcome.unity");
        }

        private static void CreateHub()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddLighting(); AddFloor(new Color(0.035f, 0.055f, 0.105f));
            AddText("ONE WORLD", new Vector3(0, 2.25f, 3.7f), 0.32f, Color.white, "Hub Title");
            AddText("Choose a world. Point and press the right trigger to enter.\nAim at the floor and press trigger to teleport.", new Vector3(0, 1.85f, 3.7f), 0.12f, new Color(0.72f, 0.83f, 1), "Hub Instructions");
            AddText("AGRICULTURE      ARMY      VIKSIT BHARAT      PUNE      SPACE", new Vector3(0, 0.18f, 5.2f), 0.12f, Color.white, "Hub Legend");
            for (int i = 0; i < Worlds.Length; i++)
            {
                float angle = (i - 2) * 0.48f;
                var position = new Vector3(Mathf.Sin(angle) * 4.4f, 1.2f, 4.2f + Mathf.Cos(angle) * 0.5f);
                CreatePortal(Worlds[i], position, i);
            }
            var returnButton = GameObject.CreatePrimitive(PrimitiveType.Cube);
            returnButton.name = "Return to Welcome"; returnButton.transform.position = new Vector3(0, 0.15f, -3.2f); returnButton.transform.localScale = new Vector3(1.4f, 0.25f, 0.25f);
            returnButton.GetComponent<Renderer>().sharedMaterial = MakeMaterial("ReturnButton", new Color(0.18f, 0.23f, 0.32f));
            returnButton.AddComponent<SceneEntryButton>().Configure("Welcome");
            AddText("WELCOME", new Vector3(0, 0.15f, -3.36f), 0.1f, Color.white, "Return Label");
            AddPlayerRig(Vector3.zero); AddTransition();
            SaveScene(scene, SceneFolder + "/ExhibitionHub.unity");
        }

        private static void CreatePortal(WorldInfo world, Vector3 position, int index)
        {
            var portalPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(PrefabFolder + "/WorldPortal.prefab");
            var root = (GameObject)PrefabUtility.InstantiatePrefab(portalPrefab);
            root.name = "Portal - " + world.title;
            root.transform.position = position;
            var portal = root.GetComponent<WorldPortal>(); portal.Configure(world.title, world.description, world.scene, world.color);
            var back = root.transform.Find("Portal Frame").gameObject;
            back.GetComponent<Renderer>().sharedMaterial = MakeMaterial("Portal_" + index, world.color, true);
            var collider = back.GetComponent<BoxCollider>(); collider.isTrigger = false;
            var center = GameObject.CreatePrimitive(PrimitiveType.Cube);
            center.name = "Portal Surface"; center.transform.SetParent(root.transform, false); center.transform.localPosition = new Vector3(0, 0, -0.12f); center.transform.localScale = new Vector3(1.1f, 1.72f, 0.04f);
            center.GetComponent<Renderer>().sharedMaterial = MakeMaterial("PortalSurface_" + index, Color.Lerp(world.color, Color.black, 0.68f), true);
            Object.DestroyImmediate(center.GetComponent<Collider>());
            AddText(world.title, position + new Vector3(0, 1.25f, -0.12f), 0.16f, world.color, world.title + " Label");
            AddText(WrapDescription(world.description), position + new Vector3(0, -0.78f, -0.12f), 0.065f, Color.white, world.title + " Description");
        }

        private static string WrapDescription(string value)
        {
            var words = value.Split(' ');
            var lines = new System.Collections.Generic.List<string>();
            var line = "";
            foreach (var word in words)
            {
                if ((line + " " + word).Trim().Length > 30)
                {
                    lines.Add(line);
                    line = word;
                }
                else line = (line + " " + word).Trim();
            }
            if (!string.IsNullOrEmpty(line)) lines.Add(line);
            return string.Join("\n", lines);
        }

        private static void SavePortalPrefab(WorldInfo world)
        {
            var temporary = new GameObject("Portal - " + world.title);
            var portal = temporary.AddComponent<WorldPortal>(); portal.Configure(world.title, world.description, world.scene, world.color);
            var frame = GameObject.CreatePrimitive(PrimitiveType.Cube); frame.name = "Portal Frame"; frame.transform.SetParent(temporary.transform, false);
            frame.transform.localScale = new Vector3(1.35f, 2, 0.18f); frame.GetComponent<Renderer>().sharedMaterial = MakeMaterial("PortalPrefab", world.color, true);
            var collider = frame.GetComponent<BoxCollider>(); collider.isTrigger = false;
            PrefabUtility.SaveAsPrefabAsset(temporary, PrefabFolder + "/WorldPortal.prefab");
            Object.DestroyImmediate(temporary);
        }

        private static void CreateDestination(WorldInfo world)
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            AddLighting(); AddFloor(Color.Lerp(world.color, Color.black, 0.88f));
            AddText(world.title, new Vector3(0, 2.0f, 3), 0.32f, world.color, "Destination Title");
            AddText("This is a placeholder destination.\nDetailed world content will be added after the VR foundation is tested.", new Vector3(0, 1.3f, 3), 0.12f, Color.white, "Placeholder Note");
            var back = GameObject.CreatePrimitive(PrimitiveType.Cube); back.name = "Return to Exhibition Hub";
            back.transform.position = new Vector3(0, 0.5f, 2.5f); back.transform.localScale = new Vector3(1.8f, 0.35f, 0.25f);
            back.GetComponent<Renderer>().sharedMaterial = MakeMaterial("DestinationReturn", new Color(0.05f, 0.55f, 0.8f), true);
            back.AddComponent<SceneEntryButton>().Configure("ExhibitionHub");
            AddText("RETURN TO HUB", new Vector3(0, 0.5f, 2.34f), 0.1f, Color.white, "Return Label");
            AddPlayerRig(Vector3.zero); AddTransition();
            SaveScene(scene, SceneFolder + "/" + world.scene + ".unity");
        }

        private static void AddPlayerRig(Vector3 position)
        {
            var rig = new GameObject("ONE WORLD XR Player"); rig.transform.position = position;
            rig.AddComponent<QuestPlayerRig>();
            var head = new GameObject("Head"); head.transform.SetParent(rig.transform, false); head.transform.localPosition = new Vector3(0, 1.65f, 0);
            var camera = head.AddComponent<Camera>(); camera.tag = "MainCamera"; camera.nearClipPlane = 0.08f; camera.farClipPlane = 60; camera.fieldOfView = 90;
            head.AddComponent<AudioListener>();
            var left = new GameObject("Left Controller"); left.transform.SetParent(rig.transform, false); left.transform.localPosition = new Vector3(-0.25f, 1.3f, 0.35f);
            var right = new GameObject("Right Controller"); right.transform.SetParent(rig.transform, false); right.transform.localPosition = new Vector3(0.25f, 1.3f, 0.35f);
            var rayObject = new GameObject("Teleport Interaction Ray"); rayObject.transform.SetParent(rig.transform, false);
            var line = rayObject.AddComponent<LineRenderer>(); line.positionCount = 2; line.startWidth = 0.012f; line.endWidth = 0.006f; line.useWorldSpace = true;
            line.material = MakeMaterial("InteractionRay", Color.white, true);
            var serialized = new SerializedObject(rig.GetComponent<QuestPlayerRig>());
            serialized.FindProperty("head").objectReferenceValue = head.transform;
            serialized.FindProperty("leftController").objectReferenceValue = left.transform;
            serialized.FindProperty("rightController").objectReferenceValue = right.transform;
            serialized.FindProperty("teleportRay").objectReferenceValue = line;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        private static void AddFloor(Color color)
        {
            var floor = GameObject.CreatePrimitive(PrimitiveType.Plane); floor.name = "Teleport Floor"; floor.transform.position = new Vector3(0, -0.03f, 0); floor.transform.localScale = new Vector3(12, 1, 12);
            floor.AddComponent<TeleportSurface>(); floor.GetComponent<Renderer>().sharedMaterial = MakeMaterial("Floor", color);
        }

        private static void AddLighting()
        {
            RenderSettings.ambientLight = new Color(0.3f, 0.34f, 0.42f);
            var lightObject = new GameObject("Exhibition Light");
            var light = lightObject.AddComponent<Light>(); light.type = LightType.Directional; light.intensity = 0.75f; light.shadows = LightShadows.None; lightObject.transform.rotation = Quaternion.Euler(45, -25, 0);
        }

        private static void AddText(string value, Vector3 position, float size, Color color, string name)
        {
            var textObject = new GameObject(name); textObject.transform.position = position; textObject.transform.rotation = Quaternion.identity;
            var text = textObject.AddComponent<TextMesh>(); text.text = value; text.anchor = TextAnchor.MiddleCenter; text.alignment = TextAlignment.Center;
            text.fontSize = 64; text.characterSize = size; text.color = color; text.richText = false;
        }

        private static void AddTransition() => new GameObject("Scene Transition").AddComponent<SceneTransition>();

        private static void SaveScene(Scene scene, string path)
        {
            EditorSceneManager.SaveScene(scene, path);
        }
    }

}
