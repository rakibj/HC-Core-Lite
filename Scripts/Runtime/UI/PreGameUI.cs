using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Rakib
{
    public class PreGameUI : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;
        [SerializeField] private UIView view;
        [SerializeField] private TMP_Text levelText;
        [SerializeField] private LevelStartPromptType levelStartPromptType = LevelStartPromptType.TouchToStart;
        [SerializeField] private Button startButton;
        private bool _levelStarted = false;
        
        public enum LevelStartPromptType
        {
            ButtonToStart,
            TouchToStart
        }
        private void OnEnable()
        {
            if (levelStartPromptType == LevelStartPromptType.ButtonToStart) startButton.onClick.AddListener(StartGame);
            _signalBus.Subscribe<LevelStartSignal>(Debug_StartGame);
            _signalBus.Subscribe<LevelLoadSignal>(LevelLoaded);
        }

        private void OnDisable()
        {
            if (levelStartPromptType == LevelStartPromptType.ButtonToStart) startButton.onClick.RemoveListener(StartGame);
            _signalBus.Unsubscribe<LevelStartSignal>(Debug_StartGame);
            _signalBus.Unsubscribe<LevelLoadSignal>(LevelLoaded);
        }

        private void LevelLoaded()
        {
            if (levelStartPromptType != LevelStartPromptType.TouchToStart) return;
            StartCoroutine(CheckForInputRoutine());
        }

        private IEnumerator CheckForInputRoutine()
        {
            while (!Input.GetMouseButtonDown(0))
            {
                yield return null;
            }

            StartGame();
        }

        private void Start()
        {
            SetPreGameUI();
        }

        private void Debug_StartGame()
        {
            if (!_levelStarted)
            {
                view.Hide();
                _levelStarted = true;
            }
        }

        private void SetPreGameUI()
        {
            if (levelStartPromptType != LevelStartPromptType.ButtonToStart) startButton.gameObject.SetActive(false);
            if (levelText == null) Debug.Log("levelText is null");
            levelText.text = "LEVEL " + 
                             _storageManager.CurrentLevel;
        }
        private void StartGame()
        {
            if (_levelStarted) return;
            _signalBus.Fire(new LevelStartSignal());
            view.Hide();
        }
        
        


    }
}