using System;
using UnityEngine;

namespace Game.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBody : MonoBehaviour
    {
        public Vector2 Velocity => _body.velocity;
        public float MoveSpeed => _moveSpeed;
        public float JumpSpeed => _jumpSpeed;

        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _jumpSpeed;

        private Rigidbody2D _body;
        private float _defaultGravityScale;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _defaultGravityScale = _body.gravityScale;
        }

        public void ZeroVelocity() { _body.velocity = Vector2.zero; }
        public void ZeroVelocityX() { _body.velocity = new Vector2(0f, _body.velocity.y); }
        public void SetVelocityX(float x) { _body.velocity = new Vector2(x, _body.velocity.y); }
        public void SetVelocityY(float y) { _body.velocity = new Vector2(_body.velocity.x, y); }

        public void SetPositionX(float x) { _body.position = new Vector2(x, _body.position.y); }

        public void SetPositionY(float y) { _body.position = new Vector2(_body.position.x, y); }

        public void SetGravityEnabled(bool toggle) { _body.gravityScale = toggle ? _defaultGravityScale : 0f; }

        public void SetFaceX(int faceDirectionX) { transform.localScale = new Vector3(faceDirectionX, 1, 1); }
    }
}