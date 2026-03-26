using System;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace br.com.bonus630.thefrog.UI
{
    public class Pause : MonoBehaviour
    {
        [SerializeField] GameObject      statusPanel;
        [SerializeField] GameObject      optionsPanel;
        [SerializeField] GameObject      controlsPanel;
        [SerializeField] Button          buttonOptions;
        [SerializeField] Button          buttonControls;
        [SerializeField] Button          gobackButtonOptions;
        [SerializeField] Button          gobackButtonControls;
        [SerializeField] TextMeshProUGUI hoursText;
        [SerializeField] TextMeshProUGUI deadsText;
        [SerializeField] TextMeshProUGUI totalTimeText;
        [SerializeField] TextMeshProUGUI runsText;
        [SerializeField] TextMeshProUGUI heartsText;
        [SerializeField] TextMeshProUGUI playerSpeedText;
        [SerializeField] TextMeshProUGUI playerJumpForceText;
        [SerializeField] Image EffectImage;
        [SerializeField] PlayerInput playerInput;
        public int hour;
        public int prevHour;
        Vector2 direction;
        float time = 0f;

        private void OnEnable()
        {
            this.hour = ServiceLocator.Instance.Get<IHourProvider>().Hour;
            TimeSpan time = TimeSpan.FromSeconds(GameManager.Instance.EnvironmentStates.GameTimeInSeconds);
            hoursText.text = this.hour.ToString("00") + " HORAS";
            deadsText.text = GameManager.Instance.PlayerStates.numDies.ToString("0000");
            runsText.text = GameManager.Instance.EnvironmentStates.run.ToString("0000");
            totalTimeText.text = time.ToString(@"hh\:mm\:ss");
            heartsText.text = $"{GameManager.Instance.PlayerStates.Hearts}/{GameManager.Instance.GameTotalHearts}";
            playerSpeedText.text = $"{(int)(GameManager.Instance.PlayerStates.Speed * 100)}";
            playerJumpForceText.text = $"{(int)(GameManager.Instance.PlayerStates.JumpForce * 100)}";
            prevHour = hour;
            playerInput.SwitchCurrentActionMap("UI");
            EventSystem.current.SetSelectedGameObject(null);

            StartCoroutine(CoroutineUtil.WaitFrames(() => { EventSystem.current.SetSelectedGameObject(buttonOptions.gameObject); }));
            buttonOptions.onClick.AddListener(OnOptionClick);
            gobackButtonOptions.onClick.AddListener(OnGobackOptionClick);
            gobackButtonControls.onClick.AddListener(OnGobackOptionClick);
            buttonControls.onClick.AddListener(OnControlsClick);
            buttonOptions.onClick.AddListener(OnOptionClick);
        }
        private void OnDisable()
        {
            playerInput.SwitchCurrentActionMap("Player");
            buttonOptions.onClick.RemoveAllListeners();
            buttonControls.onClick.RemoveAllListeners();
            gobackButtonOptions.onClick.RemoveAllListeners();
            gobackButtonControls.onClick.RemoveAllListeners();
            OnGobackOptionClick();
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
        public void OnOptionClick()
        {
           
            optionsPanel.SetActive(true);
            statusPanel.SetActive(false);
           
            EventSystem.current.SetSelectedGameObject(gobackButtonOptions.gameObject);
        }
        public void OnControlsClick()
        {
            controlsPanel.SetActive(true);
            optionsPanel.SetActive(false);
         
            EventSystem.current.SetSelectedGameObject(gobackButtonControls.gameObject);
        }
        public void OnGobackOptionClick()
        {
            optionsPanel.SetActive(false);
            controlsPanel.SetActive(false);
            statusPanel.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            //StartCoroutine(CoroutineUtil.WaitFrames(() => { EventSystem.current.SetSelectedGameObject(buttonOptions.gameObject); }));
        }
    }
}
