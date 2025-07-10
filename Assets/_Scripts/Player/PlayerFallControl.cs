using UnityEngine;

namespace br.com.bonus630.thefrog.Player
{
    public class PlayerFallControl : PlayerBase
    {
        public bool InFallControl { get; private set; } = false;
        [SerializeField] GameObject wings;
        float time = 2f;
        float timer = 0f;


        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
           // FallsControl(false);
            
        }

        private void FixedUpdate()
        {

            if (!InFallControl)
                return;
            if (timer < time)
            {
                timer += Time.deltaTime;
                player.playerMovement.TimeInFastFall = 0f;
                player.RigibodyGravityScale = player.RigibodyGravityScale * 0.5f;
                Vector2 velocity = player.RigibodyLinearVelocity;
                velocity.y *= 0.5f;
                player.RigibodyLinearVelocity = velocity;
                Debug.Log("rb.gravityScale: " + player.RigibodyGravityScale);
                Debug.Log("rb.velocity: " + player.RigibodyLinearVelocity);
            }
            else
            {
                timer = 0f;
                FallsControl(false);
            }

        }
        public void FallsControl(bool inFallControl)
        {
            InFallControl = inFallControl;
            wings.SetActive(InFallControl);
            Debug.Log("Playerfallcontrol :" +inFallControl);
            Debug.Log("Timer :" +timer);
            //// rb.gravityScale = rb.gravityScale + (0.1f * (float)player.gravityDirection);
            ////// Physics2D.Raycast(transform.position, Vector2.up * gravityDirection);
            //rb.linearVelocityY = LinearMaxY * player.gravityDirection;
            //StartCoroutine(ReducePlayerGravity());
            ////player.RemoveGravity(t);
            //FallsControlEffect();

        }
        //private IEnumerator ReducePlayerGravity()
        //{
        //    Debug.Log("Player rigidibody: " + rb.gameObject.name);
        //    Debug.Log("Player gravity direction: " + player.gravityDirection);
        //    // yield return new WaitForSeconds(1f);

        //    while (!Mathf.Approximately(rb.gravityScale, 0f))
        //    {
        //        Debug.Log("Player Velocity Y: " + rb.linearVelocityY);
        //        Debug.Log("Player Gravity scale: " + rb.gravityScale);
        //        rb.gravityScale = rb.gravityScale * 0.5f;
        //        Vector2 velocity = rb.linearVelocity;
        //        velocity.y *= 0.5f;
        //        rb.linearVelocity = velocity;
        //        yield return new WaitForSeconds(0.1f);
        //    }
        //    rb.gravityScale = 0f;
        //    rb.linearVelocity = Vector2.zero;
        //    yield return new WaitForSeconds(0.5f);
        //    player.RemoveGravity(false);

        //}
        public void FallsControlEffect()
        {
            Debug.Log("FallsControleEffect");
        }
    }
}
