using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class RandomizerInSceneObject : MonoBehaviour
    {
        [SerializeField] GameObject inSceneObject;
        [SerializeField] List<Transform> SpawnPoints;
        void Start()
        {
            int spawnIndex = Random.Range(0, SpawnPoints.Count);
            Transform point = SpawnPoints[spawnIndex];
            if (inSceneObject != null)
                inSceneObject.transform.position = point.position;
       
        }
    }
}