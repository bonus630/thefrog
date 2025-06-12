using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.Activators
{
    public class GhostShipInActivator : ActiveCamera
    {
        // [SerializeField] ActiveCamera activeCamera;
        // [SerializeField] MusicSource musicSource;
        [SerializeField] IActivator teleporter;
        bool actived = false;
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                base.OnTriggerEnter2D(collision);
                if (actived)
                    return;
                var cam = FindAnyObjectByType<CameraBackground>();
                cam.InitializeDayByHour(19);
                GameManager.Instance.StartTimer(10);
                //GameManager.Instance.StartTimer((11 * cam.CycleDurationMinutes * 60 / 12)-10);
               
                GameManager.Instance.TimeOverEvent += () => { Debug.Log("teleporter ghostship activator name:" + teleporter.gameObject.name); teleporter.Activate(); };
            }
        }


    }
}
