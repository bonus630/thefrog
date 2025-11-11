using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace br.com.bonus630.thefrog.Effects
{
    public class VignetteEffect : IEffects
    {
        private Vignette vignette;
        private float startIntensity;
        private float endIntensity;
        private float duration;
        private float timer = 0f;
        public ushort ID { get; set; }
        public bool IsFinished { get; private set; } = false;

        // Construtor
        public VignetteEffect(Volume volume, float minValue, float maxValue, float duration)
        {
            if (!volume.profile.TryGet<Vignette>(out vignette))
            {
                Debug.LogWarning("Vignette not found in volume profile!");
                IsFinished = true;
                return;
            }

            startIntensity = maxValue; // começa no valor máximo
            endIntensity = minValue;   // termina no valor mínimo
            vignette.intensity.value = startIntensity;
            this.duration = duration;
        }

        public void UpdateEffects(float deltaTime)
        {
            if (IsFinished || vignette == null) return;

            timer += deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            vignette.intensity.value = Mathf.Lerp(startIntensity, endIntensity, t);

            if (t >= 1f)
                IsFinished = true;
        }

        public void Activate()
        {
            throw new System.NotImplementedException();
        }

        public void Deactivate()
        {
            throw new System.NotImplementedException();
        }
    }

}
