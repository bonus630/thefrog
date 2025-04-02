using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class IntroAlysky : MonoBehaviour
    {
        bool play = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !play)
            {
                ExecuteIntro();
            }
        }

        public void ExecuteIntro()
        {
            play = true;
            GameObject.Find("Listener").GetComponent<MusicSource>().PlayFadIn(BackgroundMusic.AlyskyIntro);
        }
    }
}
