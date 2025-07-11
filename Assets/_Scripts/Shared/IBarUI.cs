using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IBarUI
    {
        Color Color { get; set; }
        float Value { get; set; }
        float MinValue { get; set; }
        float MaxValue { get; set; }
        void GoToValue(float value,float time);
        void Destroy();


    }
}
