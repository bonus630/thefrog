using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog.Manager
{
    public class ScreenFader : MonoBehaviour
    {
        [SerializeField]private Image fadeImage;
        public float fadeDuration = 1f;
        [SerializeField] private bool useUnscaledTime = false;
        public event Action OnFadeOutCompleted;
        public event Action OnFadeInCompleted;

        private float duration;
        private float elapsed;
        private float from;
        private float to;
        private float maxFadeTime = 10f;
        //public IEnumerator FadeOut()
        //{
        //    yield return StartCoroutine(Fade(0f, 1f));
        //}

        //public IEnumerator FadeIn()
        //{
        //    yield return StartCoroutine(Fade(1f, 0f));
        //}

        //private IEnumerator Fade(float startAlpha, float endAlpha)
        //{
        //    float timer = 0f;
        //    Color color = fadeImage.color;

        //    while (timer < fadeDuration)
        //    {
        //        float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
        //        fadeImage.color = new Color(color.r, color.g, color.b, alpha);
        //        timer += Time.deltaTime;
        //        yield return null;
        //    }

        //    fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
        //}
        private void Awake()
        {
            fadeImage.color = new Color(0, 0, 0, 0);
            state = FadeState.FadedIn;
        }
        private FadeState state = FadeState.FadedOut;
        public FadeState State
        {
            get { return state; }
            private set
            {
                if (state == value) return;
                state = value;
                if (state.Equals(FadeState.FadedIn))
                    OnFadeInCompleted?.Invoke();
                if (state.Equals(FadeState.FadedOut))
                    OnFadeOutCompleted?.Invoke();
            }
        }

        public void FadeIn(float duration, bool unscaled = false)
        {
            StopFade();
            from = 1f;
            to = 0f;
            this.duration = duration;
            this.fadeDuration = this.duration;
            elapsed = 0f;
            useUnscaledTime = unscaled;

            State = FadeState.FadingIn;
        }

        public void FadeOut(float duration, bool unscaled = false)
        {
            StopFade();
            from = 0f;
            to = 1f;
            this.duration = duration;
            elapsed = 0f;
            useUnscaledTime = unscaled;

            State = FadeState.FadingOut;
        }
        public void StopFade()
        {
            if (State == FadeState.FadingIn)
            {
                SetAlpha(0f);
                State = FadeState.FadedIn;
            }
            else if (State == FadeState.FadingOut)
            {
                SetAlpha(1f);
                State = FadeState.FadedOut;
            }

            elapsed = 0f;
        }

        void Update()
        {
            if(State == FadeState.FadedIn)
            {
                maxFadeTime -= Time.deltaTime;
                if (maxFadeTime < 0)
                {
                    SetAlpha(1f);
                    return;
                }
            }
            maxFadeTime = 10f;
            if (State != FadeState.FadingIn && State != FadeState.FadingOut)
                return;

            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            elapsed += dt;

            float t = Mathf.Clamp01(elapsed / duration);
            float a = Mathf.Lerp(from, to, t);
            SetAlpha(a);

            if (t >= 1f)
            {
                State = (State == FadeState.FadingIn)
                    ? FadeState.FadedIn
                    : FadeState.FadedOut;
            }
        }
        private void SetAlpha(float a)
        {
            var c = fadeImage.color;
            c.a = a;
            fadeImage.color = c;
        }
    }
    public enum FadeState
    {
        Idle,
        FadingIn,
        FadingOut,
        FadedIn,
        FadedOut
    }


}
