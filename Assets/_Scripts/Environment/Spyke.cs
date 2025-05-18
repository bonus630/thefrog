using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Spyke : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(!collision.gameObject.CompareTag("Player"))
            {

            }
        }
    }
}
