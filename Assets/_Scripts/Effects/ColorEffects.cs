using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class ColorEffect : IEffects
    {
        private SpriteRenderer sr;
        private Color start, end;
        private float duration;
        private float timer = 0f;
        public bool IsFinished { get; private set; } = false;

        public ColorEffect(SpriteRenderer sr,Color start, Color end, float duration = 1f)
        {
            this.sr = sr;
            this.start = start;
            this.end = end;
            this.duration = duration;
        }

        public void UpdateEffects(float deltaTime)
        {
            if (sr == null)  IsFinished = true; 
            if (IsFinished) return;
            timer += deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            sr.color = Color.Lerp(start, end, t);

            if (t >= 1f)
                IsFinished = true;
        }
    }

}
