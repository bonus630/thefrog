using System;
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
        [SerializeField] enableType enableType = enableType.Self;
        public override void Activate()
        {
            //Debug.Log($"[EnableGameObject] name:{externGameObject.name} enable:{invert}");
            if(externGameObject!=null && enableType.HasFlag(enableType.Extern))
                externGameObject.SetActive(invert ? false : true);
            if (enableType.HasFlag(enableType.Self))
            {
                //Debug.Log($"[EnableGameObject] Active name:{gameObject.name} enable:"+(invert ? false : true));
                gameObject.SetActive(invert ? false : true);
            }

        }

        public override void Deactive()
        {
            if (!permanent)
            {
                if (externGameObject != null && enableType.HasFlag(enableType.Extern))
                    externGameObject.SetActive(invert ? true : false);
                if (enableType.HasFlag(enableType.Self))
                {
                    //Debug.Log($"[EnableGameObject] Deactive name:{gameObject.name} enable:" + (invert ? false : true));
                    gameObject.SetActive(invert ? true : false);
                }
            }
        }
    }
    [Flags]
    public enum enableType
    {
        Extern = 0b0001,
        Self   = 0b0010
    }
}
