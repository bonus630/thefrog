using br.com.bonus630.thefrog.Manager;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace br.com.bonus630.thefrog.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI text;
        [SerializeField] Button continueButton;
        void Start()
        {
            if (GameManager.Instance.CanContinue())
                continueButton.gameObject.SetActive(true);
        }

        public void StartButton_clicked()
        {
            if (text.text == "Iniciar")
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

    }
}
