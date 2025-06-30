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
            Debug.Log(collision.gameObject.name);
            if (collision.CompareTag("SpawnPoint"))
                return;
            if(collision.CompareTag("Ground"))
            {
                m_Animator.SetTrigger(Ground);
                m_Rigidbody.bodyType = RigidbodyType2D.Static;
                m_AudioSource.Play();
            }
            else
            {
                
            }
            Destroy(gameObject, 0.417f);
        }
    }
}
