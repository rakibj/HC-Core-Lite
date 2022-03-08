#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Rakib
{
    [CreateAssetMenu(menuName = "Rakib/General Settings")]
    public class GeneralSettings : ScriptableObject
    {
        public CustomPlayerSettings playerSettings;
        public LevelSettings levelSettings;
    }

    [System.Serializable]
    public class CustomPlayerSettings
    {
        //TODO implement update values on player settings
        public string gameName = "Game Name";
        public string bundleIdentifier = "com.lightneer.gamename";
        public string buildVersion = "1.0.0";
        
        private void ResetSettings()
        {
#if UNITY_EDITOR
            bundleIdentifier = "com.lightneer.gamename";
            gameName = "Game Name";
            buildVersion = "1.0.0";
            Save();
#endif
        }
        //[HorizontalGroup("PlayerSettings/Button")]
        //[Button(ButtonSizes.Small)]
        private void FetchSettings()
        {
#if UNITY_EDITOR
            bundleIdentifier = PlayerSettings.GetApplicationIdentifier(BuildTargetGroup.Android);
            gameName = PlayerSettings.productName;
            buildVersion = PlayerSettings.bundleVersion;
            var fetchedIcon = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Android)[0];
            var sprite = Sprite.Create(fetchedIcon, new Rect(0.0f, 0.0f, fetchedIcon.width, fetchedIcon.height), new Vector2(0.5f, 0.5f), 100.0f);
#endif
        }
        private void Save()
        {
#if UNITY_EDITOR
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, bundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.iOS, bundleIdentifier);
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Standalone, bundleIdentifier);
            PlayerSettings.companyName = "Lightneer";
            PlayerSettings.productName = gameName;
            PlayerSettings.bundleVersion = buildVersion;
#endif
        }

    }

    [System.Serializable]
    public class LevelSettings
    {
        public enum LevelLoadType{ LoopActiveScene, SequenceThenRandom }
        [Header("Level Settings")] 
        public bool instantRestart = true;
        public LevelLoadType levelLoadType;
        public int totalLevels;
        public int repeatFrom;
        
        public bool Validate()
        {
            var currentLoadType = LevelLoadType.LoopActiveScene;
            var maxLevels = -1;
            var sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings;     
            var scenes = new string[sceneCount];
            for( var i = 0; i < sceneCount; i++ )
                scenes[i] = System.IO.Path.GetFileNameWithoutExtension( UnityEngine.SceneManagement.SceneUtility.GetScenePathByBuildIndex( i ) );

            for (int i = 0; i < sceneCount; i++)
            {
                if(int.TryParse(scenes[i], out var level))
                {
                    if (level > maxLevels) maxLevels = level;
                }
            }

            if (maxLevels == -1) currentLoadType = LevelLoadType.LoopActiveScene;
            else
            {
                currentLoadType = LevelLoadType.SequenceThenRandom;
                totalLevels = maxLevels;
            }
            return currentLoadType == levelLoadType;
        }
    }
}