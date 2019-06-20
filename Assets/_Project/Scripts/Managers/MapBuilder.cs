using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MapBuilder : IInitializable
    {
        [Inject(Id = "BaseLayer")] private readonly Tilemap _baseLayer;
        [Inject(Id = "WallLayer")] private readonly Tilemap _wallLayer;
        [Inject(Id = "DefaultPalette")] private readonly MapPalette _defaultPalette;

        public void Initialize()
        {
            Debug.Log("MapBuilder initializing.");

            for (int y = 0; y < _defaultPalette.Dimensions.y; y++)
                for (int x = 0; x < _defaultPalette.Dimensions.x; x++)
                {
                    if (x == 0 || x >= _defaultPalette.Dimensions.x - 1 ||
                        y == 0 || y >= _defaultPalette.Dimensions.y - 1)
                        _wallLayer.SetTile(new Vector3Int(x, y, 0), _defaultPalette.WallTiles[0]);

                    _baseLayer.SetTile(new Vector3Int(x, y, 0), _defaultPalette.FloorTiles[0]);
                }
        }
    }
}
