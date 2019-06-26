using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class BasicBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _velocity = 1;
        private Vector2 _direction;

        public void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _direction * _velocity * Time.fixedDeltaTime);
        }

        private void Reset(Vector3 position, Vector3 direction)
        {
            transform.position = position;
            _direction = direction;
        }

        public class Pool : MonoMemoryPool<Vector3, Vector3, BasicBullet>
        {
            protected override void Reinitialize(Vector3 position, Vector3 direction, BasicBullet bullet)
            {
                bullet.Reset(position, direction.normalized);
            }
        }
    }
}
