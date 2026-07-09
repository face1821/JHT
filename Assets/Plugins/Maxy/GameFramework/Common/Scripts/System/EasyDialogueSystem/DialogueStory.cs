using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    [CreateAssetMenu(fileName = "NewDialogueStory", menuName = "Maxy/GameFramework/Dialogue Story")]
    public class DialogueStory : ScriptableObject
    {
        [Header("Settings")]
        public Sprite GlobalBackGround;
        public bool AutoPlay = true;
        public bool FastForward = true;
        public bool EnableCharacterSpriteFadeEffect;

        [Header("Content")]
        public List<DialogueStoryContentUnit> contentList = new List<DialogueStoryContentUnit>();
    }
}