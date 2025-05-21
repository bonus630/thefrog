using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyGhost : EnemyBase
    {
        [SerializeField] protected float rotationSpeed = 10f;
        [SerializeField] protected float maxFollowTime = 5f;
        protected float followTime = 0;
        protected Vector2 moveFor;
        protected GameObject player;

        private CameraBackground cameraControl;
        public GameObject Player { get { return player; } set { player = value; } }
        protected bool active = true;

        protected override void Awake()
        {
            player = GameManager.Instance.GetPlayer;
            cameraControl = FindAnyObjectByType<CameraBackground>();
            cameraControl.HourChanged += Item_HourChanged;
        }
        private void Item_HourChanged(int hour)
        {
            if (hour >= 5 && hour < 20)
                Deactive();
            else
                Active();
        }
        protected virtual void Active()
        {
            ToggleState(true);
        }
        protected virtual void Deactive()
        {
            ToggleState(false);
        }
        private void ToggleState(bool _active)
        {
            if (active == _active)
                return;
            active = _active;
            if (TryGetComponent<BoxCollider2D>(out var bc))
                bc.enabled = active; 
            if (TryGetComponent<CircleCollider2D>(out var cc))
                cc.enabled = active; 
            if (TryGetComponent<SpriteRenderer>(out var sr))
                sr.enabled = active;
        }
        protected override void Update()
        {
            if (!active)
                return;
            FollowPlayer();
            followTime = maxFollowTime;
        }

        protected virtual void FollowPlayer()
        {
            followTime -= Time.deltaTime;
            moveFor = (player.transform.position - transform.position).normalized;
            //Debug.Log(moveFor);
            float playerAngle = Mathf.Atan2(moveFor.y, moveFor.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, playerAngle), rotationSpeed * Time.deltaTime);
            if (followTime > 0)
            {
                transform.position += transform.right * 2 * Time.deltaTime;
            }

        }
        private void OnDestroy()
        {
            cameraControl.HourChanged -= Item_HourChanged;
        }
    }
}
