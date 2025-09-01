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
    }
}