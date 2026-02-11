using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class KoarLimiter : MonoBehaviour
    {
        [SerializeField] MusicSource musicSource;
        BoxCollider2D col;
        Transform playerPos;
        float time = 1f;
        readonly string musicKey = "KoarLimiter";
        void Awake()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.DefeatWizard))
            {
                Destroy(gameObject);
                return;
            }
            col = GetComponent<BoxCollider2D>();
            playerPos = ServiceLocator.Instance.Get("Player").transform;
            musicSource = ServiceLocator.Instance.Get<MusicSource>();
        }
        public void Update()
        {
            Change(col.OverlapPoint(playerPos.position));
        }
        private void Change(bool b)
        {
            // Debug.Log("Koar limiter: " + !b);
            StartCoroutine(change(b, b ? time : 0));
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            musicSource?.PreserveMusic(musicKey);
            musicSource?.InstantPlay(BackgroundMusic.DarkWind, true);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            musicSource?.RestoreMusic(musicKey);
        }
        private IEnumerator change(bool b, float time = 0)
        {
            yield return new WaitForSeconds(time);
            if (GameManager.Instance.IsEventCompleted(GameEventName.Gravity))
                GameManager.Instance.PlayerStates.HasGravity = !b;
            if (GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
                GameManager.Instance.PlayerStates.FallsControl = !b;
        }
    }
}
