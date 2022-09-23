using UnityEngine;
using Zenject;

namespace Code.Settings
{
    public sealed class SettingsRepository : IInitializable
    {
        private const string SettingsPath = "data";
        
        public Data.Data Settings { get; private set; }
        
        public void Initialize()
        {
            var asset = Resources.Load(SettingsPath) as TextAsset;
            if (asset == null)
            {
                Debug.LogError($"Couldn't find settings file {SettingsPath} in Resources");
                return;
            }

            Settings = JsonUtility.FromJson<Data.Data>(asset.text);
        }
    }
}