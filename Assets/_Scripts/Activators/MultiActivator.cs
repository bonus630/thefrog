using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class MultiActivator : IActivator
    {
        [SerializeField] IActivator[] activators;
        [SerializeField] float timeBetween = 0;
        [SerializeField] bool circularExecution = false;
        bool running = false;
        int current = 0;
        float currentTime = 0;
        private void Update()
        {
            if (!running) return;
            if (currentTime >= timeBetween)
            {
                activators[current].Activate();
                current++;
                if (current >= activators.Length)
                {
                    current = 0;
                    if (!circularExecution)
                        running = false;
                }
                currentTime = 0f;
            }
            else
                currentTime += Time.deltaTime;
        }
        public override void Activate()
        {
            running = true;
        }

        public override void Deactive()
        {
            running = false;
        }


    }
}
