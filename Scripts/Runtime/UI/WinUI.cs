
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Rakib
{
    public class WinUI : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;
        [SerializeField] private float appearDelay = 0.5f;
        [SerializeField] private UIView view;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private Button winButton;
        private IFinalScoreUI[] _finalScoreUI;
        private int _scoreWithoutMultiplier;
        private int _scoreMultiplier;

        private void Start()
        {
            _finalScoreUI = GetComponentsInChildren<IFinalScoreUI>();
        }

        private void OnValidate()
        {
            if (view == null) view = GetComponent<UIView>();
            if (winButton == null) winButton = GetComponentInChildren<Button>();
        }

        private void OnEnable()
        {
            winButton.onClick.AddListener(WinButtonClick);
            _signalBus.Subscribe<LevelCompleteSignal>(OnLevelComplete);
        }

        private void OnDisable()
        {
            winButton.onClick.RemoveListener(WinButtonClick);
            _signalBus.Unsubscribe<LevelCompleteSignal>(OnLevelComplete);
        }

        private void OnLevelComplete(LevelCompleteSignal signal)
        {
            
            ShowWinView();
        }

        private void ShowWinView()
        {
            Invoke(nameof(ShowView), appearDelay);
        }
        
        private void ShowView()
        {
            levelText.text = "LEVEL " + (_storageManager.CurrentLevel) + " COMPLETE";

            foreach (var finalScoreUI in _finalScoreUI)
                finalScoreUI?.Prepare(_storageManager.CurrentEntity, _storageManager.CurrentScore);
            
            view.Show();
            _finalScoreUI[0].Show();
        }
        
        public void WinButtonClick()
        {
            _signalBus.Fire(new LevelLoadNextSignal());
        }

    }
}