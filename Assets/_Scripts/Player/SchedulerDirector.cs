using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Player
{
    public class SchedulerDirector : IScheduler
    {
        private Queue<SchedulerData> queue = new();
        private bool running;
        
        float actionStartTime = 0;
        float actionTime = 0;


        void Update()
        {
            if (!running)
                return;

            // Ainda aguardando a ação atual terminar
            if (Time.realtimeSinceStartup < actionStartTime + actionTime)
                return;

            // Ação terminou → pegar próxima
            if (queue.Count == 0)
            {
                running = false;
                return;
            }

            ExecuteNext();
        }

        public override void AddAction(SchedulerData action)
        {
            queue.Enqueue(action);

            Debug.Log("[Playerdirector] " + queue.Count + " tarefas na fila");
            if (!running)
                ExecuteNext();
        }

        private void ExecuteNext()
        {
            var data = queue.Dequeue();

            data.Action.Invoke();
            Debug.Log("[Playerdirector] Tarefa " + data + " iniciou!");
            actionTime = data.Time;
            actionStartTime = Time.realtimeSinceStartup;
            running = true;
        }

    }
    

}
