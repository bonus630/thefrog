using br.com.bonus630.thefrog.Shared;
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
            if (collision.gameObject.TryGetComponent<IPlayer>(out IPlayer player))
            {
                player.ChangeNumberShurykens(shurykens);

            }
        }
    }
}
