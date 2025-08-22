using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators

{
    public sealed class BreakInContactc : MonoBehaviour
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
                GetComponent<BoxCollider2D>().enabled = false;
                effect.Play();
                GetComponent<AudioSource>().PlayOneShot(wallBreak);

                StartCoroutine(DestroyEffects());
               

            }
        }
        IEnumerator DestroyEffects()
        {
            yield return new WaitForEndOfFrame();
            screenEffects.StartCameraShake(2,2);
            screenEffects.GamepadShake(0.5f, 0.1f);
            yield return new WaitForSeconds(1f);
            screenEffects.StopCameraShake();
            screenEffects.GamepadShake(0f, 0f);
            Destroy(gameObject);

        }

    }
}
