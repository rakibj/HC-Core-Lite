using TMPro;
using UnityEngine;
using Zenject;

namespace Rakib
{
    public class CurrencyUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text currencyText;
        [SerializeField] private UIView currencyView;
        private int _currency;

        private int Currency
        {
            get => _currency;
            set
            {
                _currency = value;
                var curString = UIUtils.MakeKmb(_currency);
                currencyText.text = curString;
            }
        }

        protected void ShowCurrency()
        {
            currencyView.Show();
        }

        protected void HideCurrency()
        {
            currencyView.Hide();
        }

        protected void UpdateCurrencyInstantly(int currency)
        {
            currencyView.Show();
            Currency = currency;
        }

        protected void UpdateCurrency(int currency)
        {
            StartCoroutine(UIUtils.IncrementScore(Currency, currency, 0.1f, i => Currency = i));
        }

    }
}