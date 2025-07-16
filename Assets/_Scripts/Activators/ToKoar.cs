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
        bool startCheckDistance = false;

        public void Awake()
        {
            screenEffects = FindAnyObjectByType<ScreenEffects>();
            koar = GameObject.Find("KoarActivator").transform.GetChild(0).gameObject;
        }
        void Update()
        {
            if(startCheckDistance)
            {
                if (Vector2.Distance(player.transform.position, newLocation) > 4.5)
                {
                    GameManager.Instance.GetPlayerScript.FallsControl();
                    startCheckDistance = false;
                }
            }
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
            GameManager.Instance.GetPlayerScript.MoveInputOn = false;
            // yield return new WaitForSeconds(0.2f);
            // player.transform.position = new Vector3(87f, player.transform.position.y, 0);
            screenEffects.FadeOut(0.1f);
            //GameManager.Instance.GetPlayerScript.ChangeGravity(0);
            // GameObject.Find("Kaor").SetActive(true);
            yield return new WaitForSeconds(1);
            FindAnyObjectByType<CameraBackground>().ChangeBackground();
            GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.2f;
            player.transform.position = newLocation;
            screenEffects.FadeIn(1);
            yield return new WaitForEndOfFrame();
            startCheckDistance = true;
           
  
        }
    }
}

