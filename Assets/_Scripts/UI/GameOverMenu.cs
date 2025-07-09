using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.UI
{
    public class GameOverMenu : MonoBehaviour
    {
        [SerializeField] GameObject buttons;
        [SerializeField] GameObject saves;

        public void ContinueButton_clicked()
        {
            GameManager.Instance.LoadGame(SceneStartType.Continue);
        }
        public void LoadButton_clicked()
        {
            buttons.SetActive(false);
            saves.SetActive(true);
        }
        public void QuitButton_clicked()
        {
            Application.Quit();
        }
    }
}
