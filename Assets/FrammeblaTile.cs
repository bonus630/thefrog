using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace br.com.bonus630.thefrog
{
    public class FrammeblaTile : MonoBehaviour,IElement
    {

        Tilemap tileMap;
        [SerializeField] GameObject fire;
        HashSet<Vector3Int> tilesPos = new();
        

        private void Start()
        {
            tileMap = GetComponent<Tilemap>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            Debug.Log("[frammeblaTile] collision:" + collision.gameObject.name);
            if (collision.collider.TryGetComponent<IElement>(out IElement el) && el.GetElement == Elements.Fire )
            {

                Vector2 point = collision.GetContact(0).point;
                Vector3Int tilePos = tileMap.WorldToCell(point);
                if (tilesPos.Contains(tilePos))
                    return;
                tilesPos.Add(tilePos);
                tileMap.SetTileFlags(tilePos, TileFlags.None);
                tileMap.SetTile(tilePos, null);
                // tileMap.SetColor(tilePos, Color.red);
               
                Instantiate(fire, point, Quaternion.identity);

            }
        }

        [field: SerializeField] public  Elements GetElement { get; set; } = Elements.Fire;
        [field: SerializeField] public Color ElementColor { get; set; } = Color.red;

        public Elements CanActiveBy() => Elements.Fire;
        public Elements CanDeactiveBy() => Elements.Water;
   

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
    }
}
