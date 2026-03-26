using UnityEngine;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog.UI
{
    public class PigLaughter : MonoBehaviour
    {
        public void GoTo()
        {
            SceneManager.LoadScene("Credit");
        }
    }
}
