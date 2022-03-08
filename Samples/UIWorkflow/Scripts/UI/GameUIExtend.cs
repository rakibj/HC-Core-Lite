using Zenject;

namespace Rakib
{
    public class GameUIExtend : GameUI
    {
        [Inject] private StorageManager _storageManager;
        [Inject] private SignalBus _signalBus;
        
        private void OnEnable()
        {
            _signalBus.Subscribe<ProgressUpdateSignal>(StatusUpdate);
            _signalBus.Subscribe<LevelStartSignal>(ShowUI);
            _signalBus.Subscribe<LevelCompleteSignal>(HideUI);
            _signalBus.Subscribe<ScoreUpdateSignal>(ScoreUpdate);
            _signalBus.Subscribe<EntityUpdateSignal>(EntityUpdate);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<LevelStartSignal>(ShowUI);
            _signalBus.Unsubscribe<ProgressUpdateSignal>(StatusUpdate);
            _signalBus.Unsubscribe<LevelCompleteSignal>(HideUI);
            _signalBus.Unsubscribe<ScoreUpdateSignal>(ScoreUpdate);
            _signalBus.Unsubscribe<EntityUpdateSignal>(EntityUpdate);
        }

        private void ShowUI()
        {
            ShowUI(_storageManager.CurrentLevel);
            _storageManager.CurrentEntity = 0;
        }

        private void StatusUpdate(ProgressUpdateSignal signal) => StatusUpdate(signal.Progress);
        private void ScoreUpdate() => ScoreUpdate(_storageManager.CurrentScore);
        private void EntityUpdate() => EntityUpdate(_storageManager.CurrentEntity, _storageManager.TotalEntities);
    }
}