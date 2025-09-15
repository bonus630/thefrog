using System.Collections;
using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class StaticsRoutines 
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
    }
}
