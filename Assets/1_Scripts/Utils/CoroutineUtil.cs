using System.Collections;
using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class CoroutineUtil 
    {
        public static IEnumerator LerpColor(SpriteRenderer sr, Color start, Color end, float duration = 1f)
        {
            float t = 0f;
            while (t < 1)
            {
                t += Time.deltaTime / duration;
                sr.color = Color.Lerp(start, end, t);
                yield return null;
            }
            sr.color = end;
        }
        public static IEnumerator WaitFrames(System.Action a, int frames = 1)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
            }
            a.Invoke();
        }
        public static IEnumerator WaitUntilThen(System.Action conditionMet, System.Func<bool> condition)
        {
            yield return new WaitUntil(condition);
            conditionMet?.Invoke();
        }
    }
}
