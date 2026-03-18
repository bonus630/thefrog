using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Chest : MonoBehaviour, 
        IInteract
    {
        [SerializeField] GameObject Item;
        [SerializeField] GameObject Effect;
        [SerializeField] protected string chestID;
        Animator anim;
        AudioSource audioSource;
        bool closed = true;
        private void Awake()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        private void Start()
        {
            gameObject.SetActive(!GameManager.Instance.IsOpened(chestID));
        }
        public void Drop()
        {

            GameObject instance = Instantiate(Item, new Vector2(transform.position.x, transform.position.y + 0.4f), Item.transform.rotation);
            GameObject effect = Instantiate(Effect, instance.transform);
            Destroy(gameObject);
            GameManager.Instance.PlayerStates.ChestsID.Add(chestID);
        }
        public bool ReadyToInteract(bool lookFor)
        {
            return true;
        }
        public void Interact()
        {
            if (closed)
            {
                closed = false;
                anim.SetTrigger("Open");
                audioSource.Play();
            }
        }
        public Transform GetTransform()
        {
            return transform;
        }

    }
}
