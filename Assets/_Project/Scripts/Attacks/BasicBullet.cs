using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class BasicBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        private Vector2 _velocity;

        public void FixedUpdate()
        {
            _rigidbody.MovePosition(_rigidbody.position + _velocity * Time.fixedDeltaTime);
        }

        private void Reset(Vector3 position, Vector3 velocity)
        {
            transform.position = position;
            _velocity = velocity;
        }

        public class Pool : MonoMemoryPool<Vector3, Vector3, BasicBullet>
        {
            protected override void Reinitialize(Vector3 position, Vector3 velocity, BasicBullet bullet)
            {
                bullet.Reset(position, velocity);
            }
        }
    }
}
