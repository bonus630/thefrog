using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class MultiActivator : IActivator
    {
        [SerializeField] IActivator[] activators;
        [field:SerializeField] bool randomize { get; set; } = false;
        [SerializeField] float timeBetween = 0;
        [SerializeField] bool circularExecution = false;
        [SerializeField] bool useDesativation = false;
        bool running = false;
        int current = 0;
        float currentTime = 0;
        private List<IActivator> internList;
        private void Update()
        {
            if (!running) return;
            if (currentTime >= timeBetween)
            {
                internList[current].Activate();
                current++;
                if (current >= internList.Count)
                {
                    internList = GetList();
                    current = 0;
                    running = circularExecution;
                }
                currentTime = 0f;
            }
            else
                currentTime += Time.deltaTime;
        }
        public override void Activate()
        {
            internList = GetList();
            running = true;
        }

        public override void Deactive()
        {
            running = false;
            if (useDesativation)
            {
                for (int i = 0; i < activators.Length; i++)
                {
                    activators[i].Deactive();
                }
            }
        }

        private List<IActivator> GetList()
        {
            List<IActivator> list = new();
            if(randomize)
            {
                while(list.Count < activators.Length)
                {
                    int index = Random.Range(0, activators.Length);
                    if (!list.Contains(activators[index]))
                        list.Add(activators[index]);
                }
            }
            else
                list = activators.ToList<IActivator>();
            return list;
        }

    }
}
