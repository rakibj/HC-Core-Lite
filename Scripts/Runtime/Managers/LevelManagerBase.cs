using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Rakib
{
    public class LevelManagerBase : MonoBehaviour
    {
        [Inject] protected SignalBus SignalBus;
        [Inject] protected StorageManager _storageManager;
        [SerializeField] private GameState _gameState;
        protected int RunningLevel;
        private const string GameDebugMethods = "GameDebugMethods";

        protected virtual void Start()
        {
            Time.timeScale = 1f;
            RunningLevel = _storageManager.CurrentLevel;
            SignalBus.Fire(new LevelLoadSignal());
        }

        protected virtual void OnEnable()
        {
            //SignalBus.Subscribe<LevelLoadNextSignal>(LevelLoad);
            SignalBus.Subscribe<LevelStartSignal>(LevelStart);
            SignalBus.Subscribe<LevelCompleteSignal>(LevelComplete);
            SignalBus.Subscribe<LevelFailSignal>(LevelFail);
            SignalBus.Subscribe<LevelLoadNextSignal>(LoadNext);
            SignalBus.Subscribe<LevelLoadSameSignal>(LoadSame);
        }

        protected virtual void OnDisable()
        {
            //SignalBus.Unsubscribe<LevelLoadNextSignal>(LevelLoad);
            SignalBus.Unsubscribe<LevelStartSignal>(LevelStart);
            SignalBus.Unsubscribe<LevelCompleteSignal>(LevelComplete);
            SignalBus.Unsubscribe<LevelFailSignal>(LevelFail);
            SignalBus.Unsubscribe<LevelLoadNextSignal>(LoadNext);
            SignalBus.Unsubscribe<LevelLoadSameSignal>(LoadSame);
        }
        private void LoadNext()
        {
            //_storageManager.CurrentLevel++;
        }

        public void LoadSame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        protected virtual void LevelLoadComplete()
        {
            _gameState = GameState.WaitingToStart;
            SignalBus.Fire(new LevelLoadSignal());
        }

        /// <summary>
        /// Called when LevelStartSignal is fired
        /// </summary>
        protected virtual void LevelStart()
        {
            _gameState = GameState.Running;
        }

        /// <summary>
        /// Called when LevelCompleteSignal is fired
        /// </summary>
        protected virtual void LevelComplete()
        {
            _gameState = GameState.Complete;
        }
        /// <summary>
        /// Called when LevelFailSignal is fired
        /// </summary>
        protected virtual void LevelFail()
        {
            _gameState = GameState.Fail;
            
        }

        /// <summary>
        /// Call this to fire the LevelLoadNextSignal and load the next level
        /// </summary>
        private protected virtual void Debug_LoadNextLevel()
        {
            SignalBus.Fire(new LevelLoadNextSignal());
        }

        protected virtual void Update()
        {
            #if UNITY_EDITOR
            if(Input.GetKeyDown(KeyCode.E)) Debug_LoadNextLevel();
            if(Input.GetKeyDown(KeyCode.R)) LoadSame();
            #endif
        }
        
        [ContextMenu("DebugMethods/Sim_LevelStart")]
        private void Sim_LevelStart()
        {
            SignalBus.Fire(new LevelStartSignal());
        }

        [ContextMenu("DebugMethods/Sim_LevelComplete")]
        public void Sim_LevelComplete()
        {
            SignalBus.Fire(new LevelCompleteSignal());
        }

        [ContextMenu("DebugMethods/Sim_LevelFail")]
        public void Sim_LevelFail()
        {
            SignalBus.Fire(new LevelFailSignal());
        }

        [ContextMenu("DebugMethods/Sim_LoadNextLevel")]
        private void Sim_LoadNextLevel()
        {
            SignalBus.Fire(new LevelLoadNextSignal());
        }

        public void Sim_IncreaseScore(int increment = 5)
        {
            _storageManager.CurrentScore += increment;
        }
        public void UpdateProgress(float increment = 0.5f)
        {
            SignalBus.Fire(new ProgressUpdateSignal() { Progress = increment});
        }

        public void SetEntities(int entitiesCount = 10)
        {
            _storageManager.TotalEntities = entitiesCount;
        }
        public void AddEntity()
        {
            _storageManager.CurrentEntity++;
        }

    }
    
    public enum GameState
    {
        WaitingToStart,
        Running,
        Complete,
        Fail
    }
}