using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

namespace br.com.bonus630.thefrog.Manager
{
    public class CamerasController : MonoBehaviour
    {
        [SerializeField] List<GameObject> Cameras;
     

        public int LastActiveCam { get; private set; }
        public int LastActiveConfiner { get; private set; }

        public GameObject ActiveCam(int index)
        {
            for (int i = 0; i < Cameras.Count; i++)
            {

                Cameras[i].SetActive(false);
            }
            Cameras[index].SetActive(true);
            LastActiveCam = index;
            return Cameras[index];


            
        }

        public GameObject GetActiveCamera()
        {
            if (Cameras[LastActiveCam]!=null && Cameras[LastActiveCam].activeSelf)
                return Cameras[LastActiveCam];
            else
            {
                for(int i = 0;i < Cameras.Count;i++)
                {
                    if (Cameras[i].activeSelf)
                        return Cameras[i];
                }
            }
            return null;    
        }
        public void GameObjectFocus(GameObject gameObject, float time = 1f)
        {
            StartCoroutine(gameObjectFocus(gameObject, time));
        }
        private IEnumerator gameObjectFocus(GameObject gameObject, float time)
        {
            Cinemachine.CinemachineVirtualCamera vCam = GetActiveCamera().GetComponent<CinemachineVirtualCamera>();
            vCam.Follow = gameObject.transform;
            yield return new WaitForSeconds(time);
            vCam.Follow = GameManager.Instance.GetPlayer.transform;

        }
        public void ShakeCameraEffect()
        {
            Transform camera = GetActiveCamera().transform;
            if(!camera.IsUnityNull())
            {
                StartCoroutine(shakeCamera(camera));
            }
        }
        private IEnumerator shakeCamera(Transform camera)
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(359.809998f, -3.25690581e-12f, 0.0299978238f);
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(0.0900041908f, 2.60551389e-11f, 359.849976f);
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(359.869995f, -5.21103438e-11f, 359.5499880f);
            camera.rotation = Quaternion.Euler(0.32999754f, 0, 0.149999827f); 
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(359.809998f, -3.25690581e-12f, 0.0299978238f); 
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(0.0900041908f, 2.60551389e-11f, 359.849976f);
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(0.32999754f, 0, 0.149999827f); 
            yield return new WaitForEndOfFrame();
            camera.rotation = Quaternion.Euler(0.32999754f, 0, 0.149999827f);
            yield return null;
        }
    }
}

