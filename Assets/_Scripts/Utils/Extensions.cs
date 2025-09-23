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
        public static Vector2 Vector2FromRect(this System.Random random,Bounds bound)
        {
            float x = Random.Range(bound.center.x - bound.size.x / 2,bound.center.x + bound.size.x / 2);
            float y = Random.Range(bound.center.y - bound.size.y / 2,bound.center.y + bound.size.y / 2);
            return new Vector2(x,y);
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
    }
}