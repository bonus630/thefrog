using UnityEngine;
namespace br.com.bonus630.thefrog.Shared
{
    public abstract class IActivator : MonoBehaviour
    {
        public bool Actived { get; set; }
        public abstract void Activate();
        public abstract void Deactive();


    }
}