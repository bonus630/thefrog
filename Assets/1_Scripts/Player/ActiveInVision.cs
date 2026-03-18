using br.com.bonus630.thefrog.Player;
using UnityEngine;

namespace br.com.bonus630.thefrog
{
    public class ActiveInVision : MonoBehaviour
    {
        [SerializeField] GameObject active;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("active visiso:"+collision.gameObject.name);
            if(collision.TryGetComponent<VisionController>(out var vision))
            {
                Debug.Log("active visiso22:" + vision.InVision(collision));
                active.SetActive(vision.InVision(GetComponent<Collider2D>()));
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            active.SetActive(false);
            Debug.Log("deactive visiso:" + collision.gameObject.name);
        }
    }
}
