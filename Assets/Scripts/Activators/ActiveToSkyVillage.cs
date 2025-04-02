using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveToSkyVillage : MonoBehaviour
    {
        bool active = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !active)
            {
                active = true;
                StageBuilder builder = GetComponent<StageBuilder>();
                builder.Build();
            }
        }
    }
}
