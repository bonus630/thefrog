using System.Collections;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class FrammableObject : Frammabla, IHitReaction
    {
        [SerializeField] int fires = 2;
        bool burnning = false;
        Bounds bounds;
        void Start()
        {
            bounds = GetComponent<SpriteRenderer>().bounds;
        }

        public void OnHit(Collision2D collision)
        {
            if (collision.collider.TryGetComponent<IElement>(out IElement el) && el.GetElement == Elements.Fire && !burnning && el.isActived)
            {
                StartCoroutine(Burn());
            }
        }
        private IEnumerator Burn()
        {
            burnning = true;
            for (int i = 0; i < fires; i++)
            {
                Vector2 pos = bounds.RandomVector2();
                Instantiate(fire, pos, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);

            }
            yield return null;
            Destroy(gameObject);
        }
    }
}
