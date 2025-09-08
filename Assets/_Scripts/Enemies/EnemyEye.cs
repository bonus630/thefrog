using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyEye : MonoBehaviour , IElement
    {
        [SerializeField] float[] PositionX;
        [SerializeField] float speed;
        Vector2 destine;
        int current = 0;

        public void ActiveBy(Elements element)
        {
            if (element.Equals(Elements.Lightining))
                Destroy(gameObject);
        }

        public Elements CanActiveBy() => Elements.Lightining;

        public Elements CanDeactiveBy() => Elements.Lightining;

        public void DeactiveBy(Elements element)
        {
            
        }

        public Elements GetElement() => Elements.Lightining;

        private void Start()
        {
            destine = new Vector2(PositionX[current], transform.position.y);
        }
        private void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position, destine, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, destine) < 0.001f)
            {
                current = current == 0 ? 1 : 0;
                destine = new Vector2(PositionX[current], transform.position.y);
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("Eye collision");
        }
    }
}
