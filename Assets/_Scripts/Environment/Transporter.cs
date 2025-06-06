using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Transporter : MonoBehaviour
    {
        [SerializeField] Vector2[] destines;
        [SerializeField] float travelDuration = 2f;
        [SerializeField] float speed = 10f;
        [SerializeField] float currentSpeed;
        [SerializeField] Sprite OnSprite;
        [SerializeField] Sprite OffSprite;
        [SerializeField] bool stopOnPlayerOut = false;
        [SerializeField] float stopMaxTime = 1f;
        SpriteRenderer render;

        bool going = false;
        bool playerOut = true;
        float time = 0;
        float stopTime = 0;
        private Vector3 startPosition;
        private Vector3 worldDestination;
        private Vector3 direction;
        private float totalDistance;
        private List<Vector2> destinesIntern;


        int currentDestine = 1;

        void Start()
        {
            destinesIntern = new List<Vector2>();
            destinesIntern.Add(transform.position);
            destinesIntern.AddRange(destines);
            SetPositions();
            render = GetComponent<SpriteRenderer>();
        }
        private void SetPositions()
        {
            startPosition = transform.TransformPoint(transform.position);
            worldDestination = new Vector3(destinesIntern[currentDestine].x, destinesIntern[currentDestine].y, transform.position.z);
            direction = (worldDestination - startPosition).normalized;
            totalDistance = Vector3.Distance(startPosition, worldDestination);
        }
        void Update()
        {
            if (going)
            {
                Vector3 currentPosition = transform.position;
                float distanceToTarget = Vector3.Distance(currentPosition, worldDestination);

                currentSpeed = CalculateSpeed(distanceToTarget);

                transform.position = Vector3.MoveTowards(transform.position, worldDestination, currentSpeed * Time.deltaTime);
                if (stopOnPlayerOut && playerOut)
                {
                    stopTime += Time.deltaTime;
                    if(stopTime>stopMaxTime)
                    {
                        going = false;
                        render.sprite = OffSprite;
                    }
                }
                // transform.Translate(direction * Time.deltaTime * speed);
                //time += Time.deltaTime;
                //float t = Mathf.Clamp01(time / travelDuration); 
                //Vector3 r = Vector3.Lerp(startPosition, worldDestination, t);
                ////transform.position = r;
                //transform.Translate(worldDestination,);

                //if (t >= 1)
                //{
                //    if (currentDestine >= destines.Length)
                //    {
                //        going = false;
                //        render.sprite = OffSprite;
                //    }
                //    else
                //    {
                //        currentDestine++;
                //        SetPositions();

                //    }
                //}
                // Debug.Log("Distance:" +  worldDestination);
                if (Vector3.Distance(transform.position, worldDestination) < 0.001f)
                {
                    // Debug.Log("Estou aqui ");
                    if (currentDestine >= destinesIntern.Count - 1)
                    {
                        going = false;
                        render.sprite = OffSprite;
                        currentDestine = 1;
                        destinesIntern.Reverse();
                    }
                    else
                    {
                        currentDestine++;
                    }
                    SetPositions();
                }
            }
        }
        float CalculateSpeed(float distanceToTarget)
        {
            float startSlowingDistance = totalDistance * 0.2f;

            if (distanceToTarget > startSlowingDistance)
                return speed;
            float progress = distanceToTarget / startSlowingDistance;
            float multiplier = Mathf.Pow(0.5f, 1f - progress);

            return speed * multiplier;
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                going = true;
                render.sprite = OnSprite;
                playerOut = false;
                stopTime = 0;
            }
        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                if (stopOnPlayerOut)
                {

                    playerOut = true;
                   
                }
            }
        }
    }
}
