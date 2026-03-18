using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Environment
{
    public class SceneMover : IActivator
    {
        [SerializeField] public int ToPoint;
        [SerializeField] public ScenePointsData scenePointsData;
        [SerializeField] private bool useCurrentHour = true;

        bool isActived = false;
        public override void Activate()
        {
            if (isActived)
                return;
            isActived = true;
            Debug.Log("[SceneMover] point:" + ToPoint);
            if(!useCurrentHour)
                GameManager.Instance.EnvironmentStates.playerStates.Hour = scenePointsData.PointsData[ToPoint].Hour;
            GameManager.Instance.GetPlayerScript.FreezePlayer();
            ServiceLocator.Instance.Get<ScreenEffects>().FadeOut(0.4f);
            Debug.Log("[SceneMover] player:" + ToPoint);
            GameManager.Instance.ToPoint = ToPoint;
            GameManager.Instance.LoadGame(scenePointsData.SceneType,sceneBuildIndex:scenePointsData.SceneIndex);
        }

        public override void Deactive()
        {
            
        }
      
    }
}
