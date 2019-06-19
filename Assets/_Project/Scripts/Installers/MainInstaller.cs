using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MainInstaller : MonoInstaller
    {
        [SerializeField] private MapPalette _defaultPalette;

        public override void InstallBindings()
        {
            Container.Bind<MapPalette>().WithId("DefaultPalette").FromInstance(_defaultPalette).AsSingle();
            Container.BindInterfacesAndSelfTo<MapBuilder>().AsSingle().NonLazy();
        }
    }
}