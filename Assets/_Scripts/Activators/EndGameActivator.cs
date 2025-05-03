using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog.Activators
{
    internal class EndGameActivator :   IActivator 
    {
        public override void Activate()
        {
            SceneManager.LoadScene("Credit");
        }

        public override void Deactive()
        {
            SceneManager.LoadScene("Credit");
        }
    }
}
