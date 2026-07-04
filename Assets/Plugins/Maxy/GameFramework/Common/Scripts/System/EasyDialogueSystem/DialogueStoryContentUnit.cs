using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Maxy.GameFramework.Common.System
{
    [Serializable]
    public class DialogueStoryContentUnit
    {
        [ShowInInspector, PropertyOrder(-10)] public string Sentence => $"{CharacterName + ": "}{Content}";

        [FoldoutGroup("Detail", true)]
        public string CharacterName;
        [FoldoutGroup("Detail"), TextArea] public string Content;

        [FoldoutGroup("Detail"), Header("Display Config")]
        public bool EnableTypeWriterEffect = true;
        [FoldoutGroup("Detail"), ShowIf(nameof(EnableTypeWriterEffect))]
        public float TypeCountPerSecond = 10;
        [FoldoutGroup("Detail"), HideIf(nameof(EnableTypeWriterEffect))]
        public float Duration = 3f;
        [FoldoutGroup("Detail")]
        public float DelayAfterThis;

        [FoldoutGroup("Detail/Art")]
        public bool FollowLastBackGround;
        [FoldoutGroup("Detail/Art"), HideIf(nameof(FollowLastBackGround))]
        public Sprite BackGround;
        [Space]
        [FoldoutGroup("Detail/Art")]
        public bool FollowLastCharacterSprite;
        [FoldoutGroup("Detail/Art"), HideIf(nameof(FollowLastCharacterSprite))]
        public Sprite CharacterSprite;
        [FoldoutGroup("Detail/Art")]
        public bool enablePresetPosition = true;
        [FoldoutGroup("Detail/Art"), HideIf(nameof(enablePresetPosition))]
        public Vector2 ScreenPosition;
        [FoldoutGroup("Detail/Art"), ShowIf(nameof(enablePresetPosition))]
        public DialogCharaterPresetPosition PresetPosition = DialogCharaterPresetPosition.Middle;

        [FoldoutGroup("Detail/Audio")]
        public AudioClip EnterClip;
        [FoldoutGroup("Detail/Audio")]
        public AudioClip ExitClip;
    }

    [Serializable]
    public enum DialogCharaterPresetPosition
    {
        Left,
        Middle,
        Right,
    }
}