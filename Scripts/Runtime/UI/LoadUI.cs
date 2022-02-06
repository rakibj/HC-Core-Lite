using Rakib;
using UnityEngine;
using Zenject;

namespace RopeProjects
{
    public class LoadUI : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;

        [SerializeField] private UIView view;

        private void OnValidate()
        {
            view = GetComponent<UIView>();
        }

        private void Awake()
        {
            view.Show(true);
        }

        private void OnEnable()
        {
            _signalBus.Subscribe<LevelLoadSignal>(HideView);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelLoadSignal>(HideView);
        }

        private void HideView(LevelLoadSignal signal)
        {
            view.Hide();
        }
    }
}