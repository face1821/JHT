using System;
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

        public static void Init()
        {
            _path = Path.Combine(Application.persistentDataPath, "SaveData.json");
            if (!File.Exists(_path))
                File.WriteAllText(_path, "{}");

            _saveDataDict = JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(_path));
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
            MLogger.LogWarning($"存储系统：写入 {keyName} = {value}");
            _saveDataDict[keyName] = value;
            Flush();
        }

        public static T Load<T>(string keyName, T defaultValue = default(T))
        {
            if (_saveDataDict.ContainsKey(keyName))
            {
                // 通用安全类型转换，兼容 long/double/string 转目标类型
                MLogger.LogWarning($"存储系统：读取 {keyName} = {(T)Convert.ChangeType(_saveDataDict[keyName], typeof(T))}");
                return (T)Convert.ChangeType(_saveDataDict[keyName], typeof(T));
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