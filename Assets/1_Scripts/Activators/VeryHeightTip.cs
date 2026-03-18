using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public sealed class VeryHeightTip : TipsBase
    {
        [SerializeField] GameObject KoarActivator;

        protected override void Awake()
        {
            base.Awake();
            this.boxCollider.enabled = true;
        }
        
    }
}
