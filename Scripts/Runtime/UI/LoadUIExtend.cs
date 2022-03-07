using Rakib;
using Zenject;

namespace RopeProjects
{
    public class LoadUIExtend : LoadUI
    {
        [Inject] private SignalBus _signalBus;
        private void OnEnable()
        {
            _signalBus.Subscribe<LevelLoadSignal>(HideView);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelLoadSignal>(HideView);
        }
        
    }
}