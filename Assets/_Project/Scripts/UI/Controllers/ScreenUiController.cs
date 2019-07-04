using TMPro;
using UnityEngine;

namespace PixelCurio.OccultClassic
{
    public class ScreenUiController : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _loadingText;

        public void SetLoadingText(string text) => _loadingText.text = text;
    }
}
