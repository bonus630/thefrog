using Cinemachine;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class SkullStartBattle : MonoBehaviour
    {
        bool start = false;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !start)
            {
                start = true;
                transform.parent.gameObject.transform.GetChild(1).gameObject.SetActive(true);
                transform.parent.gameObject.transform.GetChild(2).gameObject.SetActive(true);
                var confiner = GameObject.FindAnyObjectByType<CinemachineVirtualCamera>().GetComponent<CinemachineConfiner>();
                // confiner.m_BoundingShape2D = (PolygonCollider2D)GameObject.Find(GameManager.Instance.CameraContainer).transform.GetChild(5).gameObject.GetComponentAtIndex(1);
                gameObject.GetComponent<BoxCollider2D>().enabled = false;
                //Destroy(gameObject);
            }
        }
    }
}
