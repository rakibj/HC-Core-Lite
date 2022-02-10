using System;

namespace Rakib
{
    public interface IWinUI
    {
        void Prepare();
        void Show(Action onComplete = null);
    }
}