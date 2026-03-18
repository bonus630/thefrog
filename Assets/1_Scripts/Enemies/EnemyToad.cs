using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyToad : EnemyBase
    {
        protected override void Update()
        {

            base.Update();
            if (frontColliding)
            {
                ChangeDirection();
            }
            rg.linearVelocityX = Time.deltaTime * speed * xDirection;

        }
        protected virtual void ChangeDirection()
        {
            xDirection *= -1;
            transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
        }
    }
}
