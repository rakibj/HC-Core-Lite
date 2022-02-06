using TMPro;
using UnityEngine;
using Zenject;

namespace Rakib
{
    public class CurrencyUI : MonoBehaviour
    {
        [Inject] private SignalBus _signalBus;
        [Inject] private StorageManager _storageManager;
        [Inject] private UIUtils _utils;
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private UIView currencyView;
        private int _currency;
        private Camera _camera;

        public int Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                var curString = _utils.MakeKmb(_currency);
                currencyText.text = curString;
            }
        }
        
        private void Awake()
        {
            _camera = Camera.main;
        }

        private void OnEnable()
        {
            UpdateCurrencyInstant();
            _signalBus.Subscribe<CurrencyUpdateSignal>(UpdateCurrency);
            _signalBus.Subscribe<LevelLoadSignal>(UpdateCurrencyInstant);
            _signalBus.Subscribe<LevelStartSignal>(HideCurrency);
            _signalBus.Subscribe<LevelCompleteSignal>(ShowCurrency);
            _signalBus.Subscribe<LevelFailSignal>(ShowCurrency);
        }

        private void OnDisable()
        {
            _signalBus.Unsubscribe<CurrencyUpdateSignal>(UpdateCurrency);
            _signalBus.Unsubscribe<LevelLoadSignal>(UpdateCurrencyInstant);
            _signalBus.Unsubscribe<LevelStartSignal>(HideCurrency);
            _signalBus.Unsubscribe<LevelCompleteSignal>(ShowCurrency);
            _signalBus.Unsubscribe<LevelFailSignal>(ShowCurrency);

        }

        private void ShowCurrency()
        {
            currencyView.Show();
        }

        private void HideCurrency()
        {
            currencyView.Hide();
        }

        private void UpdateCurrencyInstant()
        {
            currencyView.Show();
            Currency = _storageManager.Currency;
        }

        private void UpdateCurrency(CurrencyUpdateSignal currencyUpdateSignal)
        {
            StartCoroutine(_utils.IncrementScore(Currency, _storageManager.Currency, 0.1f, i => Currency = i));
        }

    }
}