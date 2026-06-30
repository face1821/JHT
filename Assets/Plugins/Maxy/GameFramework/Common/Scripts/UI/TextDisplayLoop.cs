using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Maxy.GameFramework.Common.Tool
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextDisplayLoop : MonoBehaviour
    {
        public float DisplayInterval = 1f;
        public List<string> DisplayTextList;

        private TextMeshProUGUI _text;
        private int _index;

        private void Awake() { _text = GetComponent<TextMeshProUGUI>(); }

        private IEnumerator KeepLoop()
        {
            _index = 0;

            while (true)
            {
                _text.text = DisplayTextList[_index];
                _index = (_index + 1) % DisplayTextList.Count;

                if (!enabled)
                {
                    yield break;
                }

                yield return new WaitForSeconds(DisplayInterval);
            }
        }

        public void Play()
        {
            StopAllCoroutines();
            StartCoroutine(nameof(KeepLoop));
        }

        public void Stop() { StopAllCoroutines(); }
    }
}