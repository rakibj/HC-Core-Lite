using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Rakib
{
    public class LevelLoader : MonoBehaviour
    {
        private GameSettings _gameSettings;
        private GeneralSettings _generalSettings;
        private SignalBus m_signalBus;
        private StorageManager m_storageManager;
        private int m_currentLevel;
        
        [Inject]
        private void Construct(GameSettings gameSettings, SignalBus signalBus, StorageManager storageManager, GeneralSettings generalSettings)
        {
            _gameSettings = gameSettings;
            m_signalBus = signalBus;
            m_storageManager = storageManager;
            _generalSettings = generalSettings;
        }
        private void OnEnable()
        {
            m_signalBus.Subscribe<LevelLoadNextSignal>(LevelComplete);
        }

        private void OnDisable()
        {
            m_signalBus.Unsubscribe<LevelLoadNextSignal>(LevelComplete);
        }

        private void Awake()
        {
            m_currentLevel = m_storageManager.CurrentLevel;
            //if (!_gameSettings.loadLevelsAutomatically)
                return;
            //LoadNextLevel(m_currentLevel);
        }


        private void LevelComplete(LevelLoadNextSignal signal)
        {
            m_storageManager.CurrentLevel++;
            LoadNextLevel(m_storageManager.CurrentLevel);
        }

        public int GetSceneIndexToLoad()
        {
            return GetRefinedLevelToLoad(m_currentLevel);
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