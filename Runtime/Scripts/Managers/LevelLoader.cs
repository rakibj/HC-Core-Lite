using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Rakib
{
    public class LevelLoader : MonoBehaviour
    {
        private GeneralSettings _generalSettings;
        private SignalBus _signalBus;
        private StorageManager _storageManager;
        private int _currentLevel;
        
        [Inject]
        private void Construct(SignalBus signalBus, StorageManager storageManager, GeneralSettings generalSettings)
        {
            _signalBus = signalBus;
            _storageManager = storageManager;
            _generalSettings = generalSettings;
        }
        private void OnEnable()
        {
            _signalBus.Subscribe<LevelLoadNextSignal>(LevelComplete);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelLoadNextSignal>(LevelComplete);
        }

        private void Awake()
        {
            _currentLevel = _storageManager.CurrentLevel;
        }


        private void LevelComplete(LevelLoadNextSignal signal)
        {
            _storageManager.CurrentLevel++;
            LoadNextLevel(_storageManager.CurrentLevel);
        }

        public int GetSceneIndexToLoad()
        {
            return GetRefinedLevelToLoad(_currentLevel);
        }

        private void LoadNextLevel(int levelToLoad)
        {
            levelToLoad = GetRefinedLevelToLoad(levelToLoad);
            
            if (_generalSettings.levelSettings.levelLoadType == LevelSettings.LevelLoadType.SequenceThenRandom)
            {
                SceneManager.LoadScene(levelToLoad, LoadSceneMode.Single);////
            }
            else if (_generalSettings.levelSettings.levelLoadType == LevelSettings.LevelLoadType.LoopActiveScene)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
            }
        }

        private int GetRefinedLevelToLoad(int levelToLoad)
        {
            if (levelToLoad > _generalSettings.levelSettings.totalLevels)
                levelToLoad = Random.Range(_generalSettings.levelSettings.repeatFrom, _generalSettings.levelSettings.totalLevels + 1);

            return levelToLoad;
        }
    }
}