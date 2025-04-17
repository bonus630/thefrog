using System.Collections;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class LeverSystem : MonoBehaviour
    {
        [SerializeField] GameObject leverOn;
        //SerializeField] GameObject leverOff;
        [SerializeField] GameObject Rib;

        int OffID = Animator.StringToHash("Off");

        private void OnCollisionEnter2D(Collision2D collision)
        {
            GetComponent<AudioSource>().Play();
            leverOn.GetComponent<Animator>().SetTrigger(OffID);
            GameManager.Instance.EventCompleted(GameEventName.DuckPath);
            StartCoroutine(Drop());

        }

        IEnumerator Drop()
        {

            yield return new WaitForSeconds(1);
            // leverOn.gameObject.SetActive(false);
            // leverOff.gameObject.SetActive(true);
            GetComponent<BoxCollider2D>().enabled = false;
            Rib.GetComponent<Rigidbody2D>().gravityScale = 1;

        }

    }
}
