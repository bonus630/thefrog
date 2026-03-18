using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class PlayerCheckArea : MonoBehaviour
    {
        public bool Checked { get; private set; } = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Checked = true;
            }
        }
    }
}
