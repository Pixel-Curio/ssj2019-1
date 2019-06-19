using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PixelCurio.OccultClassic
{
    [CreateAssetMenu]
    public class MapPalette : ScriptableObject
    {
        public List<Tile> FloorTiles;
    }
}
