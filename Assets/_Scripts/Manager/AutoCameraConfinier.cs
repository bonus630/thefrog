using System.Collections;
using br.com.bonus630.thefrog.Shared;
using Cinemachine;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(1), RequireComponent(typeof(PolygonCollider2D))]
    public class AutoCameraConfinier : MonoBehaviour
    {
        PolygonCollider2D polygonCollider;
        [SerializeField] GameObject nextCamera;
        [SerializeField] GameObject confiner;
        void Start()
        {
            polygonCollider = GetComponent<PolygonCollider2D>();

            //Debug.Log("[AutoCameraConfinier] polygonCollider:" + polygonCollider);
            //Debug.Log("[AutoCameraConfinier] player:" + GameManager.Instance.GetPlayer.transform.position);


            if (polygonCollider.OverlapPoint(GameManager.Instance.GetPlayer.transform.position))
                StartCoroutine(SetConfinier());
        }
        private IEnumerator SetConfinier()
        {
            CinemachineVirtualCamera cam = null;
            while (cam == null)
            {
                yield return null;
                cam = Utils.CameraUtils.GetActiveVirtualCamera2();
            }
            GameObject camera = cam.gameObject;
            if (camera != null)
            {
                if (nextCamera != null)
                {
                    camera.SetActive(false);
                    nextCamera.SetActive(true);
                    camera = nextCamera;
                }
                camera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = polygonCollider;
                camera.GetComponent<CinemachineConfiner>().InvalidatePathCache();
                confiner = gameObject;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
                StartCoroutine(SetConfinier());
        }
        //private void OnTriggerExit2D(Collider2D collision)
        //{
        //    if(collision.TryGetComponent<IPlayer>(out IPlayer player))
        //    {
        //        if(!player.BodyTouching(1<<16))
        //        {
        //            ServiceLocator.Instance.Get<CamerasController>().GetSkyCam().gameObject.SetActive(true);
        //            Utils.CameraUtils.GetActiveVirtualCamera2().gameObject.SetActive(false);
        //        }
        //    }
        //}

    }
}
