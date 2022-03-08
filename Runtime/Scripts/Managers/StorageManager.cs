using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Zenject;

namespace Rakib
{
    public class StorageManager
    {
        private SignalBus _signalBus;
        private const string LEVEL_KEY = "LEVELKEY";
        private const string HIGHSCORE_KEY = "HIGHSCOREKEY";
        private const string CURRENCY_KEY = "CURRENCYKEY";
        
        public int CurrentLevel
        {
            get => LoadOrCreateKeyInt(LEVEL_KEY, 1);
            set => PlayerPrefs.SetInt(LEVEL_KEY, value);
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
            get => LoadOrCreateKeyInt(CURRENCY_KEY, 0);
            set
            {
                PlayerPrefs.SetInt(CURRENCY_KEY, value);
            }
        }

        public int HighScore
        {
            get => LoadOrCreateKeyInt(HIGHSCORE_KEY, 1);
            set => PlayerPrefs.SetInt(HIGHSCORE_KEY, value);
        }

        [Inject]
        public void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        private int LoadOrCreateKeyInt(string key, int defaultValue = 1)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetInt(key);
            else
            {
                PlayerPrefs.SetInt(key, defaultValue);
                return defaultValue;
            }
        }
        

    }
}