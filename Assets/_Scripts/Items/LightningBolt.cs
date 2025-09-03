using System.Linq;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class LightningBolt : IProjectilies, IElement
    {
        
        [SerializeField] LayerMask canHitLayers;
        [SerializeField] AudioSource audio;
        ParticleSystem ps;

        private void Awake()
        {
            ps = GetComponent<ParticleSystem>();
            audio = GetComponent<AudioSource>();
        }

        private void OnParticleCollision(GameObject other)
        {
            Debug.Log("Particles :" + other.name);
           // ps.Stop();
        }
        private void OnParticleSystemStopped()
        {
            Debug.Log("ParticleSystem terminou!");
            // Aqui você pode chamar qualquer método
           
        }
        public void ActiveBy(Elements element)
        {
            throw new System.NotImplementedException();
        }

        public Elements CanActiveBy()
        {
            throw new System.NotImplementedException();
        }

        public Elements CanDeactiveBy()
        {
            throw new System.NotImplementedException();
        }

        public void DeactiveBy(Elements element)
        {
            throw new System.NotImplementedException();
        }

        public override Elements GetElement()
        {
            return Elements.Lightining;
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
                frontTop.x - playerPos.x,
                frontTop.y - behindBottom.y
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
                if (hits[i].gameObject.layer == 6 && hits[i].Distance(GameManager.Instance.GetPlayer.GetComponent<Collider2D>()).distance < distance)
                    index = i;
            }
            Debug.Log("Index:" + index);
            Vector2 projectilePos;
            if (index != -1)
            {

                RaycastHit2D hit2D = Physics2D.Linecast(hits[index].gameObject.transform.position, new Vector2(hits[index].gameObject.transform.position.x, frontTop.y), LayerMask.GetMask("Ground"));

                Debug.DrawLine(hits[index].gameObject.transform.position, new Vector2(hits[index].gameObject.transform.position.x, frontTop.y), Color.yellow, 5f);
                if (hit2D.collider == null)
                {
                    Debug.Log("null:" + index);
                    projectilePos = new Vector2(hits[index].gameObject.transform.position.x, frontTop.y - 4);
                }
                else
                {
                    projectilePos = new Vector2(hits[index].gameObject.transform.position.x, hit2D.point.y - 1);
                }
            }
            else
                projectilePos = direction.x > 0 ? bounds.topRight : bounds.topLeft;
            transform.position = projectilePos;
            // Debug.Log("Camera: "+Camera.main.name);
            Debug.Log("lightining position: " + projectilePos);
            Debug.Log("lightining direction: " + direction);
            ps.Play();
            audio.Play();
        }

        public override float ReloadTime() => 1f;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
        public void Remove()
        {
            Destroy(gameObject);
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
