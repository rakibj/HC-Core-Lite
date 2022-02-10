using System;
using TMPro;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace Rakib
{
    public class FinalScoreUI : MonoBehaviour, IWinUI
    {
        [Inject] private UIUtils _utils;
        [Inject] private StorageManager _storageManager;
        [SerializeField] private TMP_Text entityText;
        [SerializeField] private TMP_Text scoreText;
        
        private Vector3 _startPosition;
        private int _finalEntityScore;
        private int _finalCurrentScore;
        private int _entityScore;
        public int EntityScore
        {
            get => _entityScore;
            set
            {
                _entityScore = value;
                entityText.text = "x" + _entityScore.ToString();
            }
        }

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                scoreText.text = _currentScore.ToString();
            }
        }

        private int _currentScore;

        public void Prepare()
        {
            EntityScore = 0;
            CurrentScore = 0;
            _finalEntityScore = _storageManager.CurrentEntity;
            _finalCurrentScore = _storageManager.CurrentScore;
            //transform.localScale = Vector3.zero;
        }
        
        public void Show(Action onComplete = null)
        {
            StartCoroutine(_utils.IncrementScore(EntityScore, _finalEntityScore, 0.5f, i => EntityScore = i));   
            StartCoroutine(_utils.IncrementScore(CurrentScore, _finalCurrentScore, 0.25f, i => CurrentScore = i,
                    () =>
                    {
                        onComplete?.Invoke();
                    }));  
        }

        
    }
}