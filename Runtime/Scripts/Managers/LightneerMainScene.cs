#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using Zenject;

namespace Rakib
{
    public class LightneerMainScene : MonoBehaviour
    {
        [Inject] private StorageManager _storageManager;
        [Inject] private GeneralSettings _generalSettings;
        [SerializeField] private Object _loaderConfigContainer;
        [SerializeField] private VideoPlayer _videoPlayer = null;
        [SerializeField] private LoadSceneMode _loadSceneMode = LoadSceneMode.Single;
        [SerializeField] private float _waitTimeAfterVideoHasEnded = 0.1f;
        [SerializeField] private float _forceLoadTime = 5f;
        [SerializeField] private bool showSplash = true;
        private int _sceneToLoad = 1;

        private AsyncOperation _sceneLoad = null;


        private void Start()
        {
            var maxLevel = _generalSettings.levelSettings.totalLevels;
            var level = _storageManager.CurrentLevel >= maxLevel
                ? Random.Range(1, maxLevel + 1)
                : _storageManager.CurrentLevel;
            _sceneToLoad = level;

#if UNITY_EDITOR
            if (showSplash)
            {
                _videoPlayer.loopPointReached += EndReached;
                _videoPlayer.Play();
                Invoke(nameof(ForceLoadScene), _forceLoadTime);
                return;
            }
            else
            {
                LoadScene(true);
                EndReached(_videoPlayer);
                Invoke(nameof(ForceLoadScene), _forceLoadTime);
                return;
            }
#endif
            if (showSplash)
            {
                _videoPlayer.loopPointReached += EndReached;
                _videoPlayer.Play();
                Invoke(nameof(ForceLoadScene), _forceLoadTime);
                return;
            }
            else
            {
                LoadScene(true);
                EndReached(_videoPlayer);
                Invoke(nameof(ForceLoadScene), _forceLoadTime);
                return;
            }
            
        }

        private void Update()
        {
            if (_sceneLoad == null && _videoPlayer.isPlaying)
            {
                LoadScene(true);
            }
        }

        private void LoadScene(bool loadAsync)
        {
            if (loadAsync)
            {
                _sceneLoad = SceneManager.LoadSceneAsync(_sceneToLoad, _loadSceneMode);
                _sceneLoad.allowSceneActivation = false;

                if (_loadSceneMode == LoadSceneMode.Additive)
                {
                    _sceneLoad.completed += operation =>
                    {
                        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_sceneToLoad));
                    };
                }
            }
            else
            {
                SceneManager.LoadScene(_sceneToLoad, _loadSceneMode);
            }
        }

        private void EndReached(VideoPlayer source)
        {
            Invoke(nameof(ActivateScene), _waitTimeAfterVideoHasEnded);
        }

        private void ActivateScene()
        {
            if (_sceneLoad != null)
            {
                _sceneLoad.allowSceneActivation = true;
                CancelInvoke(nameof(ForceLoadScene));
            }
        }

        private void ForceLoadScene()
        {
            if (_sceneLoad != null)
            {
                _sceneLoad.allowSceneActivation = true;
                CancelInvoke(nameof(ActivateScene));
            }
        }
    }
}
