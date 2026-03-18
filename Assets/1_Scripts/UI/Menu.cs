using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace br.com.bonus630.thefrog.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] Button startButton;
        [SerializeField] Button continueButton;
        [SerializeField] Button controlsButton;
        [SerializeField] Button goBackButtonControls;
        [SerializeField] Button goBackButtonOptions;

     
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject control;
        [SerializeField] GameObject options;
        [SerializeField] GameObject saves;

        void Start()
        {
            StartCoroutine(WaitLoad());

        }
        private IEnumerator WaitLoad()
        {
            yield return new WaitUntil(() => GameManager.Instance != null);// vamos esperar a instancia estatica do gamemanager ficar pronta
            if (GameManager.Instance.CanContinue())
            {
                continueButton.gameObject.SetActive(true);
                Vector2 pos = controlsButton.GetComponent<RectTransform>().anchoredPosition;
                controlsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x,pos.y-120);
            }

         //   string[] joys = Input.GetJoystickNames();
           // var gamepad = Gamepad.current;
            //  Debug.Log(gamepad);
            // startButton.Select();
        }

        public void StartButton_clicked()
        {
            if (text.text == "INICIAR")
                GameManager.Instance.LoadGame(SceneStartType.Start);
            else
                GameManager.Instance.LoadGame(SceneStartType.Continue);
        }
        public void ContinueButton_clicked()
        {
            buttons.SetActive(false);
            saves.SetActive(true);
        }
        //public void QuitButton_clicked()
        //{
        //    Application.Quit();
        //}
        public void ControlsButton_clicked()
        {
            control.SetActive(true);
            options.SetActive(false);
            EventSystem.current.SetSelectedGameObject(goBackButtonControls.gameObject);
        }
        public void OptionsButton_clicked()
        {
            buttons.SetActive(false);
            options.SetActive(true);
            EventSystem.current.SetSelectedGameObject(goBackButtonOptions.gameObject);
        }
        public void GoBackButton_clicked()
        {
            options.SetActive(false);
            control.SetActive(false);
            buttons.SetActive(true);
            saves.SetActive(false);
            EventSystem.current.SetSelectedGameObject(startButton.gameObject);
        }

    }
}
