using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class BounceEffect : IEffects
    {
        private Vector3 originalScale;
        private float bounceTimer = 0f;

        private float bounceDuration = 0.2f;
        public float bounceAmount = 0.1f;

        private Transform transform;
        private BounceEffect(Transform transform)
        {
            this.transform = transform;
            originalScale = transform.localScale;

        }

        public static BounceEffect Create(Transform transform)
        {
            return new BounceEffect(transform);
        }
        public BounceEffect WithDuration(float value)
        {
            this.bounceDuration = value;
            return this;
        }
        public BounceEffect WithAmount(float value)
        {
            this.bounceAmount = value;
            return this;
        }
        public override void UpdateEffects(float deltaTime)
        {
            if (!IsFinished)
            {
                bounceTimer += Time.deltaTime;
                float t = bounceTimer / bounceDuration;

                // Curva de bounce (pode usar Mathf.Sin, Mathf.SmoothStep, etc)
                // float bounce = Mathf.SmoothStep(originalScale.y, originalScale.y / 2, t);
                float bounce = Mathf.Sin(t * Mathf.PI); // 0 → 1 → 0
                                                        //Debug.Log($"Bounce : {bounce}");
                                                        // Debug.Log($"OriginalScaleY : {originalScale.y}");
                                                        // Aplica compressão no Y e estica no X
                float scaleY = originalScale.y - bounceAmount * bounce;
                float scaleX = originalScale.x + bounceAmount * bounce * 0.5f;
                if (transform != null)
                    transform.localScale = new Vector3(scaleX, scaleY, originalScale.z);
                else
                    IsFinished = true;
                if (t >= 1f)
                {
                    IsFinished = true;
                    transform.localScale = new Vector3(1, 1, 1);
                }
            }
        }

        public override void Activate()
        {
        }

        public override void Deactivate()
        {
            IsFinished = true;
        }
    }
}
