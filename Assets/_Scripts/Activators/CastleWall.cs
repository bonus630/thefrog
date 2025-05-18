using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace br.com.bonus630.Activators
{
    public class CastleWall : MonoBehaviour
    {
        [SerializeField] Tilemap tile;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                tile.color = new Color(1, 1, 1, 0.2f);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                tile.color = new Color(1, 1, 1, 1);
            }
        }
    }
}
