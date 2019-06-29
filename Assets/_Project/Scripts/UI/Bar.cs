using UnityEngine;

namespace PixelCurio.OccultClassic
{
    public class Bar : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _bar;

        public Bar SetWidth(float width)
        {
            Vector3 scale = _bar.transform.localScale;
            scale.x = width;
            _bar.transform.localScale = scale;
            return this;
        }
    }
}
