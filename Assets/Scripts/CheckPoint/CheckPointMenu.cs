using System;
using System.Collections;
using Maxy.GameFramework.Common.System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.CheckPoint
{
    public class CheckPointMenu : MonoBehaviour
    {
        [SerializeField] private Transform _circle;
        [SerializeField] private Transform _judgeCircle;
        [SerializeField] private float _minSize;
        [SerializeField] private float _maxSize;
        [SerializeField] private float _scaleSpeed;
        [SerializeField] private float _maxOffset;


        public void Open() { gameObject.SetActive(true); }

        public void Close() { gameObject.SetActive(false); }

        private void OnEnable()
        {
            //随机一个初始光环大小
            var size = Random.Range(_minSize, _maxSize);

            _judgeCircle.localScale = Vector3.one * size;

            StartCoroutine(nameof(JudgeCircleHandle));
        }

        private IEnumerator JudgeCircleHandle()
        {
            while (true)
            {
                //光环持续变小       
                _judgeCircle.localScale -= Vector3.one * _scaleSpeed * Time.deltaTime;

                yield return null;

                //如果玩家在此刻按下了手指，就判定
                if (Input.touchCount > 0)
                {
                    if (Mathf.Abs(_judgeCircle.localScale.x - _circle.localScale.x) < _maxOffset)
                    {
                        //成功
                        GetSuccess();
                        break;
                    }
                    else
                    {
                        //失败
                        GetFailure();
                        break;
                    }
                }

                //如果太小了，结束，自动失败
                if (_circle.localScale.x - _judgeCircle.localScale.x > _maxOffset)
                {
                    GetFailure();
                    break;
                }
            }

            Close();
        }

        private void GetSuccess() { LogSystem.Log("QTE成功"); }

        private void GetFailure() { LogSystem.Log("QTE失败"); }
    }
}