
using UnityEngine;
using UnityEngine.Analytics;
using Zenject;

namespace Rakib
{
    [CreateAssetMenu(fileName = "GameInstaller", menuName = "Installers/GameInstaller")]
    public class GameInstaller : ScriptableObjectInstaller<GameInstaller>
    {
        public GameSettings gameSettings;
        public GeneralSettings generalSettings;

        
        public override void InstallBindings()
        {
            var storageManager = new StorageManager();
            var hapticsManager = new HapticsManager();
            
            SignalBusInstaller.Install(Container);
            
            Container.BindInstance(gameSettings);
            Container.BindInstance(generalSettings);
            Container.Bind<LevelLoader>().FromComponentInHierarchy().AsSingle();
            Container.BindInstance(storageManager).AsSingle();
            Container.QueueForInject(storageManager);
            Container.BindInstance(hapticsManager).AsSingle();
            Container.QueueForInject(hapticsManager);
            Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
            //Container.Bind<HapticManager>().FromInstance(new HapticManager(true)); // boolean to determine default value whether haptics are enabled for player or not
            
            //Signals
            Container.DeclareSignal<LevelLoadSignal>();
            Container.DeclareSignal<LevelStartSignal>();
            Container.DeclareSignal<LevelCompleteSignal>();
            Container.DeclareSignal<LevelFailSignal>();
            Container.DeclareSignal<LevelLoadNextSignal>();
            Container.DeclareSignal<LevelLoadSameSignal>();
            
            Container.DeclareSignal<ProgressUpdateSignal>().OptionalSubscriber();
            Container.DeclareSignal<ScoreUpdateSignal>().OptionalSubscriber();
            Container.DeclareSignal<CurrencyUpdateSignal>().OptionalSubscriber();
            Container.DeclareSignal<EntityUpdateSignal>().OptionalSubscriber();
        }
    }
}