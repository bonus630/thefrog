using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    public class BlackHoleEffect :  IEffects
    {
        private Rigidbody2D target;
        private Vector2 center ;
        private float gravityForce ;
        private float maxSpeed ;
        //public ushort ID { get; set; } // estão na classe abstrata ieffects
        //public bool IsFinished { get; private set; }

        private BlackHoleEffect(Rigidbody2D target, Vector2 center, float gravityForce, float maxSpeed)
        {
            this.target = target;
            this.center = center;
            this.gravityForce = gravityForce;
            this.maxSpeed = maxSpeed;
            IsFinished = false;
        }

        public static BlackHoleEffect Create(Rigidbody2D target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            return new BlackHoleEffect(target, Vector2.zero, 1, 1);
        }
        public BlackHoleEffect FromCenter(Vector2 center)
        {
            this.center = center;
            return this;
        }
        public BlackHoleEffect WithGravityForce(float gravityForce)
        {
            this.gravityForce = gravityForce;
            return this;
        }
        public BlackHoleEffect WithMaxSpeed(float maxSpeed)
        {
            this.maxSpeed = maxSpeed;
            return this;
        }
        public BlackHoleEffect Build()
        {
            return this;
        }
        public override void UpdateEffects(float dt)
        {
            if (target == null)
            {
                IsFinished = true;
                return;
            }

            // Fix: Convert both positions to Vector2 before subtracting
            Vector2 direction = (Vector2)center - target.position;
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

        public override void Activate()
        {
            
        }

        public override void Deactivate()
        {
            IsFinished = true;
        }
    }
}