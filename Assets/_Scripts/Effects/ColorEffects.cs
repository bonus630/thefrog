using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class ColorEffect : IEffects
    {
        private SpriteRenderer sr;
        private Color start, end;
        private float duration;
        private float timer = 0f;
    
        private ColorEffect(SpriteRenderer sr,Color start, Color end, float duration = 1f)
        {
            this.sr = sr;
            this.start = start;
            this.end = end;
            this.duration = duration;
        }
        public static ColorEffect Create(SpriteRenderer target)
        {
            if (target == null)
                throw new System.ArgumentNullException(nameof(target));
            return new ColorEffect(target, Color.white, Color.black);
        }
        public ColorEffect WithStartColor(Color value)
        {
            this.start = value;
            return this;
        }
        public ColorEffect WithEndColor(Color value)
        {
            this.end = value;
            return this;
        }
        public ColorEffect WithDuration(float value)
        {
            this.duration = value;
            return this;
        }
        public override void UpdateEffects(float deltaTime)
        {
            if (sr == null)  IsFinished = true; 
            if (IsFinished) return;
            timer += deltaTime;
            float t = Mathf.Clamp01(timer / duration);
            sr.color = Color.Lerp(start, end, t);

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
