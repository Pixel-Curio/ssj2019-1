using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class Bat : MonoBehaviour, ITileConnection, IDamagable
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Animator _animator;
        [SerializeField] private Vector2 _movementSpeed;
        [SerializeField] private Bar _healthBar;
        [SerializeField] private float _maxHealth = 5;
        [SerializeField] private float _healthBarWidth = 10;

        [Inject] private PlayableCharacter _target;

        private Vector3Int _location;
        private float _health;
        private Pool _pool;

        private void Awake()
        {
            _health = _maxHealth;
            UpdateHealthBar();
        }

        private void UpdateHealthBar() => _healthBar.SetWidth(_healthBarWidth * (_health / _maxHealth));

        private void FixedUpdate()
        {
            Vector2 offset = Vector2.zero;

            offset = (_target.transform.position - transform.position).normalized * _movementSpeed;

            _rigidbody.MovePosition(_rigidbody.position + offset * Time.fixedDeltaTime);

            _animator.SetFloat("HorizontalSpeed", offset.x);
        }

        public void ReceiveDamage(float damage)
        {
            _health -= damage;
            if(_health <= 0) _pool.Despawn(this);
            UpdateHealthBar();
        }

        private void Reset(Vector3 position, Pool pool)
        {
            transform.position = position;
            _health = _maxHealth;
            _pool = pool;
            UpdateHealthBar();
        }

        public class Pool : MonoMemoryPool<Vector3, Bat>
        {
            protected override void Reinitialize(Vector3 position, Bat bullet)
            {
                bullet.Reset(position, this);
            }
        }

        public void SetTileDependencies(Vector3Int location) => _location = location;
    }
}
