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
        public static SchedulerData Do<T>(Action<T> action, T param, float time = 1f) => new SchedulerData(() =>  action(param) , time);
        public static SchedulerData Do<T1,T2>(Action<T1,T2> action, T1 param1,T2 param2, float time = 1f) => new SchedulerData(() =>  action(param1,param2) , time);
        public static SchedulerData Do<T1,T2,T3>(Action<T1,T2,T3> action, T1 param1,T2 param2,T3 param3, float time = 1f) => new SchedulerData(() =>  action(param1,param2,param3) , time);
        public static SchedulerData Do<T1,T2,T3,T4>(Action<T1,T2,T3,T4> action, T1 param1,T2 param2,T3 param3,T4 param4, float time = 1f) 
            => new SchedulerData(() =>  action(param1,param2,param3,param4) , time);
        private static void Waiting() { }

        public override string ToString()
        {
            
            return !string.IsNullOrEmpty(actionName) ? actionName : Action?.Method.Name;
        }

    }
}
