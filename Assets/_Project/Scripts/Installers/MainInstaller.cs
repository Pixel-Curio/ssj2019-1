using UnityEngine;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Map _defaultMap;
        [SerializeField] private GameObject _destructablePrefab;
        [SerializeField] private GameObject _lightExplosion;

        public override void InstallBindings()
        {
            Container.Bind<Map>().WithId("DefaultMap").FromInstance(_defaultMap).AsSingle();
            Container.BindInterfacesAndSelfTo<MapBuilder>().AsSingle().NonLazy();
            Container.BindMemoryPool<PlaceableEffect, PlaceableEffect.Pool>()
                .WithId("LightExplosion")
                .WithInitialSize(2)
                .FromComponentInNewPrefab(_lightExplosion)
                .UnderTransformGroup("Effects");
        }
    }
}