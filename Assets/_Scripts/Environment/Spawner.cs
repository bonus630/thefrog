using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Spawner : IActivator
    {
        [SerializeField] private List<GameObject> spawnerPoints;
        [SerializeField] private List<GameObject> spawnerTypes;
        [SerializeField] private bool randomPoints = true;
        [SerializeField] private bool randomTypes = true;
        [SerializeField] private float spawnerTime = 2;
        [SerializeField] private bool running = true;
        [SerializeField][Tooltip("Use 0 to infinity")] private int limit = 2;

        public float SpawnerTime { get { return spawnerTime; } set { spawnerTime = value; } }
        public bool Running { get { return running; } set { running = value; } }
        //public bool startBattle { get; set; }
        private float timer = 0;
        private int currentPoint = 0;
        private int currentType = 0;

        private List<GameObject> instances = new List<GameObject>();
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (running)
            {
                timer += Time.deltaTime;
                if (timer > spawnerTime)
                {
                    for (int i = 0; i < instances.Count; i++)
                    {
                        if (instances[i] == null)
                        { instances.RemoveAt(i); }
                    }
                    if (instances.Count < limit || limit == 0)
                    {
                        instances.Add(Instantiate(spawnerTypes[currentType], spawnerPoints[currentPoint].transform.position, spawnerTypes[currentType].transform.rotation));
                        timer = 0;
                        CurrentPoint();
                        CurrentType();
                    }
                }
            }
        }
        private void CurrentPoint()
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
        private void CurrentType()
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



        public override void Activate()
        {
            running = true;
        }

        public override void Deactive()
        {
            running = false;
        }
    }
}
