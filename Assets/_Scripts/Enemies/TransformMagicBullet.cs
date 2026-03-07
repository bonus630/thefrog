using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace br.com.bonus630.thefrog.Enemies
{
    public class TransformMagicBullet : MonoBehaviour
    {
        [SerializeField] GameObject[] toTransform;
        [SerializeField] LayerMask layerMask;
        [SerializeField] float speed = 4;
        [SerializeField][Range(-1, 1)] int direction = -1;
        [SerializeField] AudioClip audioClip;
        public int Direction { get { return direction; } set { direction = value; } }
        private bool actived = false;
        private bool contact = false;
        float collidedTimer = 0;
        public Vector2 finalPos;
        private void Start()
        {
            
         
        }


        private void Update()
        {
            transform.position = Vector2.MoveTowards(transform.position, finalPos, speed * Time.deltaTime);
            if (contact)
                collidedTimer += Time.deltaTime;
            if (collidedTimer >= 3)
                Destroy(gameObject);
        }
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            contact = true;
            Debug.Log("[transformmagicbullet] collision:" + collision.gameObject.layer);
            if (actived)
                return;
            if(collision.gameObject.IsInLayerMask(layerMask))
            {
                actived = true;
                GetComponent<Animator>().SetTrigger("hit");
                ServiceLocator.Instance.Get<AudioEffects>().Play(audioClip);
                Tilemap tileMap =  collision.gameObject.GetComponent<Tilemap>();
               
                Debug.Log("[TransformMagicBuller] contact normal:"+ collision.GetContact(0).normal);
               var normalV = collision.GetContact(0).normal;
                Vector3Int v1 = tileMap.WorldToCell(collision.GetContact(0).point);
                Vector3Int v0, v2;
                Instantiate(toTransform[Random.Range(0,toTransform.Length)], tileMap.CellToWorld(v1), Quaternion.FromToRotation(Vector3.up,normalV));

                Vector3Int[] v = new Vector3Int[3]; 

                if(normalV.y > 0)
                {
                    v[0] = v1;
                    v0 = v1 + Vector3Int.left;
                    v[1] = v0;

                    v2 = v1 + Vector3Int.right;
                    v[2] = v2;
                }

                // var tile = tileMap.GetTile(v);
                for (int i = 0; i < v.Length; i++)
                {


                    tileMap.SetTileFlags(v[i], TileFlags.None);
                    tileMap.SetTile(v[i], null);

                }
                return;
            }
            //Destroy(gameObject);
        }

        public void MagicEffect()
        {
            
            Destroy(gameObject);
        }

    }
}
