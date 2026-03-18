using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace br.com.bonus630.thefrog.Items
{
    public class FrammeblaTile : Frammabla
    {

        Tilemap tileMap;
        HashSet<Vector3Int> tilesPos = new();
        

        private void Start()
        {
            tileMap = GetComponent<Tilemap>();
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
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
                int cont = tileMap.GetUsedTilesCount();
                Instantiate(fire, point, Quaternion.identity);
            }
        }


    }
}
