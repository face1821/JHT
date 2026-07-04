using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.Tool
{
    public class ToggleValueAwakeSync : MonoBehaviour
    {
        public string LoadValueName;
        public bool DefaultValue;

        private void Awake() { GetComponent<Toggle>().isOn = SaveSystem.Load(LoadValueName, DefaultValue); }
    }
}