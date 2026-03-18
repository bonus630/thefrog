using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public static class Draw 
    {
        public static void Bounds2D(CameraBounds2D bounds, Color color, float time)
        {
            Debug.DrawLine(bounds.topLeft, bounds.topRight, color, time);
            Debug.DrawLine(bounds.topRight, bounds.bottomRight, color, time);
            Debug.DrawLine(bounds.bottomRight, bounds.bottomLeft, color, time);
            Debug.DrawLine(bounds.bottomLeft, bounds.topLeft, color, time);

        }
    }
}
