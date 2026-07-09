using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    public class SaveSystemHelper : System<SaveSystemHelper>, ISaveSystemHelper
    {
        [SerializeField, ReadOnly] private string LoadedValue;

        [Button]
        public void Clear() { SaveSystem.Clear(); }

        [Button]
        public void Save(string valueName, object value, SaveValueType type)
        {
            if (string.IsNullOrEmpty(valueName)) return;
            if (value == null) return;

            switch (type)
            {
                case SaveValueType.Int:
                    SaveSystem.Save(valueName, (int)value);
                    break;
                case SaveValueType.Float:
                    SaveSystem.Save(valueName, (float)value);
                    break;
                case SaveValueType.String:
                    SaveSystem.Save(valueName, (string)value);
                    break;
                case SaveValueType.Bool:
                    SaveSystem.Save(valueName, (bool)value);
                    break;
                case SaveValueType.Vector2:
                    SaveSystem.Save(valueName, (Vector2)value);
                    break;
                case SaveValueType.Vector3:
                    SaveSystem.Save(valueName, (Vector3)value);
                    break;
                case SaveValueType.Quaternion:
                    SaveSystem.Save(valueName, (Quaternion)value);
                    break;
                case SaveValueType.Color:
                    SaveSystem.Save(valueName, (Color)value);
                    break;
            }
        }

        [Button]
        public void Load(string valueName, SaveValueType type)
        {
            if (string.IsNullOrEmpty(valueName)) return;

            switch (type)
            {
                case SaveValueType.Int:
                    LoadedValue = SaveSystem.Load(valueName, default(int)).ToString();
                    break;
                case SaveValueType.Float:
                    LoadedValue = SaveSystem.Load(valueName, default(float).ToString("F6"));
                    break;
                case SaveValueType.String:
                    LoadedValue = SaveSystem.Load(valueName, default(string));
                    break;
                case SaveValueType.Bool:
                    LoadedValue = SaveSystem.Load(valueName, default(bool)).ToString();
                    break;
                case SaveValueType.Vector2:
                    LoadedValue = SaveSystem.Load(valueName, default(Vector2)).ToString();
                    break;
                case SaveValueType.Vector3:
                    LoadedValue = SaveSystem.Load(valueName, default(Vector3)).ToString();
                    break;
                case SaveValueType.Quaternion:
                    LoadedValue = SaveSystem.Load(valueName, default(Quaternion)).ToString();
                    break;
                case SaveValueType.Color:
                    LoadedValue = SaveSystem.Load(valueName, default(Color)).ToString();
                    break;
            }
        }
    }

    public enum SaveValueType
    {
        Int,
        Float,
        String,
        Bool,
        Vector2,
        Vector3,
        Quaternion,
        Color,
    }
}