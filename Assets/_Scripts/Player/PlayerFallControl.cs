using UnityEngine;

namespace br.com.bonus630.thefrog.Player
{
    public class PlayerFallControl : PlayerBase
    {
        [SerializeField] GameObject wings;
        public bool InFallControl { get; private set; } = false;
        public float Time { get; set; } = 2f;
        float timer = 0f;


        void Start()
        {
           // FallsControl(false);
            
        }

        private void FixedUpdate()
        {

            if (!InFallControl)
                return;
            if (timer < Time)
            {
                //timer += UnityEngine.Time.deltaTime;
                //player.playerMovement.TimeInFastFall = 0f;
                //player.RigibodyGravityScale = player.RigibodyGravityScale * 0.5f;
                //Vector2 velocity = player.RigibodyLinearVelocity;
                //velocity.y *= 0.5f;
                //player.RigibodyLinearVelocity = velocity;
                ////Debug.Log("rb.gravityScale: " + player.RigibodyGravityScale);
                ////Debug.Log("rb.velocity: " + player.RigibodyLinearVelocity);
                ///  timer += UnityEngine.Time.deltaTime;
                player.playerMovement.TimeInFastFall = 0f;

                float duration = Time;
                float t = Mathf.Clamp01(timer / duration);
                float minFactor = 0.2f; // não deixa passar de 30% da gravidade original
                player.RigibodyGravityScale = Mathf.Lerp(player.gravityScale, player.gravityScale * minFactor, t);

                Vector2 velocity = player.RigibodyLinearVelocity;
                velocity.y *= 0.5f;
                player.RigibodyLinearVelocity = velocity;
                timer += UnityEngine.Time.deltaTime;
            }
            else
            {
                timer = 0f;
                FallsControl(false);
                player.RemoveGravity(false);
            }

        }
        public void FallsControl(bool inFallControl)
        {
            InFallControl = inFallControl;
            wings.SetActive(InFallControl);
            Debug.Log("Playerfallcontrol :" +inFallControl);
            Debug.Log("Timer :" +timer);
        }

        public void FallsControlEffect()
        {
            Debug.Log("FallsControleEffect");
        }
    }
}
