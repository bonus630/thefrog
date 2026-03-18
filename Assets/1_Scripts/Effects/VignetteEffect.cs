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
        // Construtor
        private VignetteEffect(Volume volume, float minValue, float maxValue, float duration)
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
        public static VignetteEffect Create(Volume volume)
        {
            if (volume == null)
                throw new System.ArgumentNullException(nameof(volume));
            return new VignetteEffect(volume, 0, 0, 0);
        }
        public VignetteEffect WithInitialIntensity(float value)
        {
            this.startIntensity = value;
            return this;
        }
        public VignetteEffect WithFinalIntensity(float value)
        {
            this.endIntensity = value;
            return this;
        }
        public VignetteEffect WithDuration(float value)
        {
            this.duration = value;
            return this;
        }
        public override void UpdateEffects(float deltaTime)
        {
            if (IsFinished || vignette == null) return;

            timer += deltaTime;
            float t = Mathf.Clamp01(timer / duration);

            vignette.intensity.value = Mathf.Lerp(startIntensity, endIntensity, t);

            if (t >= 1f)
                IsFinished = true;
        }

        public override void Activate()
        {
            throw new System.NotImplementedException();
        }

        public override void Deactivate()
        {
            IsFinished = true;
        }
    }

}
