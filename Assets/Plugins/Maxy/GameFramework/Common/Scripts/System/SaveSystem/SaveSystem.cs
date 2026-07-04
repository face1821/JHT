using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public static class SaveSystem
    {
        private static Dictionary<string, object> _saveDataDict = new Dictionary<string, object>();

        private static string _path;

        static SaveSystem() { Init(); }

        public static async void Init()
        {
            _path = Path.Combine(Application.persistentDataPath, "SaveData.json");
            if (!File.Exists(_path))
                File.WriteAllText(_path, "{}");

            _saveDataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(await File.ReadAllTextAsync(_path));
        }

        public static void Flush()
        {
            var content = JsonConvert.SerializeObject(_saveDataDict);
            File.WriteAllText(_path, content);
        }

        public static void Clear()
        {
            _saveDataDict.Clear();

            Flush();
        }

        public static void Save<T>(string keyName, T value)
        {
            _saveDataDict[keyName] = value;
            Flush();
        }

        public static T Load<T>(string keyName, T defaultValue = default(T))
        {
            if (_saveDataDict.ContainsKey(keyName))
            {
                return (T)_saveDataDict[keyName];
            }

            return defaultValue;
        }

        public static void Delete(string keyName)
        {
            if (!_saveDataDict.ContainsKey(keyName)) return;

            _saveDataDict.Remove(keyName);
            Flush();
        }
    }
}