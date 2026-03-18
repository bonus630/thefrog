using System;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class DayNightCycleManager : MonoBehaviour, IHourProvider,IService
    {
        [Tooltip("Tempo que você quer que dure o ciclo inteiro (12 minutos)")]  public float cycleDurationMinutes = 1f;
        [Tooltip("Valor entre 0 e 1 que define o estado do ciclo.")]            public float cycleTime { get; private set; } 
        [Tooltip("Hora virtual baseada no ciclo.")]                             public float currentHour { get; private set; }  
        public event Action<int>  OnHourChanged;
        public event Action<bool> OnDayChanged; 

        public int Hour { get; private set; }
        public int DawnHour => 6;
        public int DuskHour => 18;
        private float speed;
        private int lastHour = -1;
        public bool IsDay { get; private set; }

        private void Awake()
        {
            ServiceLocator.Instance.Register<IHourProvider>(this);
            speed = 1f / (cycleDurationMinutes * 60f);
            CalculeHour();
            
        }
        private void Start()
        {
            OnDayChanged?.Invoke(IsInRange(DawnHour, DuskHour));
        }
        private void FixedUpdate()
        {
            CalculeHour();
        }
        private void CalculeHour()
        {
            cycleTime = Mathf.Repeat(cycleTime + Time.fixedDeltaTime * speed, 1f);
            currentHour = cycleTime * 24f;

            int hourInt = Mathf.FloorToInt(currentHour);
            if (hourInt != lastHour)
            {
                lastHour = hourInt;
                Hour = lastHour;
                OnHourChanged?.Invoke(lastHour);
            }
            bool day = IsInRange(DawnHour, DuskHour);
            if (IsDay != day)
                OnDayChanged?.Invoke(day);
            IsDay = day;
        }
        public void InitializeByHour(int hour)
        {
            cycleTime = hour / 24f;
            //Debug.Log("[IHourProvider] InitializeDayByHour hour:" + hour);
        }
        public float GetTimeUntil(float currentHour, float targetHour, float cycleDuration)
        {
            float currentCycleTime = currentHour / 24f;
            float targetCycleTime = targetHour / 24f;

            // Diferença cíclica (sempre positiva, respeitando o ciclo de 24h)
            float cycleDifference = Mathf.Repeat(targetCycleTime - currentCycleTime, 1f);

            // Tempo real que falta
            return cycleDifference * cycleDuration;
        }
        public bool IsInRange(int start, int end)
        {
            if (start <= end)
                return Hour >= start && Hour <= end;

            // intervalo atravessa meia-noite
            return Hour >= start || Hour <= end;
        }
        public float HourToCycleTime(int hour) => hour / 24f;

    }

}
