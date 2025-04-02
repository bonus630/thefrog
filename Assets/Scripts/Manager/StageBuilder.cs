using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    public class StageBuilder : IActivator
    {
        [SerializeField] GameObject entracePoint;
        [SerializeField] List<GameObject> randomModules;
        [SerializeField] GameObject endModule;
        [SerializeField] int count;



        private void Awake()
        {

        }
        public void Build()
        {
            Vector3 pos = entracePoint.transform.position;
            if (endModule != null)
            {
                count--;
            }
            for (int i = 0; i < count; i++)
            {
                pos = GenerateModule(randomModules[Random.Range(0, randomModules.Count)], pos);
            }
            if (endModule != null)
            {
                GenerateModule(endModule, pos);
            }
        }
        Vector3 GenerateModule(GameObject modulePrefab, Vector3 startPos)
        {
            var module = Instantiate(modulePrefab);
            Vector3 entry = module.transform.Find("StartPoint").transform.position;
            Vector3 diff = module.transform.position - entry;
            module.transform.position = startPos + diff;
            startPos = module.transform.Find("EndPoint").transform.position;
            return startPos;
        }

        public override void Activate()
        {
            if (!Actived)
            {
                Build();
                Actived = true;
            }
        }

        public override void Deactive()
        {
            Actived = false;
        }
    }
}
