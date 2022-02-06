using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace Rakib
{
    public class HapticsManager
    {
        [Inject] private GameSettings _gameSettings;
        public enum Haptic
        {
            Default, Selection, Success, Warning, Failure, Light, Medium, Heavy, Rigid, Soft, None
        }
        //[Inject] private HapticManager _hapticManager;

        public HapticsManager()
        {
        }

        public void PlayHaptic(Haptic hapticType)
        {
            //TODO implement haptics here
        }
    }
}