using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    internal class EnableGameObject : IActivator
    {
        [SerializeField] GameObject externGameObject;
        [SerializeField] bool permanent = false;
        public override void Activate()
        {
            if(externGameObject!=null)
                externGameObject.SetActive(true);
            gameObject.SetActive(true);
        }

        public override void Deactive()
        {
            if (!permanent)
            {
                if (externGameObject != null)
                    externGameObject.SetActive(false);
                gameObject.SetActive(false);
            }
        }
    }
}
