using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Transporter : MonoBehaviour
    {
        [SerializeField] Vector2 destine;
        [SerializeField] float travelDuration = 2f;
        // BoxCollider2D coll;
        bool going = false;
        float time = 0;
        private Vector3 startPosition;
        private Vector3 worldDestination; // Destino convertido para posição global
        void Start()
        {
            startPosition = transform.TransformPoint(transform.position);
            worldDestination = new Vector3(destine.x, destine.y, transform.position.z);
            Debug.Log(startPosition);
            //coll = GetComponent<BoxCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            if (going)
            {

                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / travelDuration); // Garante que t fique entre 0 e 1

                Vector3 r = Vector3.Lerp(startPosition, worldDestination, t);
                transform.position = r;
                // Se chegou ao destino, para o movimento
                if (t >= 1)
                {
                    going = false;
                }
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                going = true;

            }
        }
    }
}
