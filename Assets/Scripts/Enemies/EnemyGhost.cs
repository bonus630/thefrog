using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
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
            //Debug.Log("Ghost: " + gameObject + " " + GetComponent<BoxCollider2D>());

            GetComponent<BoxCollider2D>().enabled = true;
            GetComponent<CircleCollider2D>().enabled = true;
            GetComponent<SpriteRenderer>().enabled = true;

        }
        protected virtual void Deactive()
        {
            //Debug.Log("Ghost: " + gameObject + " " + GetComponent<BoxCollider2D>());
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<CircleCollider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

        }
        protected override void Update()
        {
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
