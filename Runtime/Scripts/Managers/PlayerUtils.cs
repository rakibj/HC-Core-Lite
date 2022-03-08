using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Rakib
{
    public static class PlayerUtils
    {
#if UNITY_EDITOR
        private static readonly BuildTargetGroup[] BUILD_TARGET_GROUPS = { BuildTargetGroup.Android, BuildTargetGroup.iOS, BuildTargetGroup.Standalone };
#endif
        public static void RemoveDefineDirective(string i_DefineDirective)
        {
#if UNITY_EDITOR
            foreach (var buildTarget in BUILD_TARGET_GROUPS)
            {
                string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);

                if (defineSymbols.Contains(i_DefineDirective))
                {
                    defineSymbols = defineSymbols.Replace($";{i_DefineDirective}", "");
                    defineSymbols = defineSymbols.Replace(i_DefineDirective, "");

                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, defineSymbols);
                }
            }
#endif
        }
        public static void SetDefineDirective(string i_DefineDirective)
        {
#if UNITY_EDITOR
            foreach (var buildTarget in BUILD_TARGET_GROUPS)
            {
                string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);

                if (!defineSymbols.Contains(i_DefineDirective))
                {
                    defineSymbols += $";{i_DefineDirective}";

                    PlayerSettings.SetScriptingDefineSymbolsForGroup(buildTarget, defineSymbols);
                }
            }
#endif
        }
        public static bool IsDefineDirectiveExists(string i_DefineDirective)
        {
            bool result = true;
#if UNITY_EDITOR

            foreach (var buildTarget in BUILD_TARGET_GROUPS)
            {
                string defineSymbols = PlayerSettings.GetScriptingDefineSymbolsForGroup(buildTarget);

                if (!defineSymbols.Contains(i_DefineDirective))
                {
                    result = false;
                    break;
                }
            }

#endif
            return result;
        }
    }
}