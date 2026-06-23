using System;
using Maxy.GameFramework.Common.System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.System
{
    public class AchievementSystem : System<AchievementSystem>, IAchievementSystem
    {
        public Vector2 StartPos => _startPos;
        public float EndY => _endY;
        public float Duration => _duration;

        [Header("成就窗口参数")]
        [SerializeField] private AchievementWindow _window;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private float _endY;
        [SerializeField] private float _duration;

        private AchievementConfig _config;
        
        public override void Init()
        {
            base.Init();

            _config = Resources.Load<AchievementConfig>("Datas/AchievementConfig");
        }

        public bool HasUnlockedAchievement(string achievementName) { return ES3.Load($"Achievement-{achievementName}", false); }

        [Button]
        public void UnlockAchievement(string achievementName)
        {
            foreach (var item in _config.Achievements)
            {
                if (item.Name != achievementName) continue;

                //找到成就，显示
                _window.Show(item.Name, item.Description);
                break;
            }
        }
    }
}