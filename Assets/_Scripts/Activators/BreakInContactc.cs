using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators

{
    public class BreakInContactc : MonoBehaviour
    {
        [SerializeField] private AudioClip wallBreak;
        [SerializeField] private ParticleSystem effect;
        [SerializeField] private ScreenEffects screenEffects;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IEnemy>(out IEnemy pig) && pig.IsDied)
            {
               // Debug.Log("Breakincontactc triggerenter");
                pig.DestroySelf();
                spriteRenderer.enabled = false;
                effect.Play();
                GetComponent<AudioSource>().PlayOneShot(wallBreak);
                screenEffects.ScreenShake();
                Destroy(gameObject, 2.5f);

            }
        }

    }
}
