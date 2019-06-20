using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class MapBuilder : IInitializable
    {
        [Inject(Id = "BaseLayer")] private readonly Tilemap _baseLayer;
        [Inject(Id = "WallLayer")] private readonly Tilemap _wallLayer;
        [Inject(Id = "DecorationLayer")] private readonly Tilemap _decorationLayer;
        [Inject(Id = "ObjectLayer")] private readonly Tilemap _objectLayer;
        [Inject(Id = "DefaultMap")] private readonly Map _defaultMap;

        public void Initialize()
        {
            Debug.Log("MapBuilder initializing.");

            for (int y = 0; y < _defaultMap.Dimensions.y; y++)
                for (int x = 0; x < _defaultMap.Dimensions.x; x++)
                {
                    if (x == 0 || x >= _defaultMap.Dimensions.x - 1 ||
                        y == 0 || y >= _defaultMap.Dimensions.y - 1)
                        _wallLayer.SetTile(new Vector3Int(x, y, 0), _defaultMap.WallTiles[Random.Range(0, _defaultMap.WallTiles.Count)]);

                    _baseLayer.SetTile(new Vector3Int(x, y, 0), _defaultMap.FloorTiles[0]);

                    if (Random.value < _defaultMap.DecorationChance)
                        _decorationLayer.SetTile(new Vector3Int(x, y, 0),
                            _defaultMap.DecorationTiles[Random.Range(0, _defaultMap.DecorationTiles.Count)]);

                    if (Random.value < _defaultMap.ObjectChance)
                        _objectLayer.SetTile(new Vector3Int(x, y, 0),
                            _defaultMap.ObjectTiles[Random.Range(0, _defaultMap.ObjectTiles.Count)]);
                }
        }
    }
}
