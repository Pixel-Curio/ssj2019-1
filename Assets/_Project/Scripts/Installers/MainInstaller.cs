using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Map _defaultMap;
        [SerializeField] private GameObject _lightExplosion;
        [SerializeField] private GameObject _basicBullet;

        public override void InstallBindings()
        {
            InstallMaps();
            InstallWeapons();
            InstallEffects();
        }

        private void InstallEffects()
        {
            Container.BindMemoryPool<PlaceableEffect, PlaceableEffect.Pool>()
                .WithId("LightExplosion")
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_lightExplosion)
                .UnderTransformGroup("Effects");
        }

        private void InstallMaps()
        {
            Container.BindInterfacesAndSelfTo<MapBuilder>().AsSingle().NonLazy();
            Container.Bind<Map>().WithId("DefaultMap").FromInstance(_defaultMap).AsSingle();
        }

        private void InstallWeapons()
        {
            Container.Bind(typeof(ITickable), typeof(IGun)).To<BasicGun>().AsSingle();
            Container.BindMemoryPool<BasicBullet, BasicBullet.Pool>()
                .WithInitialSize(5)
                .FromComponentInNewPrefab(_basicBullet)
                .UnderTransformGroup("Projectiles");
        }
    }
}