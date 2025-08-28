using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class Dropper : MonoBehaviour
    {
        [SerializeField] GameObject ItemToDrop;
        [SerializeField][Range(0,1)]float Rate = 1.0f;

        private void OnDestroy()
        {
            if (Random.value < Rate)
                Instantiate(ItemToDrop, transform.position, Quaternion.identity);
        }

    }
}
