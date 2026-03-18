using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class ElementLightning : MonoBehaviour , IElement
    {
        public bool isActived { get; set; }
        public virtual Elements GetElement => Elements.Lightining;
        public virtual Color ElementColor => Color.white;
        public virtual Elements CanActiveBy() => Elements.Lightining;
        public virtual Elements CanDeactiveBy() => Elements.Water;
        public abstract void ActiveBy(Elements element);
        public abstract void DeactiveBy(Elements element);
        public abstract void ActiveDeactive(bool active);
       
    }
}
