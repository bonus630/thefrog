using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public delegate PlayerDirectorData PlayerDirectorAction(Action action, float time);
    public class PlayerDirectorData
    {
        public PlayerDirectorData(Action action, float time)
        {
            this.Action = action;
            Time = time;
        }
        public PlayerDirectorData(Action action, float time,string actionName = ""):this(action,time)
        {
            this.actionName = actionName;
        }
        public Action Action { get; set; }
        public float Time { get; set; }
        private string actionName;
        public static PlayerDirectorData Wait(float time) => new PlayerDirectorData(PlayerDirectorData.Waiting, time);
        private static void Waiting() { }

        public override string ToString()
        {
            
            return !string.IsNullOrEmpty(actionName) ? actionName : nameof(this.Action);
        }

    }
}
