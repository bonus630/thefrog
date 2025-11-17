using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(CollisionRelayEx))]
    public class SimulateCollisionRelayEx : IActivator
    {
        CollisionRelayEx col;
        private void Start()
        {
            col = GetComponent<CollisionRelayEx>();
        }
        public override void Activate()
        {
            col.OnTriggerEnter2D(null);
        }

        public override void Deactive()
        {
        }
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size);
            
        }
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(GetComponent<Collider2D>().bounds.center, GetComponent<Collider2D>().bounds.size - new Vector3(2,2,2));
        }

    }
}
