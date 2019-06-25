using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PixelCurio.OccultClassic
{
    [CreateAssetMenu]
    public class Map : ScriptableObject
    {
        public Vector2 MaxMapSize = new Vector2(64, 64);
        public Vector2 MinRoomSize = new Vector2(5, 5);
        public Vector2 MaxRoomSize = new Vector2(25, 25);
        public int MaxRoomCount = 50;
        public int MaxFailedRoomCount = 1000;
        public float DecorationChance;
        public float ObjectChance;
        public int MapSeed;
        public List<TileBase> WallTiles;
        public List<TileBase> FloorTiles;
        public List<TileBase> DecorationTiles;
        public List<TileBase> ObjectTiles;
    }
}
