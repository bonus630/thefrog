using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    public class CameraFocus : IActivator
    {
        [SerializeField] GameObject[] gameObjects;
        [SerializeField] ScreenEffects screenEffects;
        [SerializeField] float time = 0.5f;
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
                GameManager.Instance.GetPlayerScript.InputsOn = false;
                StartCoroutine(RenablePlayerInput(time * gameObjects.Length));
            }
        }

        public override void Deactive()
        {
            
        }
        private IEnumerator RenablePlayerInput(float time)
        {
            yield return new WaitForSeconds(time);
            GameManager.Instance.GetPlayerScript.InputsOn = true;
        }
    }
}
