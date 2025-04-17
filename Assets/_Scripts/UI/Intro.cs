using UnityEngine;
using UnityEngine.SceneManagement;
namespace br.com.bonus630.thefrog.UI
{
    public class Intro : MonoBehaviour
    {
        public void GoToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
