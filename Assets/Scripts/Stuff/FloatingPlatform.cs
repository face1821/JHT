using UnityEngine;

namespace Game.Stuff
{
    [RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
    public class FloatingPlatform : MonoBehaviour
    {
        [SerializeField] private bool _enableFloating;
        [SerializeField] private float _floatingSpeed;
        [SerializeField] private int _awakeFloatingDirection;

        private Rigidbody2D _body;
        private float _currentSpeed;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();

            if (_awakeFloatingDirection != 0)
            {
                _currentSpeed = _awakeFloatingDirection > 0 ? _floatingSpeed : -_floatingSpeed;
            }
        }

        private void FixedUpdate()
        {
            if (_enableFloating)
            {
                _body.velocity = new Vector2(0f, _currentSpeed);
            }
        }

        public void EnableFloating()
        {
            SetFloatingStop();

            _enableFloating = true;
            _body.isKinematic = true;
            _body.gravityScale = 0f;
        }

        public void DisableFloating()
        {
            SetFloatingStop();

            _enableFloating = false;
            _body.isKinematic = false;
            _body.gravityScale = 2f;
        }

        public void SetFloatingUp() { _currentSpeed = _floatingSpeed; }

        public void SetFloatingDown() { _currentSpeed = -_floatingSpeed; }

        public void SetFloatingStop() { _currentSpeed = 0f; }
    }
}