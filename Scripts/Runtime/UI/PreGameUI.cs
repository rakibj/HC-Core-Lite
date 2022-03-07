using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rakib
{
    public class PreGameUI : MonoBehaviour
    {
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
        protected virtual void OnEnable()
        {
            if (levelStartPromptType == LevelStartPromptType.ButtonToStart) 
                startButton.onClick.AddListener(StartGame);
        }

        protected virtual void OnDisable()
        {
            if (levelStartPromptType == LevelStartPromptType.ButtonToStart) 
                startButton.onClick.RemoveListener(StartGame);
        }

        protected void LevelLoaded()
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


        protected void Debug_StartGame()
        {
            if (!_levelStarted)
            {
                view.Hide();
                _levelStarted = true;
            }
        }

        private protected void SetPreGameUI(int currentLevel)
        {
            if (levelStartPromptType != LevelStartPromptType.ButtonToStart) startButton.gameObject.SetActive(false);
            if (levelText == null) Debug.Log("levelText is null");
            levelText.text = "LEVEL " + currentLevel;
        }
        protected virtual void StartGame()
        {
            if (_levelStarted) return;
            view.Hide();
        }
        
        
    }
}