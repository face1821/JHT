using DG.Tweening;
using Game.Player;
using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Spike : MonoBehaviour
    {
        [SerializeField] private DeadAreaTrap SpikeTrap;
        [SerializeField] private float _time;
        [SerializeField] private Vector2 _startPos;
        [SerializeField] private float _endX;

        private void OnEnable() { PlayerStateMachine.OnDead += OnPlayerDead; }

        private void OnDisable() { PlayerStateMachine.OnDead -= OnPlayerDead; }

        public void Close() { SpikeTrap.gameObject.SetActive(false); }

        public void StartMove()
        {
            SpikeTrap.transform.DOKill();
            SpikeTrap.gameObject.SetActive(true);
            SpikeTrap.transform.position = new Vector3(_startPos.x, _startPos.y);
            SpikeTrap.transform.DOMoveX(_endX, _time).SetEase(Ease.Linear);
        }

        public void TpToEnd()
        {
            SpikeTrap.transform.DOKill();
            SpikeTrap.gameObject.SetActive(true);
            SpikeTrap.transform.position = new Vector3(_endX, _startPos.y);
        }

        private void OnPlayerDead() { SpikeTrap.gameObject.SetActive(false); }
    }
}