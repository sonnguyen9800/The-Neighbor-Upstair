using Godot;
using SisyphusFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCU.Data
{

    internal partial class PlayerPreferencesManager : BaseSingleton<PlayerPreferencesManager>
    {
        public Dictionary<object, object> Save(double volume)
        {
            return new Dictionary<object, object>()
        {
            { "MasterVolume", volume },
     
        };
        }

        private const string Path = "user://game.save";
        public enum ConfigKey
        {
            MasterAudioVolume,
        }
        public void SaveMasterVolume(double masterVolume)
        {
            SaveConfig(Path, ConfigKey.MasterAudioVolume, masterVolume);

        }

        public float LoadMasterVolume()
        {
            return LoadConfig(Path, ConfigKey.MasterAudioVolume);
        }

        public void SaveConfig(string filePath, ConfigKey key, double value)
        {
            var config = new ConfigFile();
            config.SetValue("UserSettings", key.ToString(), value);
            var err = config.Save(filePath);
            config.Save(filePath);

            if (err != Error.Ok)
            {
                GD.PrintErr($"Error saving config file: {filePath}");
            }
        }

        public float LoadConfig(string filePath, ConfigKey key)
        {
            var config = new ConfigFile();
            var err = config.Load(filePath);
            if (err == Error.Ok)
            {
                float volume = (float)config.GetValue("UserSettings", key.ToString(), 1.0f);
                return volume;
            }
            else
            {
                GD.PrintErr($"Error loading config file: {filePath}");
                return 1;
            }
        }
    }
}
