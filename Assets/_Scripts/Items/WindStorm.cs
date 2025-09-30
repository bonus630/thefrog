using System.Linq;
using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class WindStorm : IProjectilies, IElement
    {
        [field: SerializeField] public override Elements GetElement { get; set; } = Elements.Wind;
        [field: SerializeField] public Color ElementColor { get; set; } = Color.green;
        [SerializeField] LayerMask canHitLayers;
        [SerializeField] AudioSource audioSource;
        [SerializeField] bool isPermanent = false;
        //[SerializeField] GameObject impactZone;
        [SerializeField] float intensity = 0f;
        //   bool hit = false;
        //  ParticleSystem ps;
        float time = 3.75f;
        Animator anim;
        SpriteRenderer spriteRenderer;
        private void Awake()
        {
            //   ps = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
            anim = transform.GetChild(0).gameObject.GetComponent<Animator>();
            spriteRenderer = anim.gameObject.GetComponent<SpriteRenderer>();
            //impactZone.GetComponent<CollisionRelayEx>().OnTriggerEnterAction += LightningBolt_OnTriggerEnterAction;
        }

        private void Update()
        {
            if (isPermanent)
                return;
            time-= Time.deltaTime;
            if (time <= 0)
            {
               anim.SetTrigger("Goning");
                time = 4f;
            }
        }

        public Elements CanActiveBy() => Elements.Wind;
        public Elements CanDeactiveBy() => Elements.Earth;
       
        public override float ReloadTime() => 1.4f;

        public void ActiveBy(Elements element)
        {
            ActiveDeactive(true);
        }

        public void DeactiveBy(Elements element)
        {
            ActiveDeactive(false);
        }
        private void ActiveDeactive(bool active)
        {

        }
        public override void Launch(Vector2 direction)
        {
            Vector3 playerPos = GameManager.Instance.GetPlayer.transform.position;
            float posX = playerPos.x + (1.8f * direction.x);
            float posY = playerPos.y;
            transform.position = new Vector2(posX, posY);
            audioSource.Play();
            Remove();
        }
        public void Remove()
        {
            Debug.Log("Lightining remover:");
            // impactZone.GetComponent<CollisionRelayEx>().OnTriggerEnterAction -= LightningBolt_OnTriggerEnterAction;
            Destroy(gameObject,4f);
        }
 
        public Vector3 GetProjectileSpawnPoint(Vector3 origin, float raycastDistance, LayerMask targetLayer, float fixedY)
        {
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, raycastDistance, targetLayer);
            Debug.DrawRay(origin, Vector2.right, Color.yellow, 2f);
            if (hit.collider != null)
            {
                // Usa X do hit, Y fixo
                return new Vector3(hit.point.x, fixedY, 0f);
            }

            // Se nada for detectado, retorna um ponto padrão
            return new Vector3(origin.x + raycastDistance, fixedY, 0f);
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("WindStorm collision: " + collision.gameObject.name);
            if(collision.TryGetComponent<IElement>(out IElement el))
            {
                Color color = el.ElementColor;
                color = new Color(color.r,color.g,color.b,0.45f);
                EffectManager.instance.AddEffect(new ColorEffect(spriteRenderer,spriteRenderer.color, color, 4f));
               // StartCoroutine(StaticsRoutines.LerpColor(spriteRenderer, spriteRenderer.color, color, 4f));
                if(el.CanActiveBy()== GetElement)
                    el.ActiveBy(GetElement);
                if(el.CanDeactiveBy()== GetElement)
                    el.DeactiveBy(GetElement);
            }
        }
        public override void ChangeDirectionY()
        {
            transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
            GetComponent<AreaEffector2D>().forceAngle = 270;
        }
    }
}

