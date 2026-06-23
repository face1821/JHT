using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Game.System
{
    public class AchievementWindow : MonoBehaviour
    {
        [SerializeField] private AchievementSystem _system;
        [SerializeField] private TextMeshProUGUI Title;
        [SerializeField] private TextMeshProUGUI Description;

        private Queue<Tuple<string, string>> _achievementsCache = new Queue<Tuple<string, string>>();
        private bool _isRunning;

        public void Show(string achievementName, string description)
        {
            if (_isRunning)
            {
                _achievementsCache.Enqueue(new Tuple<string, string>(achievementName, description));

                return;
            }

            StartToShow(achievementName, description);
        }

        private void StartToShow(string achievementName, string description)
        {
            _isRunning = true;
            
            //文本配置
            Title.text = achievementName;
            Description.text = description;
            
            //显示动画
            var rect = transform as RectTransform;
            rect!.anchoredPosition = _system.StartPos;
            rect.DOMoveY(_system.EndY, _system.Duration).SetEase(Ease.InQuad).SetDelay(3f).OnComplete(() =>
            {
                rect.DOMoveY(_system.StartPos.y, _system.Duration).SetEase(Ease.InQuad).OnComplete(() =>
                {
                    _isRunning = false;

                    //如果有缓存，继续显示
                    if (_achievementsCache.Count > 0)
                    {
                        var newAchievement = _achievementsCache.Dequeue();
                        StartToShow(newAchievement.Item1, newAchievement.Item2);
                    }
                });
            });
        }
    }
}