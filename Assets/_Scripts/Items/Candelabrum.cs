using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    [RequireComponent(typeof(Animator))]
    public class Candelabrum : IActivator, IElement
    {
        [field: SerializeField] public Elements GetElement { get; set; } = Elements.Fire;
        [field: SerializeField] public Color ElementColor { get; set; } = Color.red;
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

        public Elements CanActiveBy() => Elements.Fire;

    
        public Elements CanDeactiveBy() => Elements.Water;

        public void ActiveBy(Elements element)
        {

            Activate();
        }

        public void DeactiveBy(Elements element)
        {
            Deactive();
        }
    }
}
