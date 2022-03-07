using System;
using UnityEngine;
using Zenject;

namespace Rakib
{
    public class UIView : MonoBehaviour
    {
        [SerializeField] private bool hideOnStart;
        [SerializeField] private CanvasGroup canvasGroup;
        [Inject] private UIUtils _utils;

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
                _utils.ShowCanvasGroup(canvasGroup);
                return;
            }
            StartCoroutine(_utils.FadeIn(canvasGroup));
        }

        public void Hide(bool instant = false)
        {
            if (instant)
            {
                _utils.HideCanvasGroup(canvasGroup);
                return;
            }
            StartCoroutine(_utils.FadeOut(canvasGroup));
        }
    }
}