using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rakib
{
    public class WinUI : MonoBehaviour
    {
        [SerializeField] private float appearDelay = 0.5f;
        [SerializeField] private UIView view;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button winButton;
        private IWinUI[] _winUIs;
        private int _currentLevel;
        private int _scoreWithoutMultiplier;
        private int _scoreMultiplier;

        private void Start()
        {
            _winUIs = GetComponentsInChildren<IWinUI>();
        }

        private void OnValidate()
        {
            if (view == null) view = GetComponent<UIView>();
            if (winButton == null) winButton = GetComponentInChildren<Button>();
        }

        protected virtual void OnEnable()
        {
            winButton.onClick.AddListener(WinButtonClick);
        }

        protected virtual void OnDisable()
        {
            winButton.onClick.RemoveListener(WinButtonClick);
        }

        protected void OnLevelComplete(int currentLevel)
        {
            _currentLevel = currentLevel;
            ShowWinView();
        }

        private void ShowWinView()
        {
            Invoke(nameof(ShowView), appearDelay);
        }
        
        private void ShowView()
        {
            levelText.text = "LEVEL " + (_currentLevel) + " COMPLETE";

            foreach (var winUI in _winUIs)
                winUI?.Prepare();
            
            view.Show();
            foreach (var winUI in _winUIs)
            {
                winUI.Show();
            }
        }
        
        public virtual void WinButtonClick()
        {
        }

    }
}