using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class EnemyRino : EnemyToad
    {
        [SerializeField] AudioClip footstep;
        [SerializeField] AudioClip grunt;
        [SerializeField] GameObject horny;
        [SerializeField]ScreenEffects screenEffects;
        [SerializeField] LayerMask playerLayer;
        [SerializeField] float minSpeed = 40f;
        [SerializeField] float midSpeed = 100f;
        [SerializeField] float hiSpeed = 140f;
        [SerializeField] float maxSpeed = 180f;
        [SerializeField] float animationOffset = 1f;

        float time = 0.1f;
        float timer = 0;
        float limitSpeed = 0;
        AudioSource audioSource;

        private bool cancelUpdate = false;
     

        protected override void Awake()
        {
            base.Awake();
            horny.GetComponent<CollisionRelayEx>().OnTriggerEnterAction += EnemyRino_OnTriggerEnterAction;
            audioSource = GetComponent<AudioSource>();
            limitSpeed = maxSpeed + 20;
           
        }

        protected override void Update()
        {
    
            if(this.life <= 0)
            {
                Remove();
            }
            if (cancelUpdate) return;
            RaycastHit2D front = Physics2D.Raycast(downPoint.position, topPoint.position, 0.1f, layerMask);
            RaycastHit2D playerVision = Physics2D.Raycast(downPoint.position, Vector2.right * xDirection, 10f, playerLayer);


            timer += Time.deltaTime;
            if (timer > time)
            {
                if (playerVision)
                {
                    
                    if (speed < 10)
                        speed = minSpeed;
                    speed += 10;
                }
                else
                    speed -= 2;
                timer = 0;
            }
            if (speed < 0)
                speed = 0;
            if(speed > limitSpeed)
                speed = limitSpeed;
            if (front)
            {
                if (speed > hiSpeed)
                {
                    WallCollision();
                }
                else
                {
                    ChangeDirection();
                }

            }
            if (speed < 0.0001f)
                horny.SetActive(false);
            else
                horny.SetActive(true);
            animator.SetFloat(RunID, Mathf.Abs(speed));
            animationOffset = speed / 100;
            animator.SetFloat("Footsteps",animationOffset );
            rg.linearVelocityX = speed * xDirection * Time.deltaTime;
        }

        private void WallCollision()
        {
            cancelUpdate = true;
            //Debug.Log("wall collision");
            if(speed >  maxSpeed)
            {
                this.life -= 2;
            }
            if (speed > hiSpeed && speed < maxSpeed)
                this.life -= 1;
            animator.SetTrigger(HitID);
            audioSource.PlayOneShot(grunt);
            speed = 0;
        }
        public void Restart()
        {
            cancelUpdate = false;
        }
        public override void Hit(float hit)
        {
           
        }
        public void Idle()
        {
            screenEffects.GamepadShake(0, 0);
        }
        bool flip = true;
        public void Footstep()
        {
          
            audioSource.PlayOneShot(footstep);
            if(flip)
                screenEffects.GamepadShake(animationOffset/10, 0);
            else
                screenEffects.GamepadShake(0, animationOffset/10);
            //screenEffects.GamepadShake(0, 0);
            flip = !flip;
            screenEffects.ScreenShake(1);
        }
        private void Remove()
        {
            speed = 200f;
            Destroy(gameObject);
        }
        private void HornyHitPlayer(IPlayer player, int damage, float knockHitForce)
        {
            player.Hit(damage);
            KnockUpHitForce = new Vector2(knockHitForce,10);
        }
        protected override void OnCollisionEnter2D(Collision2D collision)
        {
            ContactPoint2D[] contacts = collision.contacts;
            for (int i = 0; i < contacts.Length; i++) {
                Debug.Log(contacts[i].normal);
                    }
            if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player) && speed < minSpeed)
            {
                //vamos refatorar isso depois, criar um enum para o player retornando a posiçao, em relaçao a outro transforme
                //direta, esquerda, cima, baixa, usar flags para poder combinar os valores
                //e criar um metodo de extensão para converter o valor do enum em Vector2
                if (player.Position.x < transform.position.x && xDirection > 0 || player.Position.x > transform.position.x && xDirection < 0)
                {
                    ChangeDirection();
                    return;
                }
              
            }
        }
        private void EnemyRino_OnTriggerEnterAction(ColliderData obj)
        {
            if (obj.ColliderOther.CompareTag("Player"))
            {
                if (obj.ColliderOther.TryGetComponent<IPlayer>(out IPlayer player))
                {
                    if (speed > maxSpeed)
                    {
                        HornyHitPlayer(player, 4, 160f);
                        return;
                    }
                    if (speed < maxSpeed && speed > hiSpeed)
                    {
                        HornyHitPlayer(player, 3, 120f);
                        return;
                    }
                    if (speed < hiSpeed && speed > midSpeed)
                    {
                        HornyHitPlayer(player, 2, 100f);
                        return;
                    }
                    if (speed < midSpeed)
                    {
                        HornyHitPlayer(player, 1, 80f);
                        return;
                    }
                   
                }
            }

        }
    }
   
}
