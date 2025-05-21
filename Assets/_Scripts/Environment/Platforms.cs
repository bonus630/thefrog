using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class Platforms : MonoBehaviour
    {
        [SerializeField] private float fallingTime;
        [SerializeField] private bool respawnable;

        private Animator anim;
        private Joint2D join;
        private BoxCollider2D coll;
        private bool isFalling = false;
        private bool startCountdown = false;
        private float time;
        private float restoreTime = 10f;
        private Vector2 initialPosition;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            anim = GetComponent<Animator>();
            join = GetComponent<Joint2D>();
            coll = GetComponent<BoxCollider2D>();
            time = fallingTime;
            if (respawnable)
            {
                initialPosition = transform.position;

            }
        }
        private void Update()
        {
            if (startCountdown)
            {
                time -= Time.deltaTime;
                if (time < 0)
                    DisablePlatform();
            }
            if (isFalling && respawnable)
            {
                restoreTime -= Time.deltaTime;
                if (restoreTime < 0)
                    EnablePlatform();
            }
        }
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && collision.gameObject.GetComponent<IPlayer>().FooterTouching(coll))
            {
                startCountdown = true;
            }
            if (!collision.gameObject.CompareTag("Player") && isFalling)
                coll.isTrigger = true;

        }
        private void OnCollisionExit2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player") && !isFalling)
            {
                startCountdown = false;
                time = fallingTime;
            }
        }
        private void DisablePlatform()
        {

            anim.SetBool("on", false);
            join.enabled = false;
            // coll.isTrigger = true;
            isFalling = true;
        }
        private void EnablePlatform()
        {
            join.enabled = true;
            GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
            transform.position = initialPosition;
            anim.SetBool("on", true);
            // coll.isTrigger = true;
            isFalling = false;
            restoreTime = 10f;
            time = fallingTime;
            startCountdown = false;
        }
    }
}
