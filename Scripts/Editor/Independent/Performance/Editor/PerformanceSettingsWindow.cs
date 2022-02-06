using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Rakib
{
    public class PerformanceSettingsWindow : EditorWindow
    {
        private enum PerformanceTab{ Cpu, Gpu, Tips }
        private PerformanceTab _selectedTab;
        private static PerformanceSettingsWindow instance;
        private List<PerformanceTab> _tabs = new List<PerformanceTab>();
        private List<string> _tabLabels = new List<string>();
        private List<string> _tips = new List<string>();
        private bool _cpuFoldout;
        private Vector2 _tipsScrollPos;

        [MenuItem("Rakib/Tools/Optimized Settings")]
        public static void OpenWindow()
        {
            instance = (PerformanceSettingsWindow) GetWindow(typeof(PerformanceSettingsWindow));
            instance.titleContent = new GUIContent("Optimized Settings");
        }

        private void OnGUI()
        {
            var headerStyle = new GUIStyle()
            {
                fontSize = 13,
                fontStyle = FontStyle.Bold,
            };
            headerStyle.normal.textColor = new Color(0.8f,0.8f,0.8f);
            headerStyle.alignment = TextAnchor.MiddleLeft;
            
            GUILayout.Space(10);
            GUILayout.Label("Quick Settings", headerStyle);
            DrawPerformanceSettingsButtons();
            GUILayout.Space(40);

            GUILayout.Label("Detailed Settings", headerStyle);
            _tabs = EditorUtils.GetListFromEnum<PerformanceTab>();
            _tabLabels = new List<string>();
            foreach (var tab in _tabs)
                _tabLabels.Add(tab.ToString());
            _selectedTab = (PerformanceTab) GUILayout.Toolbar((int) _selectedTab, _tabLabels.ToArray(), GUILayout.Height(25));

            switch (_selectedTab)
            {
                case PerformanceTab.Cpu:
                    DrawCpuTab();
                    break;
                case PerformanceTab.Gpu:
                    DrawGpuTab();
                    break;
                case PerformanceTab.Tips:
                    DrawTipsTab();
                    break;
            }
        }

        private void DrawTipsTab()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawProTips();
            EditorGUILayout.EndVertical();
        }
        private void DrawGpuTab()
        {
            DrawGpuSettings();
        }
        private void DrawCpuTab()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            DrawCpuRenderingSettings();
            GUILayout.Space(20);
            DrawCpuPhysicsSettings();
            EditorGUILayout.EndVertical();
        }
        private void DrawPerformanceSettingsButtons()
        {
            EditorGUILayout.BeginVertical();
            if (GUILayout.Button("Best CPU Settings", GUILayout.MaxWidth(200)))
            {
                SetBestCpuSettings();
            }
            if (GUILayout.Button("Mobile Quality Settings", GUILayout.MaxWidth(200)))
            {
                SetBestQualitySettings();
            }
            EditorGUILayout.EndHorizontal();
        }

        public static void SetBestQualitySettings()
        {
            SetQualityIos();
            EditorUtility.DisplayDialog("Performance Settings", "All optimized quality settings applied", "Ok");
        }

        public static void SetBestCpuSettings()
        {
            PlayerSettings.gpuSkinning = true;
            PlayerSettings.gcIncremental = true;
            Physics.autoSyncTransforms = false;
            Physics.reuseCollisionCallbacks = true;
            SetBatchingForPlatform(BuildTarget.iOS, 1, 1);
            SetBatchingForPlatform(BuildTarget.Android, 1, 1);
            Debug.Log("Best CPU settings applied");
            EditorUtility.DisplayDialog("Performance Settings", "All optimized settings applied for CPU", "Ok");
        }

        private static void DrawBatchingSettings()
        {
            //Batching
            if (IsBatchingMethodsAvailable())
            {
                if(IsStaticBatchingOn() && IsDynamicBatchingOn())
                    EditorGUILayout.HelpBox("Batching is On!", MessageType.Info);
                
                if (!IsStaticBatchingOn() || !IsDynamicBatchingOn())
                {
                    if (GUILayout.Button("Turn on batching"))
                    {
                        SetBatchingForPlatform(BuildTarget.Android, 1, 1);
                        SetBatchingForPlatform(BuildTarget.iOS, 1, 1);
                    }
                }
            }
            else
            {
                GUILayout.Label("Make sure the following are true:" +
                                "\nPlayer Settings -> Other Settings -> Rendering -> Static Batching -> true" +
                                "\nPlayer Settings -> Other Settings -> Rendering -> Dynamic Batching -> true");
                if (GUILayout.Button("Open Player Settings in Inspector"))
                    Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("PlayerSettings");
            }
        }
        private static void DrawCpuRenderingSettings()
        {
            EditorGUILayout.LabelField("Rendering Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            
            //Batching
            DrawBatchingSettings();
            
            //GPU Skinning
            PlayerSettings.gpuSkinning = EditorGUILayout.Toggle("Compute Skinning", PlayerSettings.gpuSkinning);
            if (!PlayerSettings.gpuSkinning)
                EditorGUILayout.HelpBox(
                    "Ideally should be on. It moves the calculation of 'transforming mesh vertices based on current position of their bones'" +
                    " from CPU to GPU", MessageType.Warning);

            //Incremental GC
            PlayerSettings.gcIncremental = EditorGUILayout.Toggle("Incremental GC", PlayerSettings.gcIncremental);
            if (!PlayerSettings.gcIncremental)
                EditorGUILayout.HelpBox(
                    "Ideally should be on. Makes garbage collection less harsh by collecting it over several frames",
                    MessageType.Warning);


            EditorGUILayout.EndVertical();
        }
        private static void DrawCpuPhysicsSettings()
        {
            EditorGUILayout.LabelField("Physics Settings", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);

            //AutoSync Transforms
            Physics.autoSyncTransforms = !EditorGUILayout.Toggle("Don't AutoSync Transforms", !Physics.autoSyncTransforms);
            if (Physics.autoSyncTransforms)
                EditorGUILayout.HelpBox(
                    "If auto sync is on, it causes extra physics calculation outside Physics step when transform is changed"
                    , MessageType.Warning);

            //Reuse Collision Callback
            Physics.reuseCollisionCallbacks =
                EditorGUILayout.Toggle("Reuse Collision Callbacks", Physics.reuseCollisionCallbacks);
            if (!Physics.reuseCollisionCallbacks)
                EditorGUILayout.HelpBox(
                    "Ideally should be on. GC reuses collision callbacks instead of destroying and recreating new one"
                    , MessageType.Warning);

            EditorGUILayout.EndVertical();
        }
        private static void DrawGpuSettings()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            if (GUILayout.Button("Open Quality Settings in Inspector"))
                Selection.activeObject = Unsupported.GetSerializedAssetInterfaceSingleton("QualitySettings");
            EditorGUILayout.BeginHorizontal();

            if(GUILayout.Button("Set iOS Quality"))
                SetQualityIos();
            if(GUILayout.Button("Set CTR Quality"))
                SetQualityCtr();

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
        }
        private static void SetQualityIos()
        {
            QualitySettings.pixelLightCount = 1;
            QualitySettings.masterTextureLimit = 1;
            QualitySettings.antiAliasing = 4;
            QualitySettings.softParticles = false;
            QualitySettings.shadowResolution = ShadowResolution.High;
            QualitySettings.shadowDistance = 150;
            Debug.Log("Applied Quality Settings for iOS");
        }
        private static void SetQualityCtr()
        {
            QualitySettings.pixelLightCount = 3;
            QualitySettings.masterTextureLimit = 0;
            QualitySettings.antiAliasing = 8;
            QualitySettings.softParticles = true;
            QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
            QualitySettings.shadowDistance = 150;
            Debug.Log("Applied Quality Settings for CTR");
        }
        private static void SetBatchingForPlatform(BuildTarget platform, int staticBatching, int dynamicBatching)
        {
            var method = typeof(PlayerSettings).GetMethod("SetBatchingForPlatform", BindingFlags.Static | BindingFlags.Default | BindingFlags.NonPublic);
            var args = new object[]
            {
                platform,
                staticBatching,
                dynamicBatching
            };
            if (method != null) method.Invoke(null, args);
        }
        private static bool GetDynamicBatchingForPlatform(BuildTarget platform)
        {
            var method = typeof(PlayerSettings).GetMethod("GetBatchingForPlatform", BindingFlags.Static | BindingFlags.Default | BindingFlags.NonPublic);
            var staticBatching = 0; 
            var dynamicBatching = 0;
            var args = new object[]
            {
                platform,
                staticBatching,
                dynamicBatching
            };
            if (method != null) method.Invoke(null, args);
            var stat = (int)args[1];
            var dyn = (int)args[2];
            return dyn == 1;
        }
        private static bool GetStaticBatchingForPlatform(BuildTarget platform)
        {
            var method = typeof(PlayerSettings).GetMethod("GetBatchingForPlatform", BindingFlags.Static | BindingFlags.Default | BindingFlags.NonPublic);
            var staticBatching = 0; 
            var dynamicBatching = 0;
            var args = new object[]
            {
                platform,
                staticBatching,
                dynamicBatching
            };
            if (method != null) method.Invoke(null, args);
            var stat = (int)args[1];
            var dyn = (int)args[2];
            return stat == 1;
        }
        private static bool IsStaticBatchingOn() => GetStaticBatchingForPlatform(BuildTarget.iOS) &&
                                                    GetStaticBatchingForPlatform(BuildTarget.Android);
        private static bool IsDynamicBatchingOn() => GetDynamicBatchingForPlatform(BuildTarget.iOS) &&
                                                     GetDynamicBatchingForPlatform(BuildTarget.Android);
        private static bool IsBatchingMethodsAvailable()
        {
            var method = typeof(PlayerSettings).GetMethod("GetBatchingForPlatform", BindingFlags.Static | BindingFlags.Default | BindingFlags.NonPublic);
            return method != null;
        }
        private void DrawProTips()
        {
            _tipsScrollPos = GUILayout.BeginScrollView(_tipsScrollPos);
            var selectionGridIndex = -1;
            selectionGridIndex = GUILayout.SelectionGrid(
                selectionGridIndex,
                GetGuiContentsFromTips(),
                1,
                GetTipsGuiStyle()
            );
            GUILayout.EndScrollView();
        }
        private GUIContent[] GetGuiContentsFromTips()
        {
            _tips = new List<string>();
            _tips.Add("Make sure you have the following line of code in one of your Awake methods" +
                      "\nApplication.targetFrameRate = 60;");
            _tips.Add("Avoid Standard Shader. Use 'TCP2 Mobile' which is better looking with better performance");
            _tips.Add("Decrease 'Fixed Timestep' to get quick gains. Increase this instead of using continuous collision detection");
            _tips.Add("Use GPU instancing on materials that are reused a lot in non skinned meshes");
            
            var guiContents = new List<GUIContent>();
            for (int i = 0; i < _tips.Count; i++)
            {
                var guiContent = new GUIContent();
                guiContent.text = _tips[i];
                guiContents.Add(guiContent);
            }

            return guiContents.ToArray();
        }
        private GUIStyle GetTipsGuiStyle()
        {
            var guiStyle = new GUIStyle(GUI.skin.label);
            guiStyle.alignment = TextAnchor.MiddleLeft;
            guiStyle.wordWrap = true;
            guiStyle.fixedHeight = guiStyle.lineHeight * 2;
            guiStyle.stretchHeight = true;
            return guiStyle;
        }
    }
}