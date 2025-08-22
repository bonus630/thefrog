using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyPigPatrolFollowPlayer : EnemyPigPatrol
    {
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private bool detectPlayer;
        float followDistance = 6f;
        float turnDistance = 3f;
        
        protected override void Update()
        {
            base.Update();
            Debug.DrawRay(new Vector3(transform.position.x - (0.1f * xDirection), transform.position.y - 0.1f, 0), Vector3.left * turnDistance * xDirection, Color.green);
            RaycastHit2D hitLeft = Physics2D.Raycast(new Vector2(transform.position.x - (0.2f * xDirection), transform.position.y - 0.1f), Vector2.left * turnDistance * xDirection, turnDistance, playerLayer);
            
            
            
            Debug.DrawRay(new Vector3(transform.position.x + (1f * xDirection), transform.position.y - 0.1f, 0), Vector3.right * followDistance * xDirection, Color.blue);
            RaycastHit2D hitRight = Physics2D.Raycast(new Vector2(transform.position.x + (0.2f * xDirection), transform.position.y - 0.1f), Vector2.right * followDistance * xDirection, followDistance, playerLayer);

            if (hitRight.collider != null)
            {
                detectPlayer = true;
                speed = 80f;
            }
            if(hitLeft.collider!=null)
            {
                ChangeDirection();
            }

            if (runTime < 0)
            {
                runTime = maxRunTime;
                speed = 20f;
                detectPlayer = false;
            }
            if (detectPlayer)
            {
                runTime -= Time.deltaTime;
                
            }
        }
    }
}
