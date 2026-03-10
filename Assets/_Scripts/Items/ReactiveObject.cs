using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    [RequireComponent(typeof(Collision2D))]
    public class ReactiveObject : MonoBehaviour
    {
        private IHitReaction[] reactions;

        void Awake()
        {
            reactions = GetComponents<IHitReaction>();
        }

        void OnCollisionEnter2D(Collision2D collision)
        {
            foreach (var r in reactions)
                r.OnHit(collision);
        }

    }


}
