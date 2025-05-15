using System.Collections;
using UnityEngine;

namespace DeadLink.Misc
{
    public class CoroutineHandler : MonoBehaviour
    {
        public void StartNewCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(Coroutine(coroutine));
        }
        
        private IEnumerator Coroutine(IEnumerator coroutine)
        {
            yield return StartCoroutine(coroutine);
            Destroy(this);
        }
    }
}