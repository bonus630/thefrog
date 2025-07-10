using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class WorldLimiter : MonoBehaviour
    {
        [SerializeField] GameObject Next;
        [SerializeField] bool Horizontal;

        private void Awake()
        {
            CheckType();
            GameManager.Instance.eventManager.GameEventCompleted += EventManager_GameEventCompleted;
        }
        private void OnDestroy() => GameManager.Instance.eventManager.GameEventCompleted -= EventManager_GameEventCompleted;
        private void EventManager_GameEventCompleted(GameEvent obj) => CheckType();
     

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Next.GetComponent<Collider2D>().enabled = false;
                Transform p = collision.transform;
                ScreenEffects sf = FindAnyObjectByType<ScreenEffects>();
                sf.FadeOut(0.1f);
                if (Horizontal)
                {
                    p.position = new Vector3(p.position.x, Next.transform.position.y, p.position.z);
                }
                else
                {
                    p.position = new Vector3(Next.transform.position.x, p.position.y, p.position.z);
                }
                sf.FadeIn(0.2f);
                Invoke(nameof(EnableNext), 0.1f);
            }
        }
        private void CheckType()
        {
            if (GameManager.Instance.PlayerStates.FallsControl)
                TeleportCollider();
            else
                DieCollider();
        }
        private void DieCollider()
        {
            gameObject.layer = 10;
            gameObject.GetComponent<Collider2D>().isTrigger = false;
        }
        private void TeleportCollider()
        {
            gameObject.layer = 0;
            gameObject.GetComponent<Collider2D>().isTrigger = true;
        }
        private void EnableNext() =>Next.GetComponent<Collider2D>().enabled = true;
    }
}
