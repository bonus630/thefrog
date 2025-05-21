using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators

{
    public class BreakInContactc : MonoBehaviour
    {
        [SerializeField] private AudioClip wallBreak;
        [SerializeField] private ParticleSystem effect;

        private AudioSource audioSource;
        private SpriteRenderer spriteRenderer;

        private void Awake()
        {

            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent<IEnemy>(out IEnemy pig) && pig.IsDied)
            {
                pig.DestroySelf();
                spriteRenderer.enabled = false;
                effect.Play();
                FindAnyObjectByType<CamerasController>().ShakeCameraEffect();
                //StartCoroutine(ShakeCam());
                GetComponent<AudioSource>().PlayOneShot(wallBreak);
                Destroy(gameObject, 1f);

            }
        }
        //IEnumerator ShakeCam()
        //{
        //    yield return new WaitForEndOfFrame();
        //    GameObject.Find("Virtual Camera").GetComponent<Animator>().SetTrigger("Shake");
        //    yield return new WaitForEndOfFrame();
        //}

    }
}
