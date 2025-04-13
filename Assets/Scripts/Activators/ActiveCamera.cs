using br.com.bonus630.thefrog.Manager;
using Cinemachine;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class ActiveCamera : MonoBehaviour
    {
        [SerializeField] int cameraIndex;
        [SerializeField] int confinierIndex;
        [SerializeField] CamerasController controller;

        public int CameraIndex { get { return cameraIndex; } }
        public int ConfinierIndex { get { return confinierIndex; } }

        private string ConfinerName(int index)
        {
            switch(index)
            {
                case 0:
                    return "CameraContainer";
                    case 1:
                    return "SkullCameraConfinier";
                default:
                    return string.Empty;
            }
        }

        
        private void Start()
        {
            if(controller==null)
                controller = FindAnyObjectByType<CamerasController>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Collider camera Activator");
                GameObject camera = controller.ActiveCam(cameraIndex);
                string confiner = ConfinerName(cameraIndex);
                if(!string.IsNullOrEmpty(confiner))
                    camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(confiner).transform.GetChild(confinierIndex).gameObject.GetComponentAtIndex(1);
            }
        }

    }
}
