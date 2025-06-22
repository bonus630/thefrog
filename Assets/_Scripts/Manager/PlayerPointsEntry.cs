using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace br.com.bonus630.thefrog.Manager
{
    public class PlayerPointsEntry : IActivator
    {
        [SerializeField] ScreenEffects screenEffects;
        [SerializeField] List<ScenePointsData> ScenePointsDatas;
        [SerializeField] int sceneIndex = -1;
        //public Vector3 this[int index] { get => ScenePointsData.PointsData[index].Point; }

        public override void Activate()
        {
            Debug.Log("PlayerPointenttry");
            ScenePointsData data = ScenePointsDatas.SingleOrDefault(s=>s.SceneIndex == sceneIndex);
            Debug.Log("PlayerPointenttry "+data);
            if (data != null)
                GameManager.Instance.PlayerStartPosition = data.PointsData[GameManager.Instance.ToPoint].Point;
            Debug.Log("PlayerPointenttry :"+ GameManager.Instance.PlayerStartPosition);
        }

        public override void Deactive()
        {
            
        }

        private void Awake()
        {
            sceneIndex = SceneManager.GetActiveScene().buildIndex;
            screenEffects.screenFader.fadeImage.color = Color.black;
        }
        private void Start()
        {
            screenEffects.FadeIn(1f);
        }
    }
}
