using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    internal class EnableGameObject : IActivator
    {
        [SerializeField] GameObject externGameObject;
        [SerializeField] bool permanent = false;
        [Tooltip("True for disable gameobject on active")]
        [SerializeField] bool invert = false;
        public override void Activate()
        {
            if(externGameObject!=null)
                externGameObject.SetActive(invert ? false : true);
            gameObject.SetActive(invert ? false : true);
        }

        public override void Deactive()
        {
            if (!permanent)
            {
                if (externGameObject != null)
                    externGameObject.SetActive(invert ? true : false);
                gameObject.SetActive(invert ? true : false);
            }
        }
    }
}
