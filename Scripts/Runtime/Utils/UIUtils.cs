using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rakib
{
    public static class UIUtils
    {
        public static IEnumerator IncrementScore(int from, int to, float duration, Action<int> onUpdate = null, Action onComplete = null)
        {
            int start = from;
            int score;
            
            for (float timer = 0; timer < duration; timer += Time.deltaTime) 
            {
                float progress = timer / duration;
                score = (int)Mathf.Lerp (start, to, progress);
                onUpdate?.Invoke(score);
                yield return null;
            }
            onUpdate?.Invoke(to);
            onComplete?.Invoke();
        }
        
        public static string MakeKmb(int num )
        {
            double numStr;
            if( num < 1000 )
            {
                return num.ToString();
            }
            else if( num < 1000000 )
            {
                numStr = num/1000f;
                return numStr.ToString("F1") + "K";
            }
            else if( num < 1000000000f )
            {
                numStr = num/1000000f;
                return numStr.ToString("F1") + "M";
            }
            else
            {
                numStr = num/1000000000f;
                return  numStr.ToString("F1") + "B";
            }
            
        }

        public static IEnumerator FadeOut(CanvasGroup canvasGroup, float duration = 0.5f)
        {
            canvasGroup.alpha = 1f;
            var factor = (1 / duration) * Time.deltaTime;

            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha -= factor;
                yield return null;
            }
            
            HideCanvasGroup(canvasGroup);
        }

        public static void HideCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }


        public static IEnumerator FadeIn(CanvasGroup canvasGroup, float duration = 0.5f)
        {
            canvasGroup.alpha = 0f;
            var factor = (1 / duration) * Time.deltaTime;

            while (canvasGroup.alpha > 0f)
            {
                canvasGroup.alpha += factor;
                yield return null;
            }
            
            ShowCanvasGroup(canvasGroup);
        }

        public static void ShowCanvasGroup(CanvasGroup canvasGroup)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;
        }
    }
}