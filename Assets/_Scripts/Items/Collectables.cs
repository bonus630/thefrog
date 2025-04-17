using br.com.bonus630.thefrog.Manager;
using UnityEditor;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Collectables : CollectablesBase
    {

        [SerializeField] private GameObject collectableEffect;
        [SerializeField] private AudioClip collectSFX;

        private AudioSource audioSource;



        protected override void Start()
        {
            base.Start();
            audioSource = GetComponent<AudioSource>();
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                audioSource.PlayOneShot(collectSFX);
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
                gameObject.GetComponent<CircleCollider2D>().enabled = false;
                collectableEffect.SetActive(true);
                if (Amount != 0)
                {
                    GameManager.Instance.PlayerStates.CollectablesID.Add(itemID);
                    GameManager.Instance.PlayerStates.Collectables++;
                    GameManager.Instance.UpdateScore();
                }
                Destroy(gameObject, 0.12f);

            }
        }

    }
}
 