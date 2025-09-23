using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Transporter : MonoBehaviour
    {
        public Vector2[] destines;
        [SerializeField] float travelDuration = 2f;
        [SerializeField] float speed = 10f;
        [SerializeField] float currentSpeed;
        [SerializeField] float stopMaxTime = 1f;
        [SerializeField] Sprite OnSprite;
        [SerializeField] Sprite OffSprite;
        [SerializeField] bool stopOnPlayerOut = false;
        [SerializeField] public bool active = true;
        SpriteRenderer render;

        public bool going = false;
        bool playerOut = true;
        float time = 0;
        float stopTime = 0;
        private Vector3 startPosition;
        private Vector3 worldDestination;
        private Vector3 direction;
        private float totalDistance;
        public List<Vector2> DestinesIntern { get; set; }
        public event Action OnePass;

        int currentDestine = 1;
        private Rigidbody2D rb;
        [ContextMenu("Adicionar posição atual")]
        private void AddCurrentPositionToStartPosition()
        {
            var list = new System.Collections.Generic.List<Vector2>();
            list.Add(transform.position);
            destines = list.ToArray();

        }

        void Start()
        {
            TryGetComponent<Rigidbody2D>(out rb);
            Init();
        }
        public void Init()
        {
            DestinesIntern = new List<Vector2>();
            DestinesIntern.Add(transform.position);
            DestinesIntern.AddRange(destines);
            SetPositions();
            render = GetComponent<SpriteRenderer>();
        }
        public void SetPositions()
        {
            if (!active) return;
            // startPosition = transform.TransformPoint(transform.position);
            startPosition = transform.position;
            worldDestination = new Vector3(DestinesIntern[currentDestine].x, DestinesIntern[currentDestine].y, transform.position.z);
            direction = (worldDestination - startPosition).normalized;
            totalDistance = Vector3.Distance(startPosition, worldDestination);
        }
        private void Update()
        {
            UpdateTransform();
        }
        //private void FixedUpdate()
        //{
        //    UpdatePhysics();
        //}
        private void UpdateTransform()
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
                    if (stopTime > stopMaxTime)
                    {
                        going = false;
                        render.sprite = OffSprite;
                    }
                }
          
                if (Vector3.Distance(transform.position, worldDestination) < 0.001f)
                {
                    // Debug.Log("Estou aqui ");
                    if (currentDestine >= DestinesIntern.Count - 1)
                    {
                        going = false;
                        render.sprite = OffSprite;
                        currentDestine = 1;
                        DestinesIntern.Reverse();
                        OnePass?.Invoke();
                    }
                    else
                    {
                        currentDestine++;
                    }
                    SetPositions();
                }
            }
        }
        private void UpdatePhysics()
        {
            if (going)
            {
                Vector3 currentPosition = transform.position;
                float distanceToTarget = Vector3.Distance(currentPosition, worldDestination);

                currentSpeed = CalculateSpeed(distanceToTarget);

                Vector3 nextPosition = Vector3.MoveTowards(transform.position, worldDestination, currentSpeed * Time.deltaTime);

                // 👉 Troque esta linha:
                // transform.position = Vector3.MoveTowards(...);
                // 👇 Por esta:
                rb.MovePosition(nextPosition);

                if (stopOnPlayerOut && playerOut)
                {
                    stopTime += Time.deltaTime;
                    if (stopTime > stopMaxTime)
                    {
                        going = false;
                        render.sprite = OffSprite;
                    }
                }

                if (Vector3.Distance(transform.position, worldDestination) < 0.001f)
                {
                    if (currentDestine >= DestinesIntern.Count - 1)
                    {
                        going = false;
                        render.sprite = OffSprite;
                        currentDestine = 1;
                        DestinesIntern.Reverse();
                        OnePass?.Invoke();
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
            if (!active) return;
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
            if (!active) return;
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
