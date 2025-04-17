using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Checkpoint : MonoBehaviour
    {
        private bool active = false;
        private Animator anim;
        //  Color white;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            anim = GetComponent<Animator>();
            //  white = new Color(1, 1, 1, 0.5f);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !active)
            {
                anim.SetBool("checked", true);
                GameManager.Instance.PlayerStates.PlayerPosition.Position = gameObject.transform.position;
                GameManager.Instance.SaveStates();
                active = true;

            }
        }
    }
}
