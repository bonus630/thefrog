using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class GameEventObject : ScriptableObject
    {
        private List<GameEventListener> listeners = new();

        public void Raise()
        {
            for (int i = listeners.Count -1; i >= 0; i--)
            {
                listeners[i].OnRaiseEvent();
            }
        }
        public void Register(GameEventListener @event) =>listeners.Add(@event);
        public void UnRegister(GameEventListener @event) =>listeners.Remove(@event);

    }
}
