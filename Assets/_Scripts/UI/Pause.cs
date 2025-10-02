using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.UI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI hoursText;
        public int hour;
        public int prevHour;
        Vector2 direction;
        float time = 0f;

        private void OnEnable()
        {
            this.hour = ServiceLocator.Get<DayNightCycleManager>().Hour;
            hoursText.text = this.hour.ToString("00") + " HORAS";
            prevHour = hour;
        }
        private void OnDisable()
        {
          
        }
        private void Update()
        {
            if (time >= 0.2f)
            {
                if (direction.y < 0)
                {
                    DecreaseHour();
                    time = 0f;
                }
                if (direction.y > 0)
                {
                    IncreaseHour();
                    time = 0f;
                }
               
            }
            time += Time.unscaledDeltaTime;
        }
        public void GetDirection(InputAction.CallbackContext context)
        {
            
            direction = context.ReadValue<Vector2>();
           
        }
        public void ConfirmAction(InputAction.CallbackContext context)
        {
            if(context.started)
            {
                if (this.hour != prevHour)
                {
                    GameManager.Instance.PlayerStates.Hour = hour;
                    FindAnyObjectByType<CameraBackground>().InitializeDayByHour(this.hour);
                }
            }
        }
        public void IncreaseHour()
        {
            this.hour++;
            if (this.hour > 23)
                this.hour = 0;
            hoursText.text = this.hour.ToString("00") + " HORAS";
          
        }
        public void DecreaseHour()
        {
            this.hour--;
            if (this.hour < 0)
                this.hour = 23;
            hoursText.text = this.hour.ToString("00") + " HORAS";
        
        }
    }
}
