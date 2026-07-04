using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    [Serializable]
    public class DialogueStoryContentUnit
    {
        [Title("@GetTitle()")]
        public string CharacterName;
        [TextArea] public string Content;

        [Header("Display Config")]
        public bool EnableTypeWriterEffect = true;
        [ShowIf(nameof(EnableTypeWriterEffect))]
        public float TypeCountPerSecond = 10;
        [HideIf(nameof(EnableTypeWriterEffect))]
        public float Duration = 3f;
        public float DelayAfterThis;

        [FoldoutGroup("Art")]
        public bool FollowLastBackGround;
        [FoldoutGroup("Art"), HideIf(nameof(FollowLastBackGround))]
        public Sprite BackGround;
        [Space]
        [FoldoutGroup("Art")]
        public bool FollowLastCharacterSprite;
        [FoldoutGroup("Art"), HideIf(nameof(FollowLastCharacterSprite))]
        public Sprite CharacterSprite;
        [FoldoutGroup("Art")]
        public bool enablePresetPosition = true;
        [FoldoutGroup("Art"), HideIf(nameof(enablePresetPosition))]
        public Vector2 ScreenPosition;
        [FoldoutGroup("Art"), ShowIf(nameof(enablePresetPosition))]
        public DialogCharaterPresetPosition PresetPosition = DialogCharaterPresetPosition.Middle;

        [FoldoutGroup("Audio")]
        public AudioClip EnterClip;
        [FoldoutGroup("Audio")]
        public AudioClip ExitClip;

        public string GetTitle() => $"{CharacterName}: {Content}";
    }

    [Serializable]
    public enum DialogCharaterPresetPosition
    {
        Left,
        Middle,
        Right,
    }
}