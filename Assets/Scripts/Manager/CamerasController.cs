using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class CamerasController : MonoBehaviour
    {
        [SerializeField] List<GameObject> Cameras;


        public int LastActiveCam { get; private set; }
        public int LastActiveConfiner { get; private set; }

        public GameObject ActiveCam(int index)
        {
            for (int i = 0; i < Cameras.Count; i++)
            {

                Cameras[i].SetActive(false);
            }
            Cameras[index].SetActive(true);
            LastActiveCam = index;
            return Cameras[index];
        }
    }
}

