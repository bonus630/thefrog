using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class ActiveByHourDistance : MonoBehaviour
    {
        [SerializeField] GameObject player;
        [SerializeField] GameObject pointToActive;
        [SerializeField][Tooltip("GameObject ou IActivator")] GameObject itemToActive;
        [SerializeField] int startHour;
        [SerializeField] int endHour;
        [SerializeField] float distance;
        [SerializeField] bool ignoreDistanceAfterActive;
        public float playerDistance;
        private bool firstActive = false;
        private int hour;
        private CameraBackground cameraControl;
        private void LateUpdate()
        {
            playerDistance = Vector3.Distance(player.transform.position, pointToActive.transform.position);
            ActiveItem();
        }

        void Awake()
        {
            cameraControl = FindAnyObjectByType<CameraBackground>();
            this.hour = cameraControl.Hour;
            cameraControl.HourChanged += Item_HourChanged;
        }
        private void Start()
        {

        }
        private void Item_HourChanged(int hour)
        {
            this.hour = hour;
            ActiveItem();
        }
        private void ActiveItem()
        {
            if (InInterval(hour) && ActiveDistance())
            {
               // Debug.Log("Active : " + itemToActive.name + " hour: " + hour);
                ActiveItem(true);
                firstActive = true;
            }
            else
            {
               // Debug.Log("Deactive: " + itemToActive.name + " hour: " + hour);
                ActiveItem(false);
            }
        }

        private void ActiveItem(bool active)
        {
            IActivator activator;
            if (itemToActive.gameObject.TryGetComponent<IActivator>(out activator))
                activator.Activate();
            else
                itemToActive.SetActive(active);
        }
        bool InInterval(int hour)
        {
            if (startHour <= endHour)
            {
                return hour >= startHour && hour <= endHour;
            }
            else
            {
                return hour >= startHour || hour <= endHour;
            }

        }
        bool ActiveDistance()
        {
            if (ignoreDistanceAfterActive && firstActive)
                return firstActive;
            return Vector3.Distance(player.transform.position, pointToActive.transform.position) < distance;

        }
        private void OnDestroy()
        {
            cameraControl.HourChanged -= Item_HourChanged;
        }
    }
}
