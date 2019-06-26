using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Prefab Tile", menuName = "Tiles/Prefab Tile")]
    public class PrefabTile : TileBase
    {
        public Sprite Sprite;
        public GameObject Prefab;

        private static SceneContext _context;
        private static SceneContext Context
        {
            get
            {
                if (!_context)
                {
                    _context = FindObjectOfType<SceneContext>();
                    if (_context == null) throw new NullReferenceException("No zenject scene context found.");
                }

                return _context;
            }
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            if (Sprite) tileData.sprite = Sprite;
            if (Prefab) tileData.gameObject = Prefab;
        }

        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject gameObject)
        {
            Context.Container.InjectGameObject(gameObject);
            ITileConnection tileConnection = (ITileConnection) gameObject.GetComponent(typeof(ITileConnection));
            tileConnection?.SetTileDependencies(location);

            return true;
        }
    }
}
