using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{

    public class DayNightCycleManager : MonoBehaviour
    {
        [Tooltip("Tempo que você quer que dure o ciclo inteiro (12 minutos)")]public float cycleDurationMinutes = 1f;
        [Tooltip("Valor entre 0 e 1 que define o estado do ciclo.")]public float cycleTime { get; private set; } 
        [Tooltip("Hora virtual baseada no ciclo.")]public float currentHour { get; private set; }  
        public event Action<int> OnHourChanged;

        private float speed;
        private int lastHour;

        private void Awake()
        {
            speed = 1f / (cycleDurationMinutes * 60f);
        }

        private void FixedUpdate()
        {
            cycleTime = Mathf.Repeat(cycleTime + Time.fixedDeltaTime * speed, 1f);
            currentHour = cycleTime * 24f;

            int hourInt = Mathf.FloorToInt(currentHour);
            if (hourInt != lastHour)
            {
                lastHour = hourInt;
                OnHourChanged?.Invoke(lastHour);
            }
        }

        public void InitializeByHour(int hour)
        {
            cycleTime = hour / 24f;
        }
    }

}
