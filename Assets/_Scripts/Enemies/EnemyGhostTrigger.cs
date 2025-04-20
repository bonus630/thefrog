using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using br.com.bonus630.thefrog.Activators;
using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Items;
using UnityEngine;

namespace br.com.bonus630.thefrog.Assets._Scripts.Enemies
{
    public class EnemyGhostTrigger : EnemyGhost
    {
        protected override void Update()
        {
            base.Update();
            Collider2D[] hits = new Collider2D[10];
            ContactFilter2D filter = new ContactFilter2D();
            filter.SetLayerMask(LayerMask.GetMask("Projectiles")); // ou a nova layer
            filter.useTriggers = true;

            int count = Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, hits);
            if(count > 0) Debug.Log("Projetéis detectados: " + count);
        }
        protected  void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("ghost trigger:" + collision.gameObject.layer);
            if (collision.gameObject.CompareTag("Player"))
            {
                Player player;

                if (collision.gameObject.TryGetComponent<Player>(out player))
                {
                    player.KnockUp(repulse);
                    player.Hit();
                    return;
                }

            }
            if (collision.gameObject.layer == 12)
            {
                if (collision.gameObject.TryGetComponent<IProjectilies>(out IProjectilies p))
                {
                    if (enemyWeakenesses.Contains(p.GetElement()))
                    {
                        Hit(2);
                    }
                }
            }
        }
    }
}
