using System.Collections;
using br.com.bonus630.thefrog.Items;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyPlant : EnemyBase
    {
        [SerializeField] GameObject shootSpawner;
        [SerializeField] GameObject shoot;
        //[SerializeField] LayerMask layerMask;
        [SerializeField] float shootTime;
        [SerializeField] float shootForce;

        [SerializeField][Range(-1, 1)] int direction = -1;


        int AttackID = Animator.StringToHash("Attack");

        bool shooting = false;

        Animator anim;

        protected override void Start()
        {
            anim = GetComponent<Animator>();
            if (direction > 0)
            {
                if (transform.localScale.x < 0)
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            }
            if (direction < 0)
            {
                if (transform.localScale.x > 0)
                    transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);

            }
        }

        // Update is called once per frame
        protected override void Update()
        {
            Debug.DrawRay(
                new Vector3(shootSpawner.transform.position.x, shootSpawner.transform.position.y - 0.1f, 0)
                , Vector3.right * 10 * direction, Color.blue);
            RaycastHit2D hit = Physics2D.Raycast(new Vector3(shootSpawner.transform.position.x, shootSpawner.transform.position.y - 0.1f, 0), Vector3.right * 10 * direction, 10, layerMask);
            if (hit.collider != null && !shooting)
                anim.SetBool(AttackID, true);

        }
        public void Shoot()
        {
            if (!shooting)
            {
                shooting = true;
                anim.SetBool(AttackID, false);
                StartCoroutine(Shooting());
                //Debug.LogWarning("DEs");
            }
        }
        IEnumerator Shooting()
        {
            GameObject go = Instantiate(shoot, shootSpawner.transform.position, shoot.transform.rotation);
            go.GetComponent<PlantBullet>().Direction = direction;
            // go.GetComponent<Rigidbody2D>().AddForce(Vector2.right * shootForce * direction, ForceMode2D.Impulse);
            yield return new WaitForSeconds(shootTime);
            shooting = false;
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {

                if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && player.FooterTouching(coll))
                {
                    player.KnockUp(repulse);

                    return;
                }

            }
            if (collision.gameObject.layer == 12)
            {
                if (collision.gameObject.TryGetComponent<IProjectilies>(out IProjectilies p))
                {
                    if (enemyWeakenesses.Contains(p.GetElement()))
                    {
                        Hit(2);
                    }
                }
            }
        }
        public override void Hit(float hit)
        {

           // Debug.Log("Collider Hit " + gameObject.name);

            anim.SetTrigger("Hit");
            this.life = this.life - hit;
            if (life < 0.1f)
                Destroy(gameObject, 0.2f);

        }
    }
}
