

using System;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IHourProvider 
    {
         event Action<int> OnHourChanged;
         int Hour { get;  }
         void InitializeByHour(int hour);
         float GetTimeUntil(float currentHour, float targetHour, float cycleDuration);
        bool IsInRange(int start, int end);

       
    }
}
