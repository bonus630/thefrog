using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public abstract class IScheduler : MonoBehaviour
    {
        public abstract void AddAction(SchedulerData schedulerData);
    }
}
