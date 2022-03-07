using System;
using TMPro;
using UnityEngine;

namespace Rakib
{
    public class FinalScoreUI : MonoBehaviour, IWinUI
    {
        [SerializeField] private TMP_Text entityText;
        [SerializeField] private TMP_Text scoreText;
        
        private Vector3 _startPosition;
        private int _finalEntityScore;
        private int _finalCurrentScore;
        private int _entityScore;

        private int EntityScore
        {
            get => _entityScore;
            set
            {
                _entityScore = value;
                entityText.text = "x" + _entityScore.ToString();
            }
        }

        private int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                scoreText.text = _currentScore.ToString();
            }
        }

        private int _currentScore;

        protected void SetFinalEntity(int finalEntity) => _finalEntityScore = finalEntity;
        protected void SetFinalScore(int finalScore) => _finalCurrentScore = finalScore;
        public virtual void Prepare()
        {
            EntityScore = 0;
            CurrentScore = 0;
        }
        
        public void Show(Action onComplete = null)
        {
            StartCoroutine(UIUtils.IncrementScore(EntityScore, _finalEntityScore, 0.5f, i => EntityScore = i));   
            StartCoroutine(UIUtils.IncrementScore(CurrentScore, _finalCurrentScore, 0.25f, i => CurrentScore = i,
                () =>
                {
                    onComplete?.Invoke();
                }));  
        }

    }
}