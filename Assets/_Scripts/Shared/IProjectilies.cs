using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class IProjectilies : MonoBehaviour
    {
        public abstract Elements GetElement();
        public abstract void Launch(UnityEngine.Vector2 direction);
        public abstract float ReloadTime();
    }
}