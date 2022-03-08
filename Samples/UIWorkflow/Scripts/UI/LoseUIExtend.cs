using Zenject;

namespace Rakib
{
    public class LoseUIExtend : LoseUI
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private GeneralSettings _generalSettings;
        [Inject] private StorageManager _storageManager;

        protected override void OnEnable()
        {
            base.OnEnable();
            _signalBus.Subscribe<LevelFailSignal>(ShowViewDelayed);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _signalBus.Unsubscribe<LevelFailSignal>(ShowViewDelayed);
        }

        private void ShowViewDelayed()
        {
            if (_generalSettings.levelSettings.instantRestart)
            {
                LoseButtonClick();
                return;
            }
            base.ShowViewDelayed(_storageManager.CurrentLevel);
        }

        protected override void LoseButtonClick()
        {
            base.LoseButtonClick();
            _signalBus.Fire(new LevelLoadSameSignal());
        }
    }
}