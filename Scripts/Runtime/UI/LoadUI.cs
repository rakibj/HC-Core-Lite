using Rakib;
using UnityEngine;

namespace RopeProjects
{
    public class LoadUI : MonoBehaviour
    {
        [SerializeField] private UIView view;

        private void OnValidate()
        {
            view = GetComponent<UIView>();
        }

        private void Awake()
        {
            view.Show(true);
        }

        protected void HideView()
        {
            view.Hide();
        }
    }
}