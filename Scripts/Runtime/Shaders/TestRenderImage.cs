using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class TestRenderImage : MonoBehaviour
{
    public Shader curShader;
    [Range(0f,1f)]public float greyScaleAmount = 1f;
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
            ScreenMat.SetFloat("_Luminosity", greyScaleAmount);
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
