using System;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog.UI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI hoursText;
        [SerializeField] TextMeshProUGUI deadsText;
        [SerializeField] TextMeshProUGUI totalTimeText;
        [SerializeField] TextMeshProUGUI runsText;
        [SerializeField] TextMeshProUGUI heartsText;
        [SerializeField] TextMeshProUGUI playerSpeedText;
        [SerializeField] TextMeshProUGUI playerJumpForceText;
        [SerializeField] Image EffectImage;
        public int hour;
        public int prevHour;
        Vector2 direction;
        float time = 0f;

        private void OnEnable()
        {
            this.hour = ServiceLocator.Instance.Get<IHourProvider>().Hour;
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.EnvironmentStates.GameTimeInSeconds);
            hoursText.text =           this.hour.ToString("00") + " HORAS";
            deadsText.text =           GameManager.Instance.PlayerStates.numDies.ToString("0000");
            runsText.text =            GameManager.Instance.EnvironmentStates.run.ToString("0000");
            totalTimeText.text =       time.ToString(@"hh\:mm\:ss");
            heartsText.text =          $"{GameManager.Instance.PlayerStates.Hearts}/{GameManager.Instance.GameTotalHearts}";
            playerSpeedText.text =     $"{GameManager.Instance.PlayerStates.Speed * 100}";
            playerJumpForceText.text = $"{GameManager.Instance.PlayerStates.JumpForce * 100}";
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

            if (time > 2)
            {
                EffectImage.GetComponent<RectTransform>().anchoredPosition += Vector2.down * 10;
            }
            if (EffectImage.GetComponent<RectTransform>().anchoredPosition.y < -1200)
                EffectImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(EffectImage.GetComponent<RectTransform>().anchoredPosition.x, 0);
        }
        public void GetDirection(InputAction.CallbackContext context)
        {
            Debug.Log("[Pause][GetDiretions]");
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
