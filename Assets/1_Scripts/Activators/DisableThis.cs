using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class DisableThis : MonoBehaviour
    {
        [SerializeField] GameObject toDisable;
        [SerializeField][Tooltip("Use -1 to disable by collider")] float time = -1;
        float timer = 0;

        void Update()
        {
            if (time < 0)
                return;
            timer += Time.deltaTime;
            if (timer > time && toDisable != null)
                toDisable.SetActive(false);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (toDisable != null)
            {
                if (collision.CompareTag(toDisable.tag))
                {
                    toDisable.SetActive(false);
                }
            }
        }
    }
}
