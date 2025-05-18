using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.Activators
{
    public class GhostShipInActivator : ActiveCamera
    {
        // [SerializeField] ActiveCamera activeCamera;
        // [SerializeField] MusicSource musicSource;
        
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                var cam = FindAnyObjectByType<CameraBackground>();
                cam.InitializeDayByHour(19);
                GameManager.Instance.StartTimer((11 * cam.CycleDurationMinutes * 60 / 12)-10);
                base.OnTriggerEnter2D(collision);
            }
        }


    }
}
