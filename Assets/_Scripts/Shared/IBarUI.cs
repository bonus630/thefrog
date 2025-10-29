using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IBarUI
    {
        int id { get; set; }
        Color Color { get; set; }
        GameObject gameObject { get; }
        float Value { get; set; }
        float CurrentValue { get; }
        float MinValue { get; set; }
        float MaxValue { get; set; }
        float ElapsedTime { get; set; } 
        void GoToValue(float value,float time);
        void DestroyBar();
        void DestroyBar(float time);   

        event Action<GameObject, bool> BarDestroyed;

    }
}
