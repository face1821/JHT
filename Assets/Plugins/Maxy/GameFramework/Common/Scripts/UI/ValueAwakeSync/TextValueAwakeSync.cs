using Maxy.GameFramework.Common.System;
using TMPro;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    public class TextValueAwakeSync : MonoBehaviour
    {
        public string LoadValueName;
        public string DefaultValue;

        private void Awake() { GetComponent<TextMeshProUGUI>().text = SaveSystem.Load(LoadValueName, defaultValue: DefaultValue); }
    }
}