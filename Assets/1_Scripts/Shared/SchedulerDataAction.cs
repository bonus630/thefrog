using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public delegate SchedulerData SchedulerDataAction(Action action, float time);
    public class SchedulerData
    {
        public SchedulerData(Action action, float time)
        {
            this.Action = action;
            Time = time;
        }
      
        public SchedulerData(Action action, float time,string actionName = ""):this(action,time)
        {
            this.actionName = actionName;
        }
        public Action Action { get; set; }
        public float Time { get; set; }
        private string actionName;
        public static SchedulerData Wait(float time) => new SchedulerData(SchedulerData.Waiting, time);
        public static SchedulerData Do(Action action, float time = 1) => new SchedulerData(action, time);
       // public static SchedulerData Do<T>(Action<T> action, float time) => new SchedulerData(action, time);
        private static void Waiting() { }

        public override string ToString()
        {
            
            return !string.IsNullOrEmpty(actionName) ? actionName : Action?.Method.Name;
        }

    }
}
