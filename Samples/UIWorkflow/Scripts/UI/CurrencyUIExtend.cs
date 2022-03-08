using UnityEngine;
using Zenject;

namespace Rakib.UI
{
    public class CurrencyUIExtend : CurrencyUI
    {
        [Inject] private StorageManager _storageManager;
        [Inject] private SignalBus _signalBus;
        
        private void OnEnable()
        {
            UpdateCurrencyInstantly();
            _signalBus.Subscribe<CurrencyUpdateSignal>(UpdateCurrency);
            _signalBus.Subscribe<LevelLoadSignal>(UpdateCurrencyInstantly);
            _signalBus.Subscribe<LevelStartSignal>(HideCurrency);
            _signalBus.Subscribe<LevelCompleteSignal>(ShowCurrency);
            _signalBus.Subscribe<LevelFailSignal>(ShowCurrency);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<CurrencyUpdateSignal>(UpdateCurrency);
            _signalBus.Unsubscribe<LevelLoadSignal>(UpdateCurrencyInstantly);
            _signalBus.Unsubscribe<LevelStartSignal>(HideCurrency);
            _signalBus.Unsubscribe<LevelCompleteSignal>(ShowCurrency);
            _signalBus.Unsubscribe<LevelFailSignal>(ShowCurrency);
        }

        private void UpdateCurrency()
        {
            UpdateCurrency(_storageManager.Currency);
        }
        private void UpdateCurrencyInstantly()
        {
            UpdateCurrencyInstantly(_storageManager.Currency);
        }
    }
}