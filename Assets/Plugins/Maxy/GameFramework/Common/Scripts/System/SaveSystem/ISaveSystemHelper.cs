using System;

namespace Maxy.GameFramework.Common.System

{
    public interface ISaveSystemHelper : ISystem
    {
        public void Clear();
        public void Save(string valueName, object value, SaveValueType type);
        public void Load(string valueName, SaveValueType type);
    }
}