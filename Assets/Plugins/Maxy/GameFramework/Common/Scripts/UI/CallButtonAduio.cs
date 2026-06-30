using Maxy.GameFramework.Common.Events;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Maxy.GameFramework.Common.Tool
{
    
    public class ButtonAduio : MonoBehaviour, IPointerClickHandler
    {
        public string ButtonAudioName;

        public void OnPointerClick(PointerEventData eventData)
        {
            EventBus.Publish(new PlayButtonAudioEvent(ButtonAudioName));
        }
    }
}