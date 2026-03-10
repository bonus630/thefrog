using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class Frammabla : MonoBehaviour, IElement
    {
        [SerializeField]protected GameObject fire;
        [field: SerializeField] public Elements GetElement { get; set; } = Elements.Fire;
        [field: SerializeField] public Color ElementColor { get; set; } = Color.red;
        public bool isActived { get; set; }

        public Elements CanActiveBy() => Elements.Fire;
        public Elements CanDeactiveBy() => Elements.Water;


        public void ActiveBy(Elements element)
        {
            ActiveDeactive(true);
        }

        public void DeactiveBy(Elements element)
        {
            ActiveDeactive(false);
        }
        private void ActiveDeactive(bool active)
        {

        }

     
    }
}
