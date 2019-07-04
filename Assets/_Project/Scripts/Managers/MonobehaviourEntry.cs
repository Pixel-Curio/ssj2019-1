using System.Collections;
using UnityEngine;

namespace PixelCurio.OccultClassic
{
    public class MonobehaviourEntry : MonoBehaviour
    {
        public void StartChildCoroutine(IEnumerator coroutineMethod)
        {
            StartCoroutine(coroutineMethod);
        }
    }
}
