using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public sealed class AppleTree : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        bool monitor = false;
        bool active = false;
       // bool isFound = false;
        IHourProvider hourProvider;
        void Start()
        {
            hourProvider = ServiceLocator.Instance.Get<IHourProvider>();
            musicSource = ServiceLocator.Instance.Get<MusicSource>();
        }

        void Update()
        {
            if (monitor && (hourProvider.Hour > 19 || hourProvider.Hour < 6))
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
            if (musicSource.IsPlaying(BackgroundMusic.AppleTree))
                musicSource.IsSleeping = true;
            else
            {
                musicSource.StopAll();
                musicSource.Play(BackgroundMusic.AppleTree, true);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                monitor = true;
               // if (!isFound)
              //  {
                    Founded();
             //   }
              //  isFound = true;
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                monitor = false;
                musicSource.WakeUp();
            }
        }
    }
}
