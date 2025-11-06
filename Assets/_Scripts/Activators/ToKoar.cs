using System;
using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class ToKoar : MonoBehaviour
    {
        [SerializeField] public ScreenEffects screenEffects;
        [SerializeField] public Vector2 newLocation;
        [SerializeField] float playerWaitTime = 0.001f;

        [SerializeField] CollisionRelayEx upGame;
        [SerializeField] CollisionRelayEx downGame;
        [SerializeField] GameObject koarLimiter;
        //GameObject player;
        [SerializeField] GameObject koar;
        [SerializeField] float distance = 4.5f;
        bool inProgress = false;
        bool startCheckDistance = false;

        float timeLimit = 4f;
        float time = 0;
        int first = -1;
        int last = -1;
        public void Awake()
        {
            koar = GameObject.Find("KoarActivator").transform.GetChild(0).gameObject;
           // Debug.LogError($"[ToKoar] koarActivator {name} — frame {Time.frameCount}\n{new System.Diagnostics.StackTrace(true)}");
           // screenEffects = ServiceLocator.Instance.Get<ScreenEffects>();
            //Debug.LogError($"[ToKoar] screen {name} — frame {Time.frameCount}\n{new System.Diagnostics.StackTrace(true)}");
        }
        public void Start()
        {
           // Debug.LogError($"[ToKoar] {name} Start — frame {Time.frameCount}\n{new System.Diagnostics.StackTrace(true)}");
           // yield return null;
          
            // upGame = transform.GetChild(0).GetComponent<CollisionRelayEx>();
            //  downGame = transform.GetChild(1).GetComponent<CollisionRelayEx>();
            upGame.OnTriggerEnterAction += CheckEvent;
           // Debug.LogError($"[ToKoar] upGame {name} — frame {Time.frameCount}\n{new System.Diagnostics.StackTrace(true)}");
            downGame.OnTriggerEnterAction += CheckEvent;
           // Debug.LogError($"[ToKoar] downGame {name} — frame {Time.frameCount}\n{new System.Diagnostics.StackTrace(true)}");
       
    

        }
        void Update()
        {
            if (startCheckDistance)
            {
                if (Vector2.Distance(ServiceLocator.Instance.Get("Player").transform.position, newLocation) > distance)
                {
                    ServiceLocator.Instance.Get<IPlayer>().FallsControl();
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
            // Debug.Log("feather touch" + GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch));
            inProgress = true;
            //Debug.Log("Collision to koar");
          //  koar = GameObject.Find("KoarActivator").transform.GetChild(0).gameObject;
            koar.SetActive(true);
            if (GameManager.Instance.IsEventCompleted(GameEventName.KoarFounded))
                StartCoroutine(SimpleTransition(true));
            else
                StartCoroutine(PlayCutscene());

        }
        private void Deactive()
        {
            // Debug.Log("ToKoar Deactive: ");
            if (inProgress)
                return;
            inProgress = true;
          //  koar = GameObject.Find("KoarActivator").transform.GetChild(0).gameObject;
            koar.SetActive(false);
            StartCoroutine(SimpleTransition(false));
        }
        private void CheckEvent(ColliderData data)
        {
            //Debug.Log("[tokoar] first: " + first);
            //Debug.Log("[tokoar] ColliderData: " + data.Index);
            if (data.Index == last)
                return;
            last = data.Index;
            if (!CheckToContinue(data.ColliderOther))
                return;
            if (first == -1)
            {
                first = data.Index;
                return;
            }
            // player = data.Collider.gameObject;
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
        private bool CheckToContinue(Collider2D coll)
        {
            if (!GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
                return false;
            if (!coll.CompareTag("Player"))
            {
                if (!coll.transform.ContainsChildren("Player"))
                    return false;
            }
            return true;
        }
        private void ResetTimer()
        {
            time = 0;
            first = -1;
        }
        IEnumerator FixX()
        {
            yield return new WaitForSeconds(0.2f);
            ServiceLocator.Instance.Get("Player").transform.position = new Vector3(91f, GameManager.Instance.GetPlayer.transform.position.y, 0);
        }



        private IEnumerator PlayCutscene()
        {
            screenEffects.FadeOut(1f);
            koarLimiter.SetActive(false);
            //GameManager.Instance.GetPlayerScript.MoveInputOn = false;
            ServiceLocator.Instance.Get<IPlayer>().AllInputsOn(false, 0);
            ServiceLocator.Instance.Get<IPlayer>().RemoveGravity(true);
            // yield return new WaitForSeconds(0.2f);
            // player.transform.position = new Vector3(87f, player.transform.position.y, 0);
            //GameManager.Instance.GetPlayerScript.ChangeGravity(0);
            // GameObject.Find("Kaor").SetActive(true);
            yield return new WaitForSeconds(1);
            // FindAnyObjectByType<CameraBackground>().ChangeBackground();
            //GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = 0.2f;
            ServiceLocator.Instance.Get("Player").transform.position = newLocation;
            ServiceLocator.Instance.Get<IPlayer>().RemoveGravity(false);
            screenEffects.FadeIn(1);
            yield return new WaitForEndOfFrame();
            startCheckDistance = true;
            inProgress = false;
            // yield return new WaitForSeconds(6);

        }
        private IEnumerator SimpleTransition(bool toKoar = false)
        {
            // Debug.Log("Simple transition, to koar: "+ toKoar);
            screenEffects.FadeOut(0.1f);
            yield return new WaitForSeconds(0.1f);
            //  float ligth = 0.6f;
            if (toKoar)
            {
                FindAnyObjectByType<CameraBackground>().ChangeBackground();
                // ligth = 0.2f;
            }
            else
                FindAnyObjectByType<CameraBackground>().RestoreBackground();
            // GameObject.Find("Global Light 2D").GetComponent<Light2D>().intensity = ligth;
            screenEffects.FadeIn(0.1f);
            yield return new WaitForEndOfFrame();
            inProgress = false;
        }

        private void OnDestroy()
        {
            upGame.OnTriggerEnterAction -= CheckEvent;
            downGame.OnTriggerEnterAction -= CheckEvent;
        }
    }

}

