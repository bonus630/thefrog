using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Rendering.Universal;
namespace br.com.bonus630.thefrog.Activators
{
    public class ToKoar : MonoBehaviour
    {
        [SerializeField] public ScreenEffects screenEffects;
        [SerializeField] public Vector2 newLocation;
        [SerializeField] float playerWaitTime = 0.001f;

        [SerializeField] GameObject upGame;
        [SerializeField] GameObject downGame;

        GameObject player;
        GameObject koar;
        bool inProgress = false;
        bool startCheckDistance = false;

        float timeLimit = 4f;
        float time = 0;
        int first = -1;

        public void Awake()
        {
            screenEffects = FindAnyObjectByType<ScreenEffects>();
            koar = GameObject.Find("KoarActivator").transform.GetChild(0).gameObject;
            upGame.GetComponent<CollisionRelayEx>().OnTriggerEnterAction += CheckEvent;
            downGame.GetComponent<CollisionRelayEx>().OnTriggerEnterAction += CheckEvent;
        }
        void Update()
        {
            if (startCheckDistance)
            {
                if (Vector2.Distance(player.transform.position, newLocation) > 4.5)
                {
                    GameManager.Instance.GetPlayerScript.FallsControl();
                    startCheckDistance = false;
                }
            }
            if (time > timeLimit)
            {
                ResetTimer();
            }
            if (first != -1)
                time += Time.deltaTime;
        }

        private void Active(bool firstTime = true)
        {
            if (inProgress)
                return;
            Debug.Log("feather touch" + GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch));
            inProgress = true;
            Debug.Log("Collision to koar");
            koar.SetActive(true);
            if (GameManager.Instance.IsEventCompleted(GameEventName.DefeatWizard))
                StartCoroutine(SimpleTransition());
            else
                StartCoroutine(PlayCutscene());

        }
        private void Deactive()
        {
            if (inProgress)
                return;
            inProgress = true;
            koar.SetActive(false);
            StartCoroutine(SimpleTransition(false));
        }
        private void CheckEvent(ColliderData data)
        {
            if (!data.Collider.CompareTag("Player") || !GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
                return;

            if (first == -1)
            {
                first = data.Index;
                return;
            }
            player = data.Collider.gameObject;
            if (first < data.Index)
            {
                Active();
            }
            else
            {
                Deactive();
            }

            first = -1;

        }
        private void ResetTimer()
        {
            time = 0;
            first = -1;
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
            inProgress = false;

        }
        private IEnumerator SimpleTransition(bool toKoar = false)
        {
            screenEffects.FadeOut(0.1f);
            yield return new WaitForSeconds(1);
            float ligth = 0.6f;
            if (toKoar)
            {
                FindAnyObjectByType<CameraBackground>().ChangeBackground();
                ligth = 0.2f;
            }
            else
                FindAnyObjectByType<CameraBackground>().RestoreBackground();
            GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = ligth;
            screenEffects.FadeIn(1);
            yield return new WaitForEndOfFrame();
            inProgress = false;
        }
    }

}

