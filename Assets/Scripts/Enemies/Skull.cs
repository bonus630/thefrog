using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Items;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Enemies
{

    public class Skull : EnemyGhost
    {


        [SerializeField] GameObject shield;
        [SerializeField] GameObject spawnerPoint;
        [SerializeField] GameObject[] spawnerObjects;
        [SerializeField] float followMinTime = 1;
        [SerializeField] float followMaxTime = 3;
        [SerializeField] float shieldDeativedTime = 10f;
        // [SerializeField] int spawnerLimiter = 1;
        // List<GameObject> spawnList = new List<GameObject>();    
        // CircleCollider2D circleColl;
        //float followTime = 0;
        //Vector2 moveFor;
        float shieldTimer = 0;
        bool spawning = false;
        bool isDied = false;

        protected override void Start()
        {
            animator = GetComponent<Animator>();
            coll = GetComponent<BoxCollider2D>();
            followTime = 5f;
            this.life = 5;
            //base.Start();
            //circleColl = GetComponent<CircleCollider2D>();
        }

        protected override void Update()
        {
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    isDied = true;
            //    animator.SetTrigger("Die");
            //}
            if (isDied)
                return;
            FollowPlayer();
            if (followTime < 0)
            {
                if (!spawning)
                    StartCoroutine(CastFireball());
            }
            shieldTimer -= Time.deltaTime;
            if (shieldTimer < 0)
                EnableShield();
        }

        IEnumerator CastFireball()
        {
            spawning = true;
            GameObject bullet = Instantiate(spawnerObjects[Random.Range(0, spawnerObjects.Length)], spawnerPoint.transform);
            yield return new WaitForSeconds(0.4f);
            if (bullet != null)
                bullet.GetComponent<Fireball>().Launch(moveFor);
            yield return new WaitForSeconds(5);
            followTime = Random.Range(followMinTime, followMaxTime);
            spawning = false;
        }
        public void DisableShield()
        {
            shield.SetActive(false);
            animator.SetBool("Shield", false);
            shieldTimer = shieldDeativedTime;
            //Debug.Log("Disable shield");
        }
        public void EnableShield()
        {
            animator.SetBool("Shield", true);
            coll.enabled = true;
            shield.SetActive(true);
            // Debug.Log("Active shield");
        }

        protected new void OnCollisionEnter2D(Collision2D collision)
        {

            if (collision.gameObject.CompareTag("Player"))
            {
                Player player;

                if (collision.gameObject.TryGetComponent<Player>(out player) && player.FooterTouching(coll))
                {
                    //   Invoke(nameof(Restore), 0.5f);
                    //Vector3 re = player.transform.position - transform.position;
                    // repulse = re.normalized * repulseForce * -1;
                    repulse = collision.GetContact(0).normal * repulseForce * -1;
                    coll.enabled = false;
                    player.KnockUp(repulse);
                    this.life--;
                    //  Debug.Log("Boss Collider " + gameObject.name + " Life: " + this.life);
                    if (life < 1)
                    {
                        isDied = true;
                        animator.SetTrigger("Die");
                        return;
                    }
                    animator.SetTrigger(HitID);
                    // Invoke(nameof(EnableShield), 0.2f);
                }
            }
        }
        protected void Die()
        {
            transform.parent.gameObject.transform.GetChild(0).gameObject.SetActive(true);
            Destroy(gameObject);
            Debug.Log("Boss die");
        }

    }
}
