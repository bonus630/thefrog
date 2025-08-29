using System;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class ActivatorSlot : IActivator
    {
        public event Action<int,bool> Activated;
        [SerializeField] int id;
        public override void Activate()
        {
            Activated?.Invoke(id,true);
        }

        public override void Deactive()
        {
            Activated?.Invoke(id, false);
        }
    }
}
