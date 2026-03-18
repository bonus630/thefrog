using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Shared;
using Unity.Collections;
using UnityEditor;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    public class StageBuilder1 : IActivator
    {
        [SerializeField] GameObject entracePoint;
        [SerializeField] GameObject moduleBuildSegment;
        [SerializeField] int count;
        [Tooltip("Use 0 to infinity")][SerializeField] int moduleLimit = 0;
        [SerializeField] bool removeEnd = true;
        [SerializeField] List<GameObject> randomModules;
        List<BuildSegment> buildSegments;
        Dictionary<int, int> counter;
        private int currentIndex = 0;
        private readonly string STARTPOINT = "StartPoint";
        private readonly string ENDPOINT = "EndPoint";

        Vector3 pos;
        private void Awake()
        {
            buildSegments = new List<BuildSegment>();

        }
        public void Build()
        {
            pos = entracePoint.transform.position;

            for (int i = 0; i < count; i++)
            {

                pos = GenerateModule(randomModules[GetModuleIndex()], pos);
            }

        }
        Vector3 GenerateModule(GameObject modulePrefab, Vector3 startPos)
        {
            var module = Instantiate(moduleBuildSegment, transform);
            BuildSegment bs = module.GetComponent<BuildSegment>();
            bs.Inflate(modulePrefab, currentIndex);
            currentIndex++;
            bs.TriggerChanged += Bs_TriggerChanged;
            buildSegments.Add(bs);
            //  Debug.Log("[Stagebuild][GenerateModule] pos:" + pos);
            bs.SetPositionByStart(pos);
            //  Debug.Log("[Stagebuild][GenerateModule] bs.EndPoint:" + bs.EndPoint);
               

            return bs.EndPoint;
        }
        void GenerateModuleReverse(BuildSegment current)
        {
            Debug.Log("[Stagebuild][GenerateModuleReverse] index:" + current.index);
            var module = Instantiate(moduleBuildSegment, transform);
            BuildSegment bs = module.GetComponent<BuildSegment>();
            bs.Inflate(randomModules[GetModuleIndex()], currentIndex);
            currentIndex++;
            bs.TriggerChanged += Bs_TriggerChanged;
            bs.TriggerReset += Bs_TriggerReset;
            buildSegments.Add(bs);
            Vector3 end = current.StartPoint;
            bs.SetPositionByEnd(end);
          
        }

        private void Bs_TriggerReset()
        {
            Restart();
        }

        bool checkRemove = false;
        BuildSegment left = null;
        BuildSegment right = null;
        BuildSegment current = null;
        private void Bs_TriggerChanged(bool enter, int index)
        {

            //   Debug.Log("[Stagebuild][Bs_TriggerChanged] index:" + index);

            if (enter)
            {
                current = buildSegments.SingleOrDefault(r => r.index == index);
                if (!PlayerIsHere())
                    return;
                GetSegments(current, out left, out right);
                Debug.Log("[Stagebuild][Bs_TriggerChanged] " +
                    " index: " + index +
                    " left:" + (left != null ? left.index : "null") +
                    " current:" + (current != null ? current.index : "null") +
                    " right:" + (right != null ? right.index : "null"));
                Debug.Log("[Stagebuild][Bs_TriggerChanged] index: " + index + " Player:" + ServiceLocator.Instance.Get<IPlayer>().BodyTouching(current.box));
                if (index > 0 && left == null)
                {
                    GenerateModuleReverse(current);
                }
                if (right == null)
                {
                    pos = current.EndPoint;
                    // Debug.Log("[Stagebuild][Bs_TriggerChanged] entrei aqui pos:" + pos);
                    GenerateModule(randomModules[GetModuleIndex()], pos);
                }
                if (checkRemove)
                {

                    for (int i = buildSegments.Count - 1; i > 0 ; i--)
                    {
                        if (buildSegments[i] != current && buildSegments[i] != left && buildSegments[i] != right)
                        {
                            buildSegments[i].TriggerChanged -= Bs_TriggerChanged;
                            Destroy(buildSegments[i].gameObject);
                            buildSegments.Remove(buildSegments[i]);
                        }
                    }
                    checkRemove = false;
                }
            }
            else
            {
                checkRemove = true;
            }
        }
        private bool PlayerIsHere()
        {
            if (current == null)
                return false;
            IPlayer player = ServiceLocator.Instance.Get<IPlayer>();
            if (player.BodyTouching(current.box))
                return true;
            for (int i = 0; i < buildSegments.Count; i++)
            {
                if (player.BodyTouching(buildSegments[i].box))
                {
                    current = buildSegments[i];
                    return true;
                }
            }
            return false;
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
            return;
            Actived = false;
            for (int i = 0; i < buildSegments.Count; i++)
            {
                buildSegments[i].TriggerChanged -= Bs_TriggerChanged;
                buildSegments.RemoveAt(i);
                Destroy(buildSegments[i].gameObject);
            }
        }
        private void Restart()
        {
            Debug.Log("Restart");
            for (int i = buildSegments.Count - 1; i >= 0; i--)
            {

                buildSegments[i].TriggerChanged -= Bs_TriggerChanged;
                buildSegments[i].TriggerReset -= Bs_TriggerReset;
                Destroy(buildSegments[i].gameObject);
                buildSegments.Remove(buildSegments[i]);

            }

            pos = entracePoint.transform.position;
        }
        public void GetSegments(BuildSegment current, out BuildSegment left, out BuildSegment right)
        {
            left = null;
            right = null;

            foreach (var seg in buildSegments)
            {
                if (current.StartPoint == seg.EndPoint)
                    left = seg;
                if (current.EndPoint == seg.StartPoint)
                    right = seg;
            }
        }
        //public void GetSegments(BuildSegment current, out BuildSegment left, out BuildSegment right)
        //{
        //    left = null;
        //    right = null;

        //    float closestLeft = float.NegativeInfinity;
        //    float closestRight = float.PositiveInfinity;

        //    foreach (var seg in buildSegments)
        //    {
        //        float x = seg.transform.position.x;

        //        if (x <= current.transform.position.x && x > closestLeft && seg != current)
        //        {
        //            closestLeft = x;
        //            left = seg;
        //        }

        //        if (x > current.transform.position.x && x < closestRight && seg != current)
        //        {
        //            closestRight = x;
        //            right = seg;
        //        }
        //    }
        //}
    }
}
