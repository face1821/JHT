using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.Tool
{
    public class TextValueAwakeSync : MonoBehaviour
    {
        public string LoadValueName;
        public string DefaultValue;

        private void Awake() { GetComponent<TextMeshProUGUI>().text = ES3.Load(LoadValueName, defaultValue: DefaultValue); }
    }
}