using br.com.bonus630.thefrog.Caracters;
using br.com.bonus630.thefrog.Enemies;
using UnityEngine;
namespace br.com.bonus630.thefrog.Items
{
    public class CollectShuryken : IProjectilies
    {
        [SerializeField] private int shurykens = 1;
        public int Shurykens { get { return shurykens; } }

        public override Elements GetElement() => Elements.Normal;

        public override void Launch(Vector2 direction)
        {

        }

        public override float ReloadTime() => 0f;

        protected void OnTriggerEnter2D(Collider2D collision)
        {
            Player player;
            if (collision.gameObject.TryGetComponent<Player>(out player))
            {
                player.ChangeNumberShurykens(shurykens);

            }
        }
    }
}
