using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Rakib
{
    public class LoseUI : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private GeneralSettings _generalSettings;
        [Inject] private StorageManager _storageManager;
        [SerializeField] private float appearDelay = 0.5f;
        [SerializeField] private UIView view;
        [SerializeField] private TMP_Text levelCompleteText;
        [SerializeField] private Button loadNextButton;

        private void OnValidate()
        {
            view = GetComponent<UIView>();
            loadNextButton = GetComponentInChildren<Button>();
        }

        private void OnEnable()
        {
            loadNextButton.onClick.AddListener(LoseButtonClick);
            _signalBus.Subscribe<LevelFailSignal>(ShowViewAfter);
        }

        private void OnDisable()
        {
            loadNextButton.onClick.RemoveListener(LoseButtonClick);
            _signalBus.Unsubscribe<LevelFailSignal>(ShowViewAfter);
        }

        public void ShowViewAfter()
        {
            if (_generalSettings.levelSettings.instantRestart)
            {
                LoseButtonClick();
                return;
            }
            Invoke(nameof(ShowView), appearDelay);
        }

        private void ShowView()
        {
            levelCompleteText.text = "LEVEL " + _storageManager.CurrentLevel + " FAIL";
            view.Show();
        }
        private void LoseButtonClick()
        {
            _signalBus.Fire(new LevelLoadSameSignal());
        }
    }
}