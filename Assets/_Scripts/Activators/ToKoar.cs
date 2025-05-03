using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace br.com.bonus630.thefrog.Activators
{
    public class ToKoar : MonoBehaviour
    {
        GameObject player;

        public void Awake()
        {
            fader = FindAnyObjectByType<ScreenFader>(); 
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("feather touch" + GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch));
            if (collision.CompareTag("Player") && GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
            {
                Debug.Log("Collision to koar");
                player = collision.gameObject;
                StartCoroutine(PlayCutscene());
            }
        }
        IEnumerator FixX()
        {
            yield return new WaitForSeconds(0.2f);
            player.transform.position = new Vector3(91f, player.transform.position.y, 0);
        }
        public ScreenFader fader;
        public Vector2 newLocation;
       


        private IEnumerator PlayCutscene()
        {
           // yield return new WaitForSeconds(0.2f);
           // player.transform.position = new Vector3(87f, player.transform.position.y, 0);
            yield return fader.FadeOut();
            FindAnyObjectByType<CameraBackground>().ChangeBackground();
            GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.2f;
            player.transform.position = newLocation;
            yield return new WaitForSeconds(1.5f); // opcional, uma pausa dramática
            yield return fader.FadeIn();
            
        }
    }
}

