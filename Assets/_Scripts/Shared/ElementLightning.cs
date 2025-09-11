using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class ElementLightning : MonoBehaviour , IElement
    {
        public virtual Elements GetElement() => Elements.Lightining;
        public virtual Color GetElementColor() => Color.white;
        public virtual Elements CanActiveBy() => Elements.Lightining;
        public virtual Elements CanDeactiveBy() => Elements.Water;
        public abstract void ActiveBy(Elements element);
        public abstract void DeactiveBy(Elements element);
        public abstract void ActiveDeactive(bool active);
       
    }
}
