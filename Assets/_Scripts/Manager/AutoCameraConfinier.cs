using Cinemachine;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    [RequireComponent(typeof(PolygonCollider2D))]
    public class AutoCameraConfinier : MonoBehaviour
    {
        PolygonCollider2D polygonCollider;
        [SerializeField] GameObject nextCamera;
        void Start()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();
            if (polygonCollider.OverlapPoint(GameManager.Instance.GetPlayer.transform.position))
                SetConfinier();
        }
        private void SetConfinier()
        {
            GameObject camera = Utils.CameraUtils.GetActiveVirtualCamera2().gameObject;
            if (nextCamera != null)
            {
                camera.SetActive(false);
                nextCamera.SetActive(true);
                camera = nextCamera;
            }
            camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = polygonCollider;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                SetConfinier();
        }

    }
}
