using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemySlime : MonoBehaviour
    {
        [SerializeField] protected float maxFollowDistance = 6f;
        [SerializeField] GameObject blobInstante;
        [SerializeField] GameObject blobSpawn1;
        [SerializeField] GameObject blobSpawn2;

        Animator anin;
        float timer = 0;
        protected GameObject player;
        private readonly int Run = Animator.StringToHash("Run");
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            anin = GetComponent<Animator>();
            player = GameManager.Instance.GetPlayer;
        }

        // Update is called once per frame
        void Update()
        {
            if (Vector3.Distance(player.transform.position, transform.position) > maxFollowDistance)
                return;
            timer -=Time.deltaTime;
            if(timer < 0)
            {
                anin.SetTrigger(Run);
                timer = Random.Range(1f,3f);
            }
        }
        public void Blob1()
        {
            Blob(blobSpawn1);
        }
        public void Blob2()
        {
            Blob(blobSpawn2);
        }
        private void Blob(GameObject blocSpawn)
        {
            Instantiate(blobInstante,blocSpawn.transform.position,blobInstante.transform.rotation);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IPlayer player;
                if (collision.gameObject.TryGetComponent<IPlayer>(out player))
                {
                    player.Hit();
                    return;
                }

            }
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, maxFollowDistance);
        }
    }
}
