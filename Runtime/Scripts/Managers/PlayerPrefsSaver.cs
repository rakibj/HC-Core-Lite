using UnityEngine;

namespace Rakib
{
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