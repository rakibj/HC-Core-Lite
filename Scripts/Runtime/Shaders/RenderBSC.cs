using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderBSC : MonoBehaviour
{
    public Shader curShader;
    [Range(0f,2f)]public float brightness = 1.0f;
    [Range(0f,2f)]public float saturation = 1.0f;
    [Range(0f,3f)]public float contrast = 1.0f;
    private Material _screenMat;

    public Material ScreenMat
    {
        get
        {
            if (_screenMat == null)
            {
                _screenMat = new Material(curShader);
                _screenMat.hideFlags = HideFlags.HideAndDontSave;
            }
            return _screenMat;
        }
    }

    private void Start()
    {
        if (!curShader || !curShader.isSupported)
            enabled = false;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (curShader != null)
        {
            ScreenMat.SetFloat("_Brightness", brightness);
            ScreenMat.SetFloat("_Saturation", saturation);
            ScreenMat.SetFloat("_Contrast", contrast);
            Graphics.Blit(src, dest, _screenMat);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }

    private void OnDisable()
    {
        if (_screenMat)
            DestroyImmediate(_screenMat);
    }
}
