using UnityEngine;
using Zenject;

namespace Rakib
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private bool showLogs = true;
        private SignalBus _signalBus;
        private StorageManager _storageManager;
        private int _runningLevel;

        [Inject]
        private void Construct(SignalBus signalBus, StorageManager storageManager)
        {
            _signalBus = signalBus;
            _storageManager = storageManager;
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
            Log("GameManager: Level Load Next");
        }

        private void LevelLoaded(LevelLoadSignal signal)
        {
            Log("GameManager: Level Loaded");
            _storageManager.CurrentScore = 0;
            _signalBus.Fire(new CurrencyUpdateSignal());
        }
        
        private void LevelStarted(LevelStartSignal signal)
        {
            _runningLevel = _storageManager.CurrentLevel;
            Log("GameManager: Level Started: " + _runningLevel);
        }

        private void LevelComplete(LevelCompleteSignal signal)
        {
            Log("GameManager: Level Complete: " +  + _runningLevel);
            _storageManager.Currency += _storageManager.CurrentScore;
        }
        private void LevelFail(LevelFailSignal signal)
        {
            Log("GameManager: Level Fail: " + _runningLevel);
        }

        private void Log(string content)
        {
            if(showLogs) Debug.Log(content);
        }

    }
}