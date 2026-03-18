using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class SwitcherPositions : MonoBehaviour
    {
        [SerializeField] List<GameObject> objects;
        
        void Start()
        {
            for (int i = 0; i < objects.Count; i++)
            {
                int getPos = Random.Range(0, objects.Count);
                Vector3 tempPos = objects[getPos].transform.position;
                int otherPos = Random.Range(0, objects.Count);
                objects[getPos].transform.position = objects[otherPos].transform.position;
                objects[otherPos].transform.position = tempPos;
            }

        }
        
    }
}
