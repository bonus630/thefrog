using UnityEngine;

namespace br.com.bonus630.tests

{
    public class testeSeno : MonoBehaviour
    {
        Rigidbody2D rb;
        float time = 0;
        [SerializeField] float speed = 0.1f;
        [SerializeField] float factor = 0;
        [SerializeField] float duration = 10;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            rb = GetComponent<Rigidbody2D>();

        }

        // Update is called once per frame
        void Update()
        {

            time += Time.deltaTime / duration;
            if (time > 1)
                time = 1;
            factor = Mathf.Sin(time * Mathf.PI / 2);

            rb.linearVelocityX = speed * factor;
        }
    }
}
