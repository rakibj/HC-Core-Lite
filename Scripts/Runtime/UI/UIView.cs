using System;
using UnityEngine;
using Zenject;

namespace Rakib
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private bool hideOnStart;
        [SerializeField] private CanvasGroup canvasGroup;

        private void OnValidate()
        {
            if (canvasGroup == null) canvasGroup = GetComponentsInChildren<CanvasGroup>()[0];
        }

        private void Start()
        {
            GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            if(hideOnStart) Hide(true);
        }

        public void Show(bool instant = false)
        {
            if (instant)
            {
                UIUtils.ShowCanvasGroup(canvasGroup);
                return;
            }
            StartCoroutine(UIUtils.FadeIn(canvasGroup));
        }

        public void Hide(bool instant = false)
        {
            if (instant)
            {
                UIUtils.HideCanvasGroup(canvasGroup);
                return;
            }
            StartCoroutine(UIUtils.FadeOut(canvasGroup));
        }
    }
}