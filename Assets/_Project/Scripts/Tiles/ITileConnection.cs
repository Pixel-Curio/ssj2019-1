using UnityEngine;

namespace PixelCurio.OccultClassic
{
    public interface ITileConnection
    {
        void SetTileDependencies(Vector3Int location);
    }
}
