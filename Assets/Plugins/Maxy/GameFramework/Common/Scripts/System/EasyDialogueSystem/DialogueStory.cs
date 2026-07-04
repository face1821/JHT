using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    [CreateAssetMenu(fileName = "NewDialogueStory", menuName = "Maxy/GameFramework/Dialogue Story")]
    public class DialogueStory : ScriptableObject
    {
        public bool AutoPlay = true;
        public Sprite GlobalBackGround;
        
        [FoldoutGroup("Detail")]
        public bool EnableCharacterSpriteFadeEffect;
        
        public List<DialogueStoryContentUnit> contentList = new List<DialogueStoryContentUnit>();
    }
}