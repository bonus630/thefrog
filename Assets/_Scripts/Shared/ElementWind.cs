using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class ElementWind : MonoBehaviour , IElement
    {
        public virtual Elements GetElement => Elements.Wind;
        public virtual Color ElementColor => Color.green;

        public bool isActived { get; set; }

        public virtual Elements CanActiveBy() => Elements.Wind;
        public virtual Elements CanDeactiveBy() => Elements.Earth;
        public abstract void ActiveBy(Elements element);
        public abstract void DeactiveBy(Elements element);
        public abstract void ActiveDeactive(bool active);
       
    }
}
