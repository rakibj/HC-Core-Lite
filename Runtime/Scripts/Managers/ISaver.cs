namespace Rakib
{
    public interface ISaver
    {
        void SetInt(string key, int value);
        int LoadOrCreateKeyInt(string key, int defaultValue);
    }
}