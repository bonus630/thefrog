using br.com.bonus630.thefrog.Manager;
using Cinemachine;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class ActiveCamera : MonoBehaviour
    {
        [SerializeField]protected int cameraIndex;
        [SerializeField] protected int confinierIndex;
        [SerializeField] protected CamerasController controller;
        

        public int CameraIndex { get { return cameraIndex; } }
        public int ConfinierIndex { get { return confinierIndex; } }

        protected string ConfinerName(int index)
        {
            switch(index)
            {
                case 0:
                    return "CameraContainer";
                    case 1:
                    return "SkullCameraConfinier"; 
                case 3:
                    return "KoarCameraConfinier";
                default:
                    return string.Empty;
            }
        }


        protected virtual void Start()
        {
            if(controller==null)
                controller = FindAnyObjectByType<CamerasController>();
        }
        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameObject camera = controller.ActiveCam(cameraIndex);
                string confiner = ConfinerName(cameraIndex);
               // Debug.Log("Collider camera Activator:" + GameObject.Find(confiner).transform.childCount);
                if(!string.IsNullOrEmpty(confiner))
                    camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(confiner).transform.GetChild(confinierIndex).gameObject.GetComponentAtIndex(1);
            }
        }

    }
}
