using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class BossBattle : MonoBehaviour
    {

        [SerializeField] private AudioClip bossMusic;
        //[SerializeField] private BoxCollider2D collider2;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject boss;
        [SerializeField] private GameObject bossBreak;
        [SerializeField] private Transform bossPoint;
        [SerializeField] private BatSpawner batSpawner;

        private bool monitor = false;
        private bool startBattle = false;
        private float distance = 0f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            if (GameManager.Instance.IsEventCompleted(GameEventName.KillPig))
                gameObject.SetActive(false);
            //adicionar um fad out
            //GameObject.Find("Listener").GetComponent<AudioSource>().Stop();
        }

        // Update is called once per frame
        void Update()
        {
            if (monitor && !startBattle)
            {
                distance = Vector2.Distance(player.gameObject.transform.position, bossPoint.position);
                audioSource.volume = 1 / distance;
                if (distance < 3)
                {
                    audioSource.volume = 0.9f;
                    startBattle = true;
                    Instantiate(boss, bossPoint.position, boss.transform.rotation);
                    batSpawner.startBattle = true;
                    bossBreak.SetActive(true);
                }
                // Debug.Log(distance);
            }

        }
        public void EndBattle()
        {
            audioSource.Stop();
            startBattle = false;
            batSpawner.startBattle = false;
            Destroy(gameObject);

        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                monitor = true;
                FindAnyObjectByType<MusicSource>().Stop();
                audioSource.resource = bossMusic;
                audioSource.Play();
            }
        }
    }
}
