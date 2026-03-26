using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class DestroyEnimies : MonoBehaviour
    {
        [SerializeField] LayerMask layers;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
                Destroy(collision.gameObject);
        }
    }
}
