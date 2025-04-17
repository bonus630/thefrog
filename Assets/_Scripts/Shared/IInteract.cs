

using UnityEngine;
namespace br.com.bonus630.thefrog.Shared
{
    public interface IInteract
    {
        void Interact();
        bool ReadyToInteract(bool lookFor);
        Transform GetTransform();
    }
}