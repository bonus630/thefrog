using br.com.bonus630.thefrog.Manager;
using Cinemachine;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class ActiveCamera : MonoBehaviour
    {
        [SerializeField] int cameraIndex;
        [SerializeField] int confinierIndex;

        public int CameraIndex { get { return cameraIndex; } }
        public int ConfinierIndex { get { return confinierIndex; } }


        CamerasController controller;
        private void Awake()
        {
            controller = FindAnyObjectByType<CamerasController>();
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                GameObject camera = controller.ActiveCam(cameraIndex);
                camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = (PolygonCollider2D)GameObject.Find("SkullCameraConfinier").transform.GetChild(confinierIndex).gameObject.GetComponentAtIndex(1);
            }
        }

    }
}
