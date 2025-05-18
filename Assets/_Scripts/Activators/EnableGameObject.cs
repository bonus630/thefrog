using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    internal class EnableGameObject : IActivator
    {
        [SerializeField] bool permanent = false;
        public override void Activate()
        {
            gameObject.SetActive(true);
        }

        public override void Deactive()
        {
            if(!permanent)
            gameObject.SetActive(false);
        }
    }
}
