
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class ShipDoor : MonoBehaviour, IInteract
    {
        [SerializeField] AudioClip openingAudio;
        [SerializeField] AudioClip closingAudio;
        [SerializeField] bool isOpen = true;
        [SerializeField] bool isExit;

        Animator anim;
        AudioSource audioSource;
        SpriteRenderer sprite;
        GameObject door;
        BoxCollider2D boxCollider;
        Player player;


        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            anim = GetComponent<Animator>();
            door = transform.GetChild(0).gameObject;
            boxCollider = GetComponent<BoxCollider2D>();
        }
        //private void Update()
        //{
        //    var col = Physics2D.OverlapBox(transform.position, boxCollider.bounds.size, 0, 9);
        //    Debug.Log(col.gameObject.name);
        //    if(col && col.gameObject.TryGetComponent<Player>(out player) && player.inGround)
        //    {
        //        if (isOpen)
        //        {
        //            Close();
        //        }
        //        else
        //            Open();
        //    }
        //}
        //private void OnDrawGizmos()
        //{
        //    Gizmos.color = Color.blue;
        //    Gizmos.DrawCube(transform.position, boxCollider.bounds.size);
        //}
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (collision.TryGetComponent<Player>(out player))
                {
                    if (!player.inGround)
                        return;
                    if (isOpen)
                    {
                        Close();
                    }
                    else
                        Open();
                }
            }
        }
        //private void Update()
        //{
        //    if (canOperate && player !=null && player.inGround)
        //    {
        //        if (isOpen)
        //        {
        //            Close();
        //        }
        //        else
        //            Open();
        //    }
        //}
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if (collision.CompareTag("Player"))
        //        canOperate = false;
        //}

        public void Closed()
        {
            if (TryGetComponent<IActivator>(out IActivator tele))
            {
                tele.Activate();

            }

        }
        public void Opened()
        {
            door.SetActive(true);

        }
        public void Close()
        {
            audioSource.PlayOneShot(closingAudio);
            anim.SetBool("Closed", true);
            isOpen = false;
            Closed();
            if (isExit)
            {
                door.SetActive(false);
                GetComponent<BoxCollider2D>().enabled = false;
            }
            Invoke(nameof(EnablesDoor), 0.5f);
        }
        public void Open()
        {
            audioSource.PlayOneShot(openingAudio);
            anim.SetBool("Closed", false);
            isOpen = true;
            Invoke(nameof(EnablesDoor), 0.5f);
        }
        private void EnablesDoor()
        {
            transform.GetChild(0).gameObject.SetActive(isOpen);
        }

        public void Interact()
        {
            if (!isOpen)
                Open();
        }

        public bool ReadyToInteract(bool lookFor)
        {
            return true;
        }

        public Transform GetTransform()
        {
            return transform;
        }
    }
}

