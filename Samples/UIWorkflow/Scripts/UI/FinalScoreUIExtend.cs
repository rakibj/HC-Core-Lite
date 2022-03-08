using Zenject;
using Random = UnityEngine.Random;

namespace Rakib
{
    public class FinalScoreUIExtend : FinalScoreUI
    {
        [Inject] private StorageManager _storageManager;

        public override void Prepare()
        {
            base.Prepare();
            SetFinalEntity(_storageManager.CurrentEntity);
            SetFinalScore(_storageManager.CurrentScore);
        }
    }
}