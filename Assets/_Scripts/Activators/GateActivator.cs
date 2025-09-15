using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class GateActivator : IActivator
    {
        Animator anim;
        AudioSource audioSource;
        [SerializeField] bool Opened = false;
        [SerializeField] string ID;
        readonly int OpenedID = Animator.StringToHash("Opened");
        void Start()
        {
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            if (GameManager.Instance.IsActived(ID))
                Activate();
        }

        public override void Activate()
        {
            Toggle(true);
        }

        public override void Deactive()
        {
            Toggle(false);
        }
        private void Toggle(bool open)
        {
            Opened = open;
            Actived = open;
            anim.SetBool(OpenedID, Opened);
            audioSource.Play();
            transform.GetChild(0).gameObject.SetActive(!Opened);
            GameManager.Instance.SetActived(ID, open);
        }
    }
}
