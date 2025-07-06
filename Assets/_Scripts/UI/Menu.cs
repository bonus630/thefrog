using br.com.bonus630.thefrog.Manager;
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
        [SerializeField] Button goBackButton;

        [SerializeField] GameObject buttons;
        [SerializeField] GameObject control;


        void Start()
        {
            if(GameManager.Instance.CanContinue())
            {
                continueButton.gameObject.SetActive(true);
                Vector2 pos = controlsButton.GetComponent<RectTransform>().anchoredPosition;
                controlsButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(pos.x,pos.y-120);
            }

            string[] joys = Input.GetJoystickNames();
            var gamepad = Gamepad.current;
            //  Debug.Log(gamepad);
            // startButton.Select();
        }

        public void StartButton_clicked()
        {
            if (text.text == "INICIAL")
                GameManager.Instance.LoadGame(SceneStartType.Start);
            else
                GameManager.Instance.LoadGame(SceneStartType.Continue);
        }
        public void ContinueButton_clicked()
        {
            GameManager.Instance.LoadGame(SceneStartType.Continue);
        }
        public void QuitButton_clicked()
        {
            Application.Quit();
        }
        public void ControlsButton_clicked()
        {
            control.SetActive(true);
            buttons.SetActive(false);
            EventSystem.current.SetSelectedGameObject(goBackButton.gameObject);
        }
        public void GoBackButton_clicked()
        {
            control.SetActive(false);
            buttons.SetActive(true);
            EventSystem.current.SetSelectedGameObject(controlsButton.gameObject);
        }
        //void Update()
        //{
        //    if (Input.GetButtonDown("Jump"))
        //        StartButton_clicked();
        //}

        //public void OnMove(InputAction.CallbackContext context)
        //{
        //    if(context.canceled)
        //    {
        //        startButton.se
        //    }
        //}
    }
}
