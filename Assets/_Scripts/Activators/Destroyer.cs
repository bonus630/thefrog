
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class Destroyer : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Destroy(collision.gameObject);
        }
    }
}

