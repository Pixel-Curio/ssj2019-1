using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    [Serializable]
    [CreateAssetMenu(fileName = "New Prefab Tile", menuName = "Tiles/Spawner Tile")]
    public class SpwanerTile : TileBase
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private EnemyType _enemyType;

        [Inject] private Bat.Pool _batPool;

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
            if (_sprite) tileData.sprite = _sprite;
        }

        public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject gameObject)
        {
            Context.Container.InjectGameObject(gameObject);
            Context.Container.Inject(this);
            ITileConnection tileConnection = (ITileConnection)gameObject.GetComponent(typeof(ITileConnection));
            tileConnection?.SetTileDependencies(location);

            switch (_enemyType)
            {
                case EnemyType.Bat:
                    _batPool.Spawn(location);
                    break;
                default:
                    Debug.LogWarning("Spwaner tile with no emeny type selected.");
                    break;
            }

            return true;
        }
    }

    public enum EnemyType
    {
        Bat
    }
}
