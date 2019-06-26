using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    [RequireComponent(typeof(ParticleSystem))]
    public class PlaceableEffect : MonoBehaviour
    {
        private ParticleSystem _particleSystem;
        private Pool _pool;

        private void OnEnable()
        {
            if (!_particleSystem) _particleSystem = GetComponent<ParticleSystem>();
        }

        public void OnParticleSystemStopped()
        {
            _pool.Despawn(this);
        }

        private void Reset(Vector3 position)
        {
            transform.position = position;
        }

        public class Pool : MonoMemoryPool<Vector3, PlaceableEffect>
        {
            protected override void OnSpawned(PlaceableEffect effect)
            {
                effect._pool = this;
                base.OnSpawned(effect);
            }

            protected override void Reinitialize(Vector3 position, PlaceableEffect effect)
            {
                effect.Reset(position);
            }
        }
    }
}
