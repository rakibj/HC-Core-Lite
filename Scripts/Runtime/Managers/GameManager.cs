using UnityEngine;
using Zenject;

namespace Rakib
{
    public class GameManager : MonoBehaviour
    {
        private SignalBus _signalBus;
        private StorageManager _storageManager;
#if SDKS_INSTALLED
        private RateGame m_rateGame;
#endif
        private GameSettings _gameSettings;
        public GameSettings GameSettings => _gameSettings;
        private int _runningLevel;
        private bool _analyticsInitialized = false;

        public bool AnalyticsInitialized
        {
            get => _analyticsInitialized;
            set => _analyticsInitialized = value;
        }

        [Inject]
        private void Construct(SignalBus signalBus, StorageManager storageManager,
            GameSettings gameSettings)
        {
            _signalBus = signalBus;
            _storageManager = storageManager;
            _gameSettings = gameSettings;
        }

        private void Awake()
        {
            Application.targetFrameRate = 60;
            Time.timeScale = 1f;
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<LevelLoadSignal>(LevelLoaded);
            _signalBus.Subscribe<LevelStartSignal>(LevelStarted);
            _signalBus.Subscribe<LevelCompleteSignal>(LevelComplete);
            _signalBus.Subscribe<LevelFailSignal>(LevelFail);
            _signalBus.Subscribe<LevelLoadNextSignal>(LoadNext);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelLoadSignal>(LevelLoaded);
            _signalBus.Unsubscribe<LevelStartSignal>(LevelStarted);
            _signalBus.Unsubscribe<LevelCompleteSignal>(LevelComplete);
            _signalBus.Unsubscribe<LevelFailSignal>(LevelFail);
            _signalBus.Unsubscribe<LevelLoadNextSignal>(LoadNext);

        }

        private void LoadNext()
        {
            Debug.Log("GameManager: Level Load Next");
        }

        private void LevelLoaded(LevelLoadSignal signal)
        {
            Debug.Log("GameManager: Level Loaded");
            _storageManager.CurrentScore = 0;
            _signalBus.Fire(new CurrencyUpdateSignal());
        }
        
        private void LevelStarted(LevelStartSignal signal)
        {
            Debug.Log("GameManager: Level Started");
            _runningLevel = _storageManager.CurrentLevel;
        }

        private void LevelComplete(LevelCompleteSignal signal)
        {
            Debug.Log("GameManager: Level Complete");
            _storageManager.Currency += _storageManager.CurrentScore;
#if SDKS_INSTALLED
            RateGame.Instance.IncreaseCustomEvents();
            RateGame.Instance.ShowRatePopup();
#endif
        }
        private void LevelFail(LevelFailSignal signal)
        {
            Debug.Log("GameManager: Level Fail");
        }

        public void Sim_LevelLoadNext()
        {
            _signalBus.Fire(new LevelLoadNextSignal());
            Debug.Log("GameManager: Level Load Next");
        }
        
    }
}