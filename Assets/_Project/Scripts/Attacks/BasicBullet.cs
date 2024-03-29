﻿using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class BasicBullet : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private float _velocity = 1;
        [SerializeField] private float _lifeTime = 2;
        private Vector2 _direction;
        private Pool _pool;
        private float _timeAlive;
        private bool _isDespawning;

        public void FixedUpdate()
        {
            if ((_timeAlive += Time.deltaTime) > _lifeTime) Kill();
            _rigidbody.MovePosition(_rigidbody.position + _direction * _velocity * Time.fixedDeltaTime);
        }

        private void Reset(Vector3 position, Vector3 direction, Pool pool)
        {
            transform.position = position;
            _timeAlive = 0;
            _direction = direction;
            _pool = pool;
            _isDespawning = false;
        }

        private void Kill()
        {
            if(_isDespawning) return;

            _isDespawning = true;
            _pool.Despawn(this);
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            IDamagable target = col.gameObject.GetComponent<IDamagable>();
            target?.ReceiveDamage(1);
            Kill();
        }

        public class Pool : MonoMemoryPool<Vector3, Vector3, BasicBullet>
        {
            protected override void Reinitialize(Vector3 position, Vector3 direction, BasicBullet bullet)
            {
                bullet.Reset(position, direction.normalized, this);
            }
        }
    }
}
