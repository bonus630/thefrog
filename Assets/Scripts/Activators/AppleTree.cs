using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class AppleTree : MonoBehaviour
    {
        bool monitor = false;
        bool active = false;
        bool isFound = false;
        void Start()
        {

        }

        void Update()
        {
            if (monitor && (GameManager.Instance.PlayerStates.Hour > 19 || GameManager.Instance.PlayerStates.Hour < 6))
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(true);
                gameObject.transform.GetChild(1).gameObject.SetActive(false);
                active = true;

            }
            else
            {
                gameObject.transform.GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(1).gameObject.SetActive(true);
            }
            if (active)
                Active();
        }
        private void Active()
        {
            active = false;
            if (GameManager.Instance.IsEventCompleted(GameEventName.AppleTreeFounded))
                return;
            GameManager.Instance.EventCompleted(GameEventName.AppleTreeFounded);

        }
        private void Founded()
        {
            GameObject.Find("Listener").GetComponent<MusicSource>().PlayFadIn(BackgroundMusic.AppleTree);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                monitor = true;
                if (!isFound)
                {
                    Founded();
                }
                isFound = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                monitor = false;
        }
    }
}
