using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.System
{
    [CreateAssetMenu(fileName = "AchievementConfig", menuName = "Configs/Achievement")]
    public class AchievementConfig : ScriptableObject
    {
        [LabelText("成就列表")]
        public List<AchievementInfo> Achievements;
    }

    [Serializable]
    public class AchievementInfo
    {
        [LabelText("成就名")]
        public string Name;
        [TextArea, LabelText("成就介绍")]
        public string Description;
    }
}