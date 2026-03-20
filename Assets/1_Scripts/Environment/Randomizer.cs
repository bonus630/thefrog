using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class Randomizer : MonoBehaviour
    {
        [SerializeField] GameObject RealGameObject;
        [SerializeField] GameObject[] FakerGameobjects;
        [SerializeField] List<Transform> SpawnPoints;

       

        void Start()
        {
            Debug.Log("[Randomizer] create:" + gameObject.name);
            int spawnIndex = Random.Range(0, SpawnPoints.Count);
            Transform point = SpawnPoints[spawnIndex];
            if(RealGameObject!=null)
                Instantiate(RealGameObject, point);
            SpawnPoints.RemoveAt(spawnIndex);

            while(SpawnPoints.Count > 0)
            {
                int fakerIndex = Random.Range(0, FakerGameobjects.Length);
                spawnIndex = Random.Range(0, SpawnPoints.Count);
                 point = SpawnPoints[spawnIndex];
                Instantiate(FakerGameobjects[fakerIndex], point);
                SpawnPoints.RemoveAt(spawnIndex);
            }
        }
    }
}
