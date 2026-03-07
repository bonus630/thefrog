using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using br.com.bonus630.thefrog.Shared;

namespace br.com.bonus630.thefrog.Manager
{
    public class PlayerPointsEntry : MonoBehaviour,IService
    {
        [SerializeField] ScreenEffects screenEffects;
        [SerializeField] List<ScenePointsData> ScenePointsDatas;
        [SerializeField] int sceneIndex = -1;
        public Vector3 GetPoint(int pointIndex,int sceneIndex)
        {
            for (int i = 0; i < ScenePointsDatas.Count; i++)
            {
                Debug.Log("[PlayerPointEntry] sceneindex:" + ScenePointsDatas[i].SceneIndex);
                if(sceneIndex== ScenePointsDatas[i].SceneIndex)
                    return ScenePointsDatas[i].PointsData[pointIndex].Point;
            }
          //  Debug.Log("[PlayerPointEntry]" + data.PointsData[pointIndex].Name);
            return Vector3.zero;
            //GameManager.Instance.PlayerStartPosition = data.PointsData[GameManager.Instance.ToPoint].Point;
        }
        public Vector3 GetPoint(int pointIndex)
        {
            return this.GetPoint(pointIndex, this.sceneIndex);
        }
        private void Awake()
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            screenEffects.FadeOut(0f);
        }
        private void Start()
        {
            ServiceLocator.Instance.Register(this);
            screenEffects.FadeIn(1f);
        }
    }
}
