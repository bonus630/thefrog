using System;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class SpawnerTrap : AreaSpawner
    {
        [SerializeField] int EnemiesToKill;
        [SerializeField] IActivator ItemToActive;
        [SerializeField] float time;

        private int spawnedCounter = 0;

        public override void Activate()
        {
            if (running)
                return;
            base.Activate();
            GameManager.Instance.StartTimer(time, ResetTimer);
        }
        protected override Vector3 GetPoint()
        {
            spawnedCounter++;
            if(spawnedCounter>=EnemiesToKill+this.limit && !this.Actived)
            {
                this.Actived = true;
                ItemToActive.Activate();
                ResetTimer();
            }
            return base.GetPoint();
        }

        private void ResetTimer()
        {
            running = false;
            for (int i = instances.Count-1; i >=0  ; i--)
            {
                Destroy(instances[i].gameObject);
                instances.RemoveAt(i);
            }
            spawnedCounter = 0;
        }
    }
}
