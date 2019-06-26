using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;

namespace PixelCurio.OccultClassic
{
    public class Destructable : MonoBehaviour, ITileConnection
    {
        [SerializeField] private LayerMask _destructionLayers;
        private Vector3Int _location;
        private bool _destroy;

        [Inject(Id = "ObjectLayer")] private readonly Tilemap _objectLayer;
        [Inject(Id = "LightExplosion")] private readonly PlaceableEffect.Pool _effectPool;

        public void Update()
        {
            if (_destroy)
            {
                Destroy(gameObject);
                _effectPool.Spawn(transform.position);
                _objectLayer.SetTile(_location, null);
            }
        }

        public void SetTileDependencies(Vector3Int location)
        {
            _location = location;
        }

        public void OnCollisionEnter2D(Collision2D col)
        {
            if (_destructionLayers == (_destructionLayers | (1 << col.gameObject.layer)))
            {
                _destroy = true;
            }
        }
    }
}
