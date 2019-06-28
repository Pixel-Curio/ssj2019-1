using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class BasicGun : IGun, ITickable
    {
        [Inject] private readonly BasicBullet.Pool _basicBulletPool;
        private const float CooldownTime = 0.25f;
        private float _waitTime;

        public void Tick()
        {
            _waitTime -= Time.deltaTime;
        }

        public void Fire(Vector3 position, Vector2 direction)
        {
            if (_waitTime > 0) return;

            _basicBulletPool.Spawn(position, direction);
            _waitTime = CooldownTime;
        }
    }
}
