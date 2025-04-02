using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class HightPlace : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            StartCoroutine(Active());
        }
        IEnumerator Active()
        {
            yield return new WaitForSeconds(3f);
            GameManager.Instance.EventCompleted(GameEventName.MysticScroll);
        }
    }
}
