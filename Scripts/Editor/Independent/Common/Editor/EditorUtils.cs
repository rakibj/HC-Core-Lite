using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Rakib
{
    public static class EditorUtils
    {
        public static T GetAssetWithScript<T>(string path) where T : MonoBehaviour
        {
            T tmp;
            string assetPath;
            GameObject asset;
            var assetList = new List<T>();
            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] {path});
            for (int i = 0; i < guids.Length; i++)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                tmp = asset.GetComponent<T>();
                if(tmp != null) assetList.Add(tmp);
            }

            return assetList[0];
        }
        public static List<T> GetAssetsWithScript<T>(string path) where T : MonoBehaviour
        {
            T tmp;
            string assetPath;
            GameObject asset;
            var assetList = new List<T>();
            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] {path});
            for (int i = 0; i < guids.Length; i++)
            {
                assetPath = AssetDatabase.GUIDToAssetPath(guids[i]);
                asset = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
                tmp = asset.GetComponent<T>();
                if(tmp != null) assetList.Add(tmp);
            }

            return assetList;
        }
        public static List<T> GetListFromEnum<T>()
        {
            var enumList = new List<T>();
            var enums = System.Enum.GetValues(typeof(T));
            foreach (T e in enums)
                enumList.Add(e);
            return enumList;
        }
        
                
    }
}