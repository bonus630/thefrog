using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class Checkpoint : MonoBehaviour
    {
        protected bool active = false;


        protected void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !active)
            {
                Check();
            }
        }
        protected virtual void Check()
        {
            // Debug.Log("base checkpoint");
            GameManager.Instance.PlayerStates.PlayerPosition.Position = gameObject.transform.position;
            GameManager.Instance.SaveStates();
            active = true;
        } 
        protected virtual void UnCheck()
        {
            active = false;
        }
    }
}

