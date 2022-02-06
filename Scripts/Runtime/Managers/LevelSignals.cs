using UnityEngine;

namespace Rakib
{
    public class LevelSignals
    {
    }
    
    public class LevelLoadSignal{}
    public class LevelStartSignal{}
    public class LevelCompleteSignal{}
    public class LevelFailSignal{}
    public class LevelLoadNextSignal{}
    public class LevelLoadSameSignal{}

    public class ProgressUpdateSignal
    {
        public float Progress;
    }
    public class ScoreUpdateSignal{}
    public class EntityUpdateSignal{}
    public class CurrencyUpdateSignal{}

    
}