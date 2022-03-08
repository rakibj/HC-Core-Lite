using UnityEngine;
using Zenject;

namespace Rakib
{
    public class StorageManager
    {
        private SignalBus _signalBus;
        private ISaver _saver;
        private const string LEVEL_KEY = "LEVELKEY";
        private const string HIGHSCORE_KEY = "HIGHSCOREKEY";
        private const string CURRENCY_KEY = "CURRENCYKEY";
        
        public int CurrentLevel
        {
            get => _saver.LoadOrCreateKeyInt(LEVEL_KEY, 1);
            set => _saver.SetInt(LEVEL_KEY, value);
        }

        private int _currentScore;

        public int CurrentScore
        {
            get => _currentScore;
            set
            {
                _currentScore = value;
                if (_currentScore > HighScore) HighScore = _currentScore;
                _signalBus.Fire(new ScoreUpdateSignal());
            }
        }
        
        private int _currentEntity = 0;
        public int CurrentEntity
        {
            get => _currentEntity;
            set
            {
                _currentEntity = value;
                _currentEntity = Mathf.Clamp(_currentEntity, 0, _totalEntities);
                _signalBus.Fire(new EntityUpdateSignal());
            }
        }
        private int _totalEntities = 99;
        public int TotalEntities
        {
            get => _totalEntities;
            set
            {
                _totalEntities = value;
                _signalBus.Fire(new EntityUpdateSignal());
            }
        }
        
        public int Currency
        {
            get => _saver.LoadOrCreateKeyInt(CURRENCY_KEY, 0);
            set
            {
                _saver.SetInt(CURRENCY_KEY, value);
            }
        }

        public int HighScore
        {
            get => _saver.LoadOrCreateKeyInt(HIGHSCORE_KEY, 1);
            set => _saver.SetInt(HIGHSCORE_KEY, value);
        }

        [Inject]
        public void Construct(SignalBus signalBus, ISaver saver)
        {
            _signalBus = signalBus;
            _saver = saver;
        }
        
        
        

    }
}