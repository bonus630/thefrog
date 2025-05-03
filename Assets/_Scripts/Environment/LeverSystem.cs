using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class LeverSystem : IActivator
    {
        [SerializeField] GameObject Rib;

        IEnumerator Drop()
        {

            yield return new WaitForSeconds(1);
      
            Rib.GetComponent<Rigidbody2D>().gravityScale = 1;

        }

        public override void Activate()
        {

        }

        public override void Deactive()
        {
            GameManager.Instance.EventCompleted(GameEventName.DuckPath);
            StartCoroutine(Drop());
        }
    }
}
