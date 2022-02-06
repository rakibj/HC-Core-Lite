using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChildrenTransformCopier : ScriptableObject
{
    private static Vector3[] positions;
    private static Quaternion[] rotations;
    private static Vector3[] scales;
    
    [MenuItem("CONTEXT/Transform/Copy Children Transforms _c",false,151)]
    static void DoRecord () 
    {
        var transform = Selection.activeTransform;
        int count = transform.childCount;
        positions = new Vector3[count];
        rotations = new Quaternion[count];
        scales = new Vector3[count];
        for(int i = 0; i < count; i++)
        {
            positions[i] = transform.GetChild(i).localPosition;
            rotations[i] = transform.GetChild(i).localRotation;
            scales[i] = transform.GetChild(i).localScale;
        }
    }
    
    [MenuItem ("CONTEXT/Transform/Paste Children Transforms _v",false,200)]
    static void DoApplyChildrenPosition () 
    {
        var transform = Selection.activeTransform;
        Undo.RecordObject(transform, "Paste Child Transforms" + transform.name);
        int count = transform.childCount;
        for(int i = 0; i < count; i++)
        {
            transform.GetChild(i).localPosition = positions[i];
            transform.GetChild(i).localRotation = rotations[i];
            transform.GetChild(i).localScale = scales[i];
        }

        EditorUtility.SetDirty(transform);
    }
}
