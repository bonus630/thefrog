using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{

    public class ShipDoor : Door, IInteract
    {
        [SerializeField] AudioClip openingAudio;
        [SerializeField] AudioClip closingAudio;
        [SerializeField] bool isOpen = true;
        [SerializeField] bool isExit;

        Animator anim;
        AudioSource audioSource;
        GameObject door;
        BoxCollider2D boxCollider;

        SpriteRenderer sr;

        bool inOperation = false;
        //talvez mudar para algo no  IInteract


        protected override void Awake()
        {
            base.Awake();
            audioSource = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            door = transform.GetChild(0).gameObject;
            boxCollider = GetComponent<BoxCollider2D>();
            sr = GetComponent<SpriteRenderer>();
            if (teleporter == null)
                isExit = true;
        }
        int cont = 0;
        protected override void Update()
        {
            if (InteractUp.WasPressedThisFrame() && inside)
            {
                if (player == null)
                    player = ServiceLocator.Instance.Get<IPlayer>();
                Debug.Log($"[DoorShip] inGround:{player.InGround} touching:{player.BodyTouching(boxCollider)}");
                inside = player.BodyTouching(boxCollider);
                if (!inside)
                    return;
                if (isOpen)
                {
                    Debug.Log($"[DoorShip] inside:{inside} isOpen:{isOpen}");
                    //var p = GameManager.Instance.GetPlayerScript;
                    if (player.InGround && player.BodyTouching(boxCollider))
                    {
                        Close();
                        return;
                    }
                }
                else
                    Open();

            }

        }
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log($"[DoorShip] EXIT {collision.name} | CompareTag={collision.CompareTag("Player")}");
            if (collision.CompareTag("Player"))
            {
                if (collision.TryGetComponent<IPlayer>(out player))
                {
                    Debug.Log("ipayer: " + player.InGround);
                    inside = true;
                    if (!player.InGround)
                        return;
                }
            }
        }
        //Método chamado pela animação
        public void Closed()
        {
            if (isExit)
                return;
            player.AllInputsOn(true, 0);
            //return;
            if (teleporter != null)
            {
                teleporter.Activate();
            }
        }

        public void Opened()
        {
            door.SetActive(true);
        }
        public void Close()
        {
            Debug.Log("[ShipDoor] cont:" + cont++);
            if (!isExit)
            {
                player.AllInputsOn(false, 0);
                Debug.Log($"[DoorShip] iplayer:{player}");
                GetComponent<SpriteRenderer>().sortingOrder = 11;
                StartCoroutine(EnablesDoor());
            }
            audioSource.PlayOneShot(closingAudio);
            anim.SetBool("Closed", true);
            isOpen = false;
            Closed();
            door.SetActive(isOpen);
        }
        public void Open()
        {
            Debug.Log("[ShipDoor] cont:" + cont++);
            audioSource.PlayOneShot(openingAudio);
            anim.SetBool("Closed", false);
            isOpen = true;
            Invoke(nameof(EnablesDoor2), 0.1f);
        }
        private IEnumerator EnablesDoor()
        {
            while (player.BodyTouching(boxCollider))
            {
                Debug.Log($"[DoorShip] Enumerator touching:{player.BodyTouching(boxCollider)}");
                yield return new WaitForSeconds(0.1f);
            }
            GetComponent<SpriteRenderer>().sortingOrder = 9;
            Debug.Log("Chegamos");
        }
        private void EnablesDoor2()
        {
            door.SetActive(isOpen);
        }

        public void Interact()
        {
            //if (!isOpen)
            //    Open();
        }

        public bool ReadyToInteract(bool lookFor) => true;

        public Transform GetTransform() => transform;
    }
}

