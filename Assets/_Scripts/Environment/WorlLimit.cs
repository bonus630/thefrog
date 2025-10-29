using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class WorldLimiter : MonoBehaviour
    {
        [SerializeField] GameObject Next;
        [SerializeField] bool Horizontal;




        private void OnTriggerEnter2D(Collider2D collision)
        {
            //Debug.Log("[Worllimit] " + name);
            if (collision.CompareTag("Player"))
            {
                if (GameManager.Instance.PlayerStates.FallsControl)
                {
                    Next.GetComponent<Collider2D>().enabled = false;
                    Transform p = collision.transform;
                    ScreenEffects sf = FindAnyObjectByType<ScreenEffects>();
                    sf.FadeOut(0.5f);
                    if (Horizontal)
                    {
                        p.position = new Vector3(p.position.x, Next.transform.position.y, p.position.z);
                    }
                    else
                    {
                        p.position = new Vector3(Next.transform.position.x, p.position.y, p.position.z);
                    }
                    sf.FadeIn(0.2f);
                    Invoke(nameof(EnableNext), 1f);
                }
                else
                {
                    Die();
                }
            }
            else
            {
                //Debug.Log("[Worllimit] "+name);
                Next.GetComponent<Collider2D>().enabled = false;
                Transform p = collision.transform;
                if (Horizontal)
                {
                    p.position = new Vector3(p.position.x, Next.transform.position.y, p.position.z);
                }
                else
                {
                    p.position = new Vector3(Next.transform.position.x, p.position.y, p.position.z);
                }
            }
        }

        private void Die()
        {
            GameManager.Instance.GetPlayerScript.CurrentLife = 1;
            GameManager.Instance.GetPlayerScript.Hit();
        }

        private void EnableNext() => Next.GetComponent<Collider2D>().enabled = true;
    }
}
