using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PixelCurio.OccultClassic
{
    [CreateAssetMenu]
    public class Map : ScriptableObject
    {
        public Vector2 Dimensions;
        public float DecorationChance;
        public float ObjectChance;
        public List<TileBase> WallTiles;
        public List<TileBase> FloorTiles;
        public List<TileBase> DecorationTiles;
        public List<TileBase> ObjectTiles;
    }
}
