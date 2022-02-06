using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class RenderDepth : MonoBehaviour
{
    public Shader curShader;
    [Range(0f,1f)]public float depthPower = 1f;
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
            ScreenMat.SetFloat("_DepthPower", depthPower);
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
