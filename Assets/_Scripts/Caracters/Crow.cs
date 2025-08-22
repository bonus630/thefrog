using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class Crow : MonoBehaviour
    {
        Animator animator;
        AudioSource audio;
        int CrawID = Animator.StringToHash("craw");
        float timer = 0;
        float maxTimer = 0f;

        void Start()
        {
            maxTimer = Random.Range(2f, 16f);
            animator = GetComponent<Animator>();
            audio = GetComponent<AudioSource>();
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
            audio.Play();
            animator.SetTrigger(CrawID);
        }
    }
}
