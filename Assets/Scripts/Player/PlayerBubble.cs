using Maxy.GameFramework.Common.Tool;
using TMPro;
using UnityEngine;

namespace Game.Player
{
    public class PlayerBubble : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private OverlayFadeEffect _bubble;

        public void Speak(string content)
        {
            _text.text = content;
            _bubble.PlayFadeOutAndIn(0.5f, 2f, 0.5f);
        }
    }
}