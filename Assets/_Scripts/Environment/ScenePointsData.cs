using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    [Serializable]
    public struct PointData
    {
        public string Name;
        public Vector3 Point;
        [HideInInspector] public Transform transform;
    }
    [CreateAssetMenu(fileName = "_ScenePointsData", menuName = "ScriptableObject/DScenePointsData", order = 1)]
    public class ScenePointsData : ScriptableObject
    {
        public int SceneIndex;
        public List<PointData> PointsData;

        public SceneStartType SceneType;
    }
}
