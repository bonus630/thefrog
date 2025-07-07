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
        [Tooltip("Use 0 to infinity")][SerializeField] int moduleLimit = 0;
        List<GameObject> modules;
        Dictionary<int, int> counter;

        private void Awake()
        {
            modules = new List<GameObject>();

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

                pos = GenerateModule(randomModules[GetModuleIndex()], pos);
            }
            if (endModule != null)
            {
                GenerateModule(endModule, pos);
            }
        }
        Vector3 GenerateModule(GameObject modulePrefab, Vector3 startPos)
        {
            var module = Instantiate(modulePrefab);
            modules.Add(module);
            Vector3 entry = module.transform.Find("StartPoint").transform.position;
            Vector3 diff = module.transform.position - entry;
            module.transform.position = startPos + diff;
            startPos = module.transform.Find("EndPoint").transform.position;
            return startPos;
        }
        private int GetModuleIndex()
        {
            int moduleIndex = Random.Range(0, randomModules.Count);
            if (IgnoreLimit())
                return moduleIndex;
            if (counter == null)
                counter = new Dictionary<int, int>();
            if (counter.ContainsKey(moduleIndex))
            {
                if (counter[moduleIndex] < moduleLimit)
                    counter[moduleIndex]++;
                else
                    return GetModuleIndex();
            }
            else
                counter.Add(moduleIndex, 1);
            return moduleIndex;
        }
        private bool IgnoreLimit()
        {
            if (moduleLimit == 0)
                return true;
            int uses = moduleLimit * randomModules.Count;
            //if (endModule != null)
            //    uses++;
            return uses >= count;
           
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
            for (int i = 0; i < modules.Count; i++)
            {
                Destroy(modules[i]);
            }
        }
    }
}
