using System;
using Zenject;

namespace Rakib
{
    public class PreGameUIExtend : PreGameUI
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;

        protected override void OnEnable()
        {
            base.OnEnable();
            _signalBus.Subscribe<LevelStartSignal>(Debug_StartGame);
            _signalBus.Subscribe<LevelLoadSignal>(LevelLoaded);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            _signalBus.Unsubscribe<LevelStartSignal>(Debug_StartGame);
            _signalBus.Unsubscribe<LevelLoadSignal>(LevelLoaded);
        }
        
        private void Start()
        {
            SetPreGameUI(_storageManager.CurrentLevel);
        }

        protected override void StartGame()
        {
            base.StartGame();
            _signalBus.Fire(new LevelStartSignal());
        }
    }
}