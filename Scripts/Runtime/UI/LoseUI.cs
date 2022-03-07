using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rakib
{
    public class LoseUI : MonoBehaviour
    {
        [SerializeField] private float appearDelay = 0.5f;
        [SerializeField] private UIView view;
        [SerializeField] private TMP_Text levelCompleteText;
        [SerializeField] private Button loadNextButton;

        private void OnValidate()
        {
            view = GetComponent<UIView>();
            loadNextButton = GetComponentInChildren<Button>();
        }

        protected virtual void OnEnable()
        {
            loadNextButton.onClick.AddListener(LoseButtonClick);
        }

        protected virtual void OnDisable()
        {
            loadNextButton.onClick.RemoveListener(LoseButtonClick);
        }

        public virtual void ShowViewAfter()
        {
            
            Invoke(nameof(ShowView), appearDelay);
        }

        private void ShowView(int currentLevel)
        {
            levelCompleteText.text = "LEVEL " + currentLevel + " FAIL";
            view.Show();
        }

        protected virtual void LoseButtonClick()
        {
        }
    }
}