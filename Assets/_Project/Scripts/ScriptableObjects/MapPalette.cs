﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PixelCurio.OccultClassic
{
    [CreateAssetMenu]
    public class MapPalette : ScriptableObject
    {
        public Vector2 Dimensions;
        public List<Tile> WallTiles;
        public List<Tile> FloorTiles;
    }
}
