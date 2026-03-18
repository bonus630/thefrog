using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class ElementFire : MonoBehaviour , IElement
    {
        public bool isActived { get; set; }
        public virtual Elements GetElement => Elements.Fire;
        public virtual Color ElementColor => Color.red;
        public virtual Elements CanActiveBy() => Elements.Fire;
        public virtual Elements CanDeactiveBy() => Elements.Water;
        public abstract void ActiveBy(Elements element);
        public abstract void DeactiveBy(Elements element);
        public abstract void ActiveDeactive(bool active);
       
    }
}
