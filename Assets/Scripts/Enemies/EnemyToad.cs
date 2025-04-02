using UnityEngine;
namespace br.com.bonus630.thefrog.Enemies
{
    // INHERITANCE
    public class EnemyToad : EnemyBase
    {
        // POLYMORPHISM
        protected override void Update()
        {

            base.Update();
            if (frontColliding)
            {
                xDirection *= -1;
                transform.localScale = new Vector3(-1 * transform.localScale.x, transform.localScale.y);
            }
            rg.linearVelocityX = Time.deltaTime * speed * xDirection;

        }
    }
}
