using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class BatSpawner : IActivator
    {
        [SerializeField] private List<GameObject> spawnerPoints;
        [SerializeField] private GameObject bat;


        // ENCAPSULATION
        [HideInInspector] public float spawnTime { get; set; } = 2;
        [HideInInspector] public bool startBattle { get; set; }
        private float timer = 0;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (startBattle)
            {
                timer += Time.deltaTime;
                if (timer > spawnTime)
                {
                    Instantiate(bat, spawnerPoints[Random.Range(0, spawnerPoints.Count)].transform.position, bat.transform.rotation);
                    timer = 0;
                }
            }
        }

        public override void Activate()
        {
            startBattle = true;
        }

        public override void Deactive()
        {
            startBattle=false;
        }
    }
}
