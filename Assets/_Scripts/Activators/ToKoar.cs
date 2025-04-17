using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class ToKoar : MonoBehaviour
    {
        GameObject player;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && GameManager.Instance.IsEventCompleted(GameEventName.FeatherTouch))
            {
                player = collision.gameObject;
                StartCoroutine(FixX());
            }
        }
        IEnumerator FixX()
        {
            yield return new WaitForSeconds(0.2f);
            player.transform.position = new Vector3(91f, player.transform.position.y, 0);
        }
    }
}

