using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Spawner : IActivator
    {
        [SerializeField] protected List<GameObject> spawnerPoints;
        [SerializeField] protected List<GameObject> spawnerTypes;
        [SerializeField] protected bool randomPoints = true;
        [SerializeField] protected bool randomTypes = true;
        [SerializeField] protected float spawnerTime = 2;
        [SerializeField] protected bool running = true;
        [SerializeField] protected bool destroySpawnedInDisable = false;
        [SerializeField][Tooltip("Use 0 to infinity")] protected int limit = 2;

        public float SpawnerTime { get { return spawnerTime; } set { spawnerTime = value; } }
        public bool Running { get { return running; } set { running = value; } }
        //public bool startBattle { get; set; }
        protected float timer = 0;
        protected int currentPoint = 0;
        protected int currentType = 0;
        protected List<GameObject> instances = new List<GameObject>();
        protected virtual void Start()
        {
            
        }
        void Update()
        {
            if (running)
            {
                timer += Time.deltaTime;
                if (timer > spawnerTime)
                {
                    for (int i = instances.Count - 1; i >= 0; i--)
                    {
                        if (instances[i] == null)
                        { instances.RemoveAt(i); }
                    }
                    if (instances.Count < limit || limit == 0)
                    {
                        instances.Add(Instantiate(spawnerTypes[currentType], GetPoint(), spawnerTypes[currentType].transform.rotation));
                        timer = 0;
                        CurrentPoint();
                        CurrentType();
                    }
                }
            }
        }
        protected virtual void CurrentPoint()
        {
            if (randomPoints)
            {
                currentPoint = Random.Range(0, spawnerPoints.Count);
            }
            else
            {
                if (currentPoint < spawnerPoints.Count - 1)
                    currentPoint++;
                else
                    currentPoint = 0;
            }
        }
        protected virtual void CurrentType()
        {
            if (randomTypes)
            {
                currentType = Random.Range(0, spawnerTypes.Count);
            }
            else
            {
                if (currentType < spawnerTypes.Count - 1)
                    currentType++;
                else
                    currentType = 0;
            }
        }
        protected virtual Vector3 GetPoint()
        {
            return spawnerPoints[currentPoint].transform.position;
        }
        public override void Activate()
        {
            running = true;
        }
        public override void Deactive()
        {
            running = false;
        }
        private void OnDisable()
        {
            if (destroySpawnedInDisable)
            {
                running = false;
                for (int i = instances.Count - 1; i >= 0; i--)
                {
                    Destroy(instances[i]);
                }
            }
        }
    }
}
