using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private Map _defaultMap;

        public override void InstallBindings()
        {
            Container.Bind<Map>().WithId("DefaultMap").FromInstance(_defaultMap).AsSingle();
            Container.BindInterfacesAndSelfTo<MapBuilder>().AsSingle().NonLazy();
        }
    }
}