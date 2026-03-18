using UnityEngine;
using br.com.bonus630.thefrog.Utils;

namespace br.com.bonus630.thefrog.Environment
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class AreaSpawner : Spawner
    {
        Bounds box;
        protected override void Start()
        {
            base.Start();
            box = GetComponent<BoxCollider2D>().bounds;
        }
        protected override Vector3 GetPoint() => box.RandomVector2();
       
    }
}
