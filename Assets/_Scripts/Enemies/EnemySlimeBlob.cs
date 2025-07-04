using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemySlimeBlob : MonoBehaviour
    {
        private readonly int Ground = Animator.StringToHash("ground");
        private Animator m_Animator;
        private Rigidbody2D m_Rigidbody;
        private AudioSource m_AudioSource;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            m_Animator = GetComponent<Animator>();
            m_Rigidbody = GetComponent<Rigidbody2D>();
            m_AudioSource = GetComponent<AudioSource>();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
           // 
            if (collision.CompareTag("SpawnPoint"))
                return;
            m_Rigidbody.bodyType = RigidbodyType2D.Static;
            if (collision.CompareTag("Ground"))
            {
                m_Animator.SetTrigger(Ground);
                m_AudioSource.Play();
            }
            else
            {
                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player))
                {Debug.Log(collision.gameObject.name);
                    player.Hit();
                }
                RunGasAnimation();
            }
            Destroy(gameObject, 0.417f);
        }

        private void RunGasAnimation()
        {
            SpriteRenderer sr = GetComponent<SpriteRenderer>();
            sr.enabled = false;
        }
    }
}
