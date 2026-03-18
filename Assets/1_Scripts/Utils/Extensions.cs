using System.Collections;
using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class Extensions 
    {
        public static Vector2 Vector2FromRect(this System.Random random,Rect bound)
        {
            float x = Random.Range(bound.center.x - bound.size.x / 2,bound.center.x + bound.size.x / 2);
            float y = Random.Range(bound.center.y - bound.size.y / 2,bound.center.y + bound.size.y / 2);
            return new Vector2(x,y);
        }
        public static Vector2 RandomVector2(this  Rect bound)
        {
            float x = Random.Range(bound.center.x - bound.size.x / 2, bound.center.x + bound.size.x / 2);
            float y = Random.Range(bound.center.y - bound.size.y / 2, bound.center.y + bound.size.y / 2);
            return new Vector2(x, y);
        }
        public static Vector2 Vector2FromRect(this System.Random random,Bounds bound)
        {
            float x = Random.Range(bound.center.x - bound.size.x / 2,bound.center.x + bound.size.x / 2);
            float y = Random.Range(bound.center.y - bound.size.y / 2,bound.center.y + bound.size.y / 2);
            return new Vector2(x,y);
        }
        public static Vector2 RandomVector2(this Bounds bound)
        {
            float x = Random.Range(bound.center.x - bound.size.x / 2, bound.center.x + bound.size.x / 2);
            float y = Random.Range(bound.center.y - bound.size.y / 2, bound.center.y + bound.size.y / 2);
            return new Vector2(x, y);
        }

        public static bool ContainsChildren(this Transform transform, string childrenName)
        {
            if (transform.childCount == 0)
                return false;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).CompareTag(childrenName))
                    return true;
            }
            return false;
        }
        public static void SetAlpha(this SpriteRenderer sr, float alpha)
        {
            Color c = sr.color;
            c.a = alpha;
            sr.color = c;
        }
        public static bool IsInLayerMask(this GameObject obj, LayerMask layerMask)
        {
            // Converte o layer do objeto em um bitmask
            int objLayerMask = 1 << obj.layer;

            // Faz o AND bitwise e verifica se o resultado é diferente de zero
            return (layerMask.value & objLayerMask) != 0;
        }
      
        public static float Distance2D(this Vector3 a, Vector3 b) => Vector2.Distance(new Vector2(a.x, a.y), new Vector2(b.x, b.y));
        public static float Distance2D(this Vector3 a, Vector2 b) => Vector2.Distance(new Vector2(a.x, a.y), b);
        public static float Distance2D(this Vector2 a, Vector3 b) => Vector2.Distance(a, new Vector2(b.x, b.y));
        public static float Distance2D(this Vector2 a, Vector2 b) => Vector2.Distance(a, b);

        public static Vector2 ToVector2(this Vector3 v)=>new Vector2(v.x,v.y);


    }
}