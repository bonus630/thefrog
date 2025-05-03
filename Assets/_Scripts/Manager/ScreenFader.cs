using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog
{
    public class ScreenFader : MonoBehaviour
    {
        public Image fadeImage;
        public float fadeDuration = 1f;

        public IEnumerator FadeOut()
        {
            yield return StartCoroutine(Fade(0f, 1f));
        }

        public IEnumerator FadeIn()
        {
            yield return StartCoroutine(Fade(1f, 0f));
        }

        private IEnumerator Fade(float startAlpha, float endAlpha)
        {
            float timer = 0f;
            Color color = fadeImage.color;

            while (timer < fadeDuration)
            {
                float alpha = Mathf.Lerp(startAlpha, endAlpha, timer / fadeDuration);
                fadeImage.color = new Color(color.r, color.g, color.b, alpha);
                timer += Time.deltaTime;
                yield return null;
            }

            fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
        }
    }
}
