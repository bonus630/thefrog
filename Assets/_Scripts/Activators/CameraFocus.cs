using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class CameraFocus : IActivator
    {
        public GameObject[] gameObjects;
        [SerializeField] ScreenEffects screenEffects;
        public float time = 0.5f;
        [SerializeField] bool disablePlayerMove = false;
        [SerializeField] bool oneTime = false;
        int runtime = 0;
        public override void Activate()
        {
            if (oneTime && runtime > 0)
                return;
            runtime++;
            screenEffects.GameObjectsFocus(gameObjects, time);
            if (disablePlayerMove)
            {
                GameManager.Instance.GetPlayerScript.MoveInputOn = false;
                StartCoroutine(RenablePlayerInput(time * gameObjects.Length));
            }
        }

        public override void Deactive()
        {
            
        }
        private IEnumerator RenablePlayerInput(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.Instance.GetPlayerScript.MoveInputOn = true;
        }
    }
}
