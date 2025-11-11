using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class BlackHoleEffect :  IEffects
    {
        private Rigidbody2D target;
        private Transform center;
        private float gravityForce;
        private float maxSpeed;
        public ushort ID { get; set; }
        public bool IsFinished { get; private set; }

        public BlackHoleEffect(Rigidbody2D target, Transform center, float gravityForce, float maxSpeed)
        {
            this.target = target;
            this.center = center;
            this.gravityForce = gravityForce;
            this.maxSpeed = maxSpeed;
            IsFinished = false;
        }

        public void UpdateEffects(float dt)
        {
            if (target == null)
            {
                IsFinished = true;
                return;
            }

            // Fix: Convert both positions to Vector2 before subtracting
            Vector2 direction = (Vector2)center.position - target.position;
            float distance = direction.magnitude;
            Vector2 force = direction.normalized * gravityForce / Mathf.Max(distance, 0.1f);

            target.linearVelocity += force * dt;

            if (target.linearVelocity.magnitude > maxSpeed)
                target.linearVelocity = target.linearVelocity.normalized * maxSpeed;
           // Debug.Log($"BlackHoleEffect - Target Velocity: {distance}");   
            // Opcional: finalizar efeito quando estiver perto do centro
            if (distance < 0.3f)
                IsFinished = true;
        }

        public void Activate()
        {
            
        }

        public void Deactivate()
        {
            IsFinished = true;
        }
    }
}