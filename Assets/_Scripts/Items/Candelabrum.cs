using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    [RequireComponent(typeof(Animator))]
    public class Candelabrum : IActivator
    {
        [SerializeField] private bool off = false;
        private readonly int OffID = Animator.StringToHash("Off");
        private Animator anim;
        public override void Activate()
        {
            anim.SetBool(OffID, false);
            GetComponent<AudioSource>().Play();
        }

        public override void Deactive()
        {
            anim.SetBool(OffID, false);
        }

        void Start()
        {
            anim = GetComponent<Animator>();
            anim.SetBool(OffID, off);
        }

       
    }
}
