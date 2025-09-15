using System.Linq;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class LightningBolt : IProjectilies, IElement
    {
        [field: SerializeField] public override Elements GetElement { get; set; } = Elements.Lightining;
        [field: SerializeField] public Color ElementColor { get; set; } = Color.white;
        [SerializeField] LayerMask canHitLayers;
        [SerializeField] AudioSource audioSource;
        [SerializeField] GameObject impactZone;
        [SerializeField] float intensity = 1f;
        bool hit = false;
        //  ParticleSystem ps;

        private void Awake()
        {
            //   ps = GetComponent<ParticleSystem>();
            audioSource = GetComponent<AudioSource>();
            //impactZone.GetComponent<CollisionRelayEx>().OnTriggerEnterAction += LightningBolt_OnTriggerEnterAction;
        }

        //private void LightningBolt_OnTriggerEnterAction(ColliderData obj)
        //{
        //    Debug.Log("impactzone event:"+obj.GameObjectOwner.name);
        //    Finish(obj.ColliderOther.gameObject);
        //}

        public Elements CanActiveBy() => Elements.Lightining;
        public Elements CanDeactiveBy() => Elements.Water;
      
        public override float ReloadTime() => 1f;

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
        

        private void Finish(GameObject other)
        {
            Debug.Log(other.name);
            //if(other.TryGetComponent<SpriteRenderer>(out var render))
            //{
            //    render.color = Color.gray;
            //}
            if (hit)
                return;
            hit = true;
            IEnemy enemy;
            if (other.TryGetComponent<IEnemy>(out enemy))
            {
                enemy.Hit(intensity);
                return;
            }
            if (other.TryGetComponent<IElement>(out IElement element))
            {
                if (element.CanActiveBy().Equals(GetElement))
                {
                    element.ActiveBy(GetElement);
                }
                if (element.CanDeactiveBy().Equals(GetElement))
                {
                    element.DeactiveBy(GetElement);
                }

            }
            //Remove();
        }
        public override void Launch(Vector2 direction)
        {

            // ps.Simulate(1f, true, true);
            //float y = Camera.main.ViewportToWorldPoint(Vector3.up).y;
            //float x = Camera.main.ViewportToWorldPoint(Vector3.right * direction).x;
            CameraBounds2D bounds = CameraUtils.GetBounds2D();
            Vector2 frontTop = direction.x > 0 ? bounds.topRight : bounds.topLeft;
            Vector2 behindBottom = direction.x > 0 ? bounds.bottomLeft : bounds.bottomRight;
            Vector3 playerPos = GameManager.Instance.GetPlayer.transform.position;

            // Debug.DrawLine(bottomL, GameManager.Instance.GetPlayer.transform.position, Color.red,10f );

            // Vector2 PlayerBottom = new Vector2(playerPos.x, bottomL.y);
            // Vector2 size = new Vector2(topR.x - playerPos.x, topR.y - bottomL.y);


            //RaycastHit2D[] hits = Physics2D.BoxCastAll(PlayerBottom, size, 0, Vector2.right);

            // Vector3 position = GetProjectileSpawnPoint(-direction, 10, 0, -240);


            // Posição central do box (não no canto inferior)
            Vector2 center = new Vector2(
                playerPos.x + (frontTop.x - playerPos.x) / 2f, // meio entre player.x e topo direito.x
                (frontTop.y + behindBottom.y) / 2f                  // meio entre topo e bottom
            );

            // Tamanho do retângulo
            Vector2 size = new Vector2(
                Mathf.Abs(frontTop.x - playerPos.x),
                Mathf.Abs(frontTop.y - behindBottom.y)
            );

            Draw.Bounds2D(bounds, Color.red, 10f);
            Debug.Log(bounds);
            // Desenha o retângulo pra debug
            Debug.DrawLine(new Vector2(playerPos.x, frontTop.y), frontTop, Color.blue, 5f);
            Debug.DrawLine(frontTop, new Vector2(frontTop.x, behindBottom.y), Color.blue, 5f);
            Debug.DrawLine(new Vector2(frontTop.x, behindBottom.y), new Vector2(playerPos.x, behindBottom.y), Color.blue, 5f);
            Debug.DrawLine(new Vector2(playerPos.x, behindBottom.y), new Vector2(playerPos.x, frontTop.y), Color.blue, 5f);

            // Detecta colliders dentro da área
            Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f, canHitLayers);
            hits = hits.OrderBy(l => l.gameObject.layer).ToArray();
            float distance = 10000f;
            int index = -1;
            for (int i = 0; i < hits.Length; i++)
            {
                Debug.Log($"hits:{i} name:{hits[i].name} layer:{hits[i].gameObject.layer}");
                if (hits[i].Distance(GameManager.Instance.GetPlayer.GetComponent<Collider2D>()).distance < distance)
                    index = i;
            }
            Debug.Log("Index:" + index);
            //utilizar esse código para um raio que encontre um teto 
            //Vector2 projectilePos;
            //if (index != -1) 
            //{

            //    RaycastHit2D hit2D = Physics2D.Linecast(hits[index].gameObject.transform.position, new Vector2(hits[index].gameObject.transform.position.x, frontTop.y), LayerMask.GetMask("Ground"));

            //    Debug.DrawLine(hits[index].gameObject.transform.position, new Vector2(hits[index].gameObject.transform.position.x, frontTop.y), Color.yellow, 5f);
            //    if (hit2D.collider == null)
            //    {
            //        Debug.Log("null:" + index);
            //        projectilePos = new Vector2(hits[index].gameObject.transform.position.x, frontTop.y - 4);
            //    }
            //    else
            //    {
            //        projectilePos = new Vector2(hits[index].gameObject.transform.position.x, hit2D.point.y - 1);
            //    }
            //}
            //else
            // projectilePos = direction.x > 0 ? bounds.topRight : bounds.topLeft;
            //transform.position = projectilePos;
            float posX = playerPos.x + (2f * direction.x);
            float posY = playerPos.y;
            if (index > -1)
            {
                posX = hits[index].gameObject.transform.position.x;
                posY = hits[index].gameObject.transform.position.y;
            }
            transform.position = new Vector2(posX, posY);
            // Debug.Log("Camera: "+Camera.main.name);
            //  Debug.Log("lightining position: " + projectilePos);
            Debug.Log("lightining direction: " + direction);
            // ps.Play();
            audioSource.Play();
        }
        public void Remove()
        {
            Debug.Log("Lightining remover:");
            // impactZone.GetComponent<CollisionRelayEx>().OnTriggerEnterAction -= LightningBolt_OnTriggerEnterAction;
            Destroy(gameObject);
        }
        public void Impact()
        {
            Debug.Log("Lightining impact:");
            Collider2D raycastHit = Physics2D.OverlapCircle(gameObject.transform.position, 0.2f, canHitLayers);
            if (raycastHit != null)
            {
                Debug.Log("Lightining impact: " + raycastHit.name);
                Finish(raycastHit.gameObject);
            }
            //impactZone.SetActive(true);
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
    }
}
