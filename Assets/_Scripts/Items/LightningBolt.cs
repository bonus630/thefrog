using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    public class LightningBolt :  IProjectilies,IElement
    {
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
            return Elements.Lightning;
        }

        public override void Launch(Vector2 direction)
        {
            float y = Camera.main.ViewportToWorldPoint(Vector3.up).y;
            Vector3 position = GetProjectileSpawnPoint(direction, 10, 0, y);
            transform.position = position;
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
