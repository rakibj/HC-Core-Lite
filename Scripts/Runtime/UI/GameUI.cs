using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Rakib
{
    public class GameUI : MonoBehaviour
    {
        [Inject] private StorageManager _storageManager;
        [Inject] private SignalBus _signalBus;
        [SerializeField] private TMP_Text currentLevel;
        [SerializeField] private TMP_Text nextLevel;
        [SerializeField] private Image progressor;
        [SerializeField] private UIView gameView;
        [SerializeField] private TMP_Text scoreText;
        private int _score = 0;
        public int Score
        {
            get => _score;
            set
            {
                _score = value;
                scoreText.text = _score.ToString();
            }
        }

        [Space(10)] [Header("Entity UI")] [SerializeField]
        private bool displayEntities = false;
        [SerializeField] private Transform entityPnl;
        [SerializeField] private Image entityImg;
        [SerializeField] private TMP_Text entityText;
        [SerializeField] private Image entityTickImg;
        private int _entity = 0;
        public int Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                entityText.text = _entity + "/" + _storageManager.TotalEntities;
                if (_entity == _storageManager.TotalEntities)
                {
                    entityText.enabled = false;
                    entityTickImg.enabled = true;
                }
                
            }
        }
        private void OnEnable()
        {
            _signalBus.Subscribe<ProgressUpdateSignal>(StatusUpdate);
            _signalBus.Subscribe<LevelStartSignal>(ShowUI);
            _signalBus.Subscribe<LevelCompleteSignal>(HideUI);
            _signalBus.Subscribe<ScoreUpdateSignal>(ScoreUpdate);
            _signalBus.Subscribe<EntityUpdateSignal>(EntityUpdate);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelStartSignal>(ShowUI);
            _signalBus.Unsubscribe<ProgressUpdateSignal>(StatusUpdate);
            _signalBus.Unsubscribe<LevelCompleteSignal>(HideUI);
            _signalBus.Unsubscribe<ScoreUpdateSignal>(ScoreUpdate);
            _signalBus.Unsubscribe<EntityUpdateSignal>(EntityUpdate);

        }

        private void Start()
        {
            if(entityTickImg)
                entityTickImg.enabled = false;
            entityPnl.gameObject.SetActive(displayEntities);
        }

        private void EntityUpdate(EntityUpdateSignal obj)
        {
            Entity = _storageManager.CurrentEntity;
        }


        private void ScoreUpdate(ScoreUpdateSignal scoreUpdateSignal)
        {
            StartCoroutine(UIUtils.IncrementScore(Score, _storageManager.CurrentScore, 0.25f,
                i => { Score = i; }));
        }

        private void ShowUI()
        {
            progressor.fillAmount = (0f);
            currentLevel.text = _storageManager.CurrentLevel.ToString();
            nextLevel.text = (_storageManager.CurrentLevel+1).ToString();
            gameView.Show();
            _storageManager.CurrentEntity = 0;
        }

        private void HideUI() => gameView.Hide();

        private void StatusUpdate(ProgressUpdateSignal progressUpdateSignal)
        {
            progressor.fillAmount = progressUpdateSignal.Progress;
        }
        
        
    }
}