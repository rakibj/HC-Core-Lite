using System;
using System.Collections;
using System.Collections.Generic;
using Rakib;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Zenject;

namespace Rakib
{
    public class RestartButton : MonoBehaviour
    {
        private Button _restartButton;

        private void Awake()
        {
            _restartButton = GetComponent<Button>();
        }

        private void OnEnable()
        {
            _restartButton.onClick.AddListener(OnClick_Restart);
        }

        private void OnDisable()
        {
            _restartButton.onClick.RemoveListener(OnClick_Restart);
        }

        private void OnClick_Restart()
        {
            //_levelManager.LoadSame();
            LoadSame();
        }

        public void LoadSame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}