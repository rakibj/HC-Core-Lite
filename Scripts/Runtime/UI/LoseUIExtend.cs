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
            _signalBus.Subscribe<LevelFailSignal>(ShowViewAfter);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _signalBus.Unsubscribe<LevelFailSignal>(ShowViewAfter);
        }

        public override void ShowViewAfter()
        {
            if (_generalSettings.levelSettings.instantRestart)
            {
                LoseButtonClick();
                return;
            }
            base.ShowViewAfter();
        }

        protected override void LoseButtonClick()
        {
            base.LoseButtonClick();
            _signalBus.Fire(new LevelLoadSameSignal());
        }
    }
}