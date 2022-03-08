
using System.Collections;
using Zenject;

namespace Rakib
{
    public class WinUIExtend : WinUI
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;

        protected override void OnEnable()
        {
            base.OnEnable();
            _signalBus.Subscribe<LevelCompleteSignal>(OnLevelComplete);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _signalBus.Unsubscribe<LevelCompleteSignal>(OnLevelComplete);
        }

        private void OnLevelComplete() => OnLevelComplete(_storageManager.CurrentLevel);
        public override void WinButtonClick()
        {
            base.WinButtonClick();
            _signalBus.Fire(new LevelLoadNextSignal());
        }
    }
}