using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class Dropper : IActivator
    {
        [SerializeField] GameObject ItemToDrop;
        [SerializeField][Range(0,1)]float Rate = 1.0f;

        public override void Activate()
        {
            Drop();
        }

        public override void Deactive()
        {
        }

        private void Drop()
        {
         

            if (ItemToDrop == null)
            {
                return;
            }

            if (Random.value < Rate)
            {
                Instantiate(ItemToDrop, transform.position, Quaternion.identity);
            }
        }

    }
}
