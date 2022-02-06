using UnityEngine;
using Zenject;

namespace Rakib
{
    [CreateAssetMenu(fileName = "UIInstaller", menuName = "Installers/UIInstaller")]
    public class UIInstaller : ScriptableObjectInstaller<UIInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<UIUtils>().FromComponentInHierarchy().AsSingle();
        }
    }
}