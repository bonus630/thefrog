using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class SelfDestroy : MonoBehaviour
    {
        public void Destroy()
        {
            Destroy(gameObject);
        }
    }
}
