using br.com.bonus630.thefrog.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
namespace br.com.bonus630.thefrog.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;

        [SerializeField] Button continueButton;
        [SerializeField] Button startButton;
        void Start()
        {
            if (GameManager.Instance.CanContinue())
                continueButton.gameObject.SetActive(true);
           string[] joys = Input.GetJoystickNames();
          var gamepad =  Gamepad.current;
          //  Debug.Log(gamepad);
           // startButton.Select();
        }

        public void StartButton_clicked()
        {
            if (text.text == "INICIAL")
                GameManager.Instance.LoadGame(StartType.Start);
            else
                GameManager.Instance.LoadGame(StartType.Continue);
        }
        public void ContinueButton_clicked()
        {
            GameManager.Instance.LoadGame(StartType.Continue);
        }
        public void QuitButton_clicked()
        {
            Application.Quit();
        }
        void Update()
        {
            if (Input.GetButtonDown("Jump"))
                StartButton_clicked();
        }
      
        //public void OnMove(InputAction.CallbackContext context)
        //{
        //    if(context.canceled)
        //    {
        //        startButton.se
        //    }
        //}
    }
}
