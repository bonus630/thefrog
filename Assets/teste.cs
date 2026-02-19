using UnityEngine;

namespace br.com.bonus630.thefrog
{
    public class teste : MonoBehaviour
    {
        [SerializeField]float speed = 10f;
        [SerializeField]float offset = 1;
        float startTime;
        private void Start()
        {
            startTime = Time.time;
        }
        private void Update()
        {
            float t = Time.time - startTime;
            float x = transform.position.x + Mathf.Sin(t * speed) * offset * Time.deltaTime;
            Vector2 pos = new Vector2(x, transform.position.y);
            transform.position = pos;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == 8)
                gameObject.GetComponent<teste>().enabled = false;
        }

    }
}
