using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class Crow : MonoBehaviour
    {
        Animator animator;
        AudioSource audioSource;
        int CrawID = Animator.StringToHash("craw");
        float timer = 0;
        float maxTimer = 0f;

        void Start()
        {
            maxTimer = Random.Range(2f, 16f);
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            timer += Time.deltaTime;
            if (timer > maxTimer)
            {
                timer = 0;
                maxTimer = Random.Range(2f, 16f);
                Craw();
            }
        }
        private void Craw()
        {
            audioSource.Play();
            animator.SetTrigger(CrawID);
        }
    }
}
