using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class LeverSystem : IActivator
    {
        [SerializeField] GameObject Rib;

        private void Start()
        {
            if(GameManager.Instance.IsEventCompleted(GameEventName.DuckPath))
                Rib.GetComponent<Rigidbody2D>().gravityScale = 1;
        }
        IEnumerator Drop()
        {

            yield return new WaitForSeconds(0.5f);
      
            Rib.GetComponent<Rigidbody2D>().gravityScale = 1;

        }

        public override void Activate()
        {
            GameManager.Instance.EventCompleted(GameEventName.DuckPath);
            StartCoroutine(Drop());
        }

        public override void Deactive()
        {
           
        }
    }
}
