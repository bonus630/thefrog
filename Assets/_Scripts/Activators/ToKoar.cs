using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace br.com.bonus630.thefrog.Activators
{
    public class ToKoar : MonoBehaviour
    {
        [SerializeField] public ScreenEffects screenEffects;
        [SerializeField] public Vector2 newLocation;
        [SerializeField] float playerWaitTime = 0.001f;

        GameObject player;
        GameObject koar;
        bool actived = false;

        public void Awake()
        {
            screenEffects = FindAnyObjectByType<ScreenEffects>();
            koar = GameObject.Find("KoarActivator").transform.GetChild(0).gameObject;
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("feather touch" + GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch));
            if (collision.CompareTag("Player") && GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
            {
                actived = true;
                Debug.Log("Collision to koar");
                player = collision.gameObject;
                koar.SetActive(true);
                //player.GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
                StartCoroutine(PlayCutscene());
            }
        }
        IEnumerator FixX()
        {
            yield return new WaitForSeconds(0.2f);
            player.transform.position = new Vector3(91f, player.transform.position.y, 0);
        }
       


        private IEnumerator PlayCutscene()
        {
            GameManager.Instance.GetPlayerScript.InputsOn = false;
            // yield return new WaitForSeconds(0.2f);
            // player.transform.position = new Vector3(87f, player.transform.position.y, 0);
            screenEffects.FadeOut(1);
            //GameManager.Instance.GetPlayerScript.ChangeGravity(0);
          // GameObject.Find("Kaor").SetActive(true);
            FindAnyObjectByType<CameraBackground>().ChangeBackground();
            GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.2f;
            player.transform.position = newLocation;
            yield return new WaitForSeconds(playerWaitTime);
            GameManager.Instance.GetPlayerScript.RemoveGravity(true);
            Debug.Log("PlayCutscene");
            yield return new WaitForSeconds(1.5f); // opcional, uma pausa dramática
            screenEffects.FadeIn(1);
            GameManager.Instance.GetPlayerScript.RemoveGravity(false);
            yield return new WaitForSeconds(0.5f);
       
            GameManager.Instance.GetPlayerScript.FallsControl();
        }
    }
}

