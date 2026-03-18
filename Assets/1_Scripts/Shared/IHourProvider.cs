using System;
namespace br.com.bonus630.thefrog.Shared
{
    public interface IHourProvider
    {
        event Action<int> OnHourChanged;
        event Action<bool> OnDayChanged;
        int DawnHour { get; }
        int DuskHour { get; }
        int Hour { get; }
        bool IsDay { get;  }
        float cycleTime { get; }
        float HourToCycleTime(int hour);
        void InitializeByHour(int hour);
        float GetTimeUntil(float currentHour, float targetHour, float cycleDuration);
        bool IsInRange(int start, int end);
    }
}
