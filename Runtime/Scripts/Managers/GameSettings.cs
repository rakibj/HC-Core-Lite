using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Rakib
{
    [CreateAssetMenu(menuName = "Rakib/GameSettings")]
    public class GameSettings: ScriptableObject
    {
        public ModuleSettings moduleSettings;
    }

    [Serializable]
    public class ModuleSettings
    {
        private static readonly string ANALYTICS_DIRECTIVE = "LIGHTNEER_ANALYTICS"; 
        
        //TODO update with button
        //[OnValueChanged(nameof(UpdateAnalytics))] [InlineButton(nameof(GetAnalytics), "Fetch")]
        public bool enableAnalytics = false;
        private void GetAnalytics() => enableAnalytics = PlayerUtils.IsDefineDirectiveExists(ANALYTICS_DIRECTIVE);
        private void UpdateAnalytics()
        {
            if (enableAnalytics)
            {
                PlayerUtils.SetDefineDirective(ANALYTICS_DIRECTIVE);
            }
            else
            {
                PlayerUtils.RemoveDefineDirective(ANALYTICS_DIRECTIVE);
            }
        }

    }
    
    
}