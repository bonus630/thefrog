using System.Collections;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Transporter : MonoBehaviour
    {
        [SerializeField] Vector2[] destines;
        [SerializeField] float travelDuration = 2f;

        [SerializeField] Sprite OnSprite;
        [SerializeField] Sprite OffSprite;
        SpriteRenderer render;

        bool going = false;
        float time = 0;
        private Vector3 startPosition;
        private Vector3 worldDestination; 

        int currentDestine = 0;

        void Start()
        {
            
            SetPositions();
            render = GetComponent<SpriteRenderer>();
        }
        private void SetPositions()
        {
            startPosition = transform.TransformPoint(transform.position);
            worldDestination = new Vector3(destines[currentDestine].x, destines[currentDestine].y, transform.position.z);
        }
        void Update()
        {
            if (going)
            {
                time += Time.deltaTime;
                float t = Mathf.Clamp01(time / travelDuration); 
                Vector3 r = Vector3.Lerp(startPosition, worldDestination, t);
                transform.position = r;

                if (t >= 1)
                {
                    if (currentDestine >= destines.Length)
                    {
                        going = false;
                        render.sprite = OffSprite;
                    }
                    else
                    {
                        currentDestine++;
                        SetPositions();

                    }
                }
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                going = true;
                render.sprite = OnSprite;
            }
        }
    }
}
