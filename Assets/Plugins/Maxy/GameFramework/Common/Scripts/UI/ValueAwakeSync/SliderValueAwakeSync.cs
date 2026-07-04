using System;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using UnityEngine.UI;

namespace Maxy.GameFramework.Common.Tool
{
    public class SliderValueAwakeSync : MonoBehaviour
    {
        public string LoadValueName;
        public float DefaultValue;

        private void Awake() { GetComponent<Slider>().value = SaveSystem.Load(LoadValueName, DefaultValue); }
    }
}