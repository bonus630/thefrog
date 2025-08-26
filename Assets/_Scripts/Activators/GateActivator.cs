using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class GateActivator : IActivator
    {
        Animator anim;
        [SerializeField] bool Opened = false;
        readonly int OpenedID = Animator.StringToHash("Opened");
        void Start()
        {
            anim = GetComponent<Animator>();
        }

        public override void Activate()
        {
            Opened = true;
            anim.SetBool(OpenedID, Opened);
        }

        public override void Deactive()
        {

            Opened = false;
            anim.SetBool(OpenedID, Opened);
        }
    }
}
