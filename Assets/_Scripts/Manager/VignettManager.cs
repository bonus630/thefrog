using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace br.com.bonus630.thefrog.Manager
{
    public class VignetteManager : MonoBehaviour
    {
        [SerializeField] Volume volume;
        private Vignette vignette;
        void Start()
        {
            volume.profile.TryGet<Vignette>(out vignette);
        }

        public void FashVignette(float minValue, float maxValue, float duration, Color color)
        {
            StopAllCoroutines();
            StartCoroutine(RunFashVignette(minValue,maxValue,duration,color));
        }
        private IEnumerator RunFashVignette(float minValue,float maxValue,float duration,Color color)
        {
            vignette.intensity.value = maxValue;

            
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                vignette.intensity.value = Mathf.Lerp(maxValue, minValue, elapsed / duration);
                yield return null;
            }

            vignette.intensity.value = minValue;
        }
    }
}
