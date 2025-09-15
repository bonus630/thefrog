using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class IProjectilies : MonoBehaviour
    {
        public abstract Elements GetElement { get; set; }

        public abstract void Launch(UnityEngine.Vector2 direction);
        public abstract float ReloadTime();
    }
}