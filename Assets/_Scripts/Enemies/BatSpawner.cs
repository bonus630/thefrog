using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class BatSpawner : MonoBehaviour
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


    }
}
