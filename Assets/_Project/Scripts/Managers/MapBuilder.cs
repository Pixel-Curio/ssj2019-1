using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MapBuilder : IInitializable
    {
        [Inject(Id = "BaseLayer")] private readonly Tilemap _baseLayer;
        [Inject(Id = "DefaultPalette")] private readonly MapPalette _defaultPalette;

        public void Initialize()
        {
            Debug.Log("MapBuilder initializing.");
            _baseLayer.SetTile(new Vector3Int(0,0,0), _defaultPalette.FloorTiles[0]);
        }
    }
}
