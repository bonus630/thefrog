using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyEye : MonoBehaviour
    {
        [SerializeField] float[] PositionX;
        [SerializeField] float speed;
        Vector2 destine;
        int current = 0;
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
    }
}
