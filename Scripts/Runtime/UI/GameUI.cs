using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rakib
{
    public class GameUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currentLevel;
        [SerializeField] private TMP_Text nextLevel;
        [SerializeField] private Image progressor;
        [SerializeField] private UIView gameView;
        [SerializeField] private TMP_Text scoreText;
        private int _score = 0;

        [Space(10)] [Header("Entity UI")] [SerializeField]
        private bool displayEntities = false;
        [SerializeField] private Transform entityPnl;
        [SerializeField] private TMP_Text entityText;
        [SerializeField] private Image entityTickImg;
        private int _entity = 0;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                scoreText.text = _score.ToString();
            }
        }


        private void Start()
        {
            if(entityTickImg)
                entityTickImg.enabled = false;
            entityPnl.gameObject.SetActive(displayEntities);
        }

        protected void EntityUpdate(int currentEntity, int totalEntities)
        {
            entityText.text = currentEntity + "/" + totalEntities;
            if (_entity == totalEntities)
            {
                entityText.enabled = false;
                entityTickImg.enabled = true;
            }
        }
        protected void ScoreUpdate(int currentScore)
        {
            StartCoroutine(UIUtils.IncrementScore(Score, currentScore, 0.25f,
                i => { Score = i; }));
        }
        protected void ShowUI(int currentLevelNumber)
        {
            progressor.fillAmount = (0f);
            currentLevel.text = currentLevelNumber.ToString();
            nextLevel.text = (currentLevelNumber+1).ToString();
            gameView.Show();
        }

        protected void HideUI() => gameView.Hide();

        protected void StatusUpdate(float progress)
        {
            progressor.fillAmount = progress;
        }
        
    }
}