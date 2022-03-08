using UnityEngine;

namespace Rakib
{
    public interface ISaver
    {
        void SetInt(string key, int value);
        int LoadOrCreateKeyInt(string key, int defaultValue);
    }

    public class PlayerPrefsSaver : ISaver
    {
        public void SetInt(string key, int value)
        {
            PlayerPrefs.SetInt(key, value);
        }
        
        public int LoadOrCreateKeyInt(string key, int defaultValue = 1)
        {
            if (PlayerPrefs.HasKey(key))
                return PlayerPrefs.GetInt(key);
            else
            {
                PlayerPrefs.SetInt(key, defaultValue);
                return defaultValue;
            }
        }
    }
}