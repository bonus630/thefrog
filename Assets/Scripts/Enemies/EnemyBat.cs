using Unity.VisualScripting;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class EnemyBat : EnemyBase
    {
        public float startTime = 1f;
        private bool startFly = false;
        private AudioSource audioSource;
        [SerializeField] private Vector2 EndPoint;
        protected readonly int StartFlyID = Animator.StringToHash("StartFly");




        protected override void Start()
        {
            base.Start();
            Invoke(nameof(StartFly), 1);
            audioSource = GetComponent<AudioSource>();
            repulse = Vector2.up * 140;
        }
        protected override void Update()
        {
            base.Update();
            if (startFly)
            {
                if (startTime < 0)
                    gameObject.transform.Translate(Vector2.left * speed * Time.deltaTime);
                else
                    startTime -= Time.deltaTime;
                if (gameObject.transform.position.x < EndPoint.x)
                    Destroy(gameObject);
            }

        }
        private void StartFly()
        {
            startFly = true;
            audioSource.Play();
            animator.SetBool(StartFlyID, true);
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            base.OnCollisionEnter2D(collision);
            //if (collision.gameObject.CompareTag("Player"))
            //{
            //    Hit(1);
            //}
        }
    }
}
