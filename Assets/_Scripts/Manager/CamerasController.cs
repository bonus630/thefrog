using System;
using System.Collections;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using br.com.bonus630.thefrog.Utils;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace br.com.bonus630.thefrog.Manager
{
    public class CamerasController : MonoBehaviour,IService
    {
        //Ainda posso confiar nesta lista de cameras
        [SerializeField] List<GameObject> Cameras;
        [field: SerializeField] public GameObject ThumbCamera { get; protected set; }

        public int LastActiveCam { get; private set; }
        public int LastActiveConfiner { get; private set; }

        public PolygonCollider2D currentConfiner;
        public PolygonCollider2D prevConfiner;
        public CinemachineVirtualCamera currentCamera;



        public void SwitchConfiner(PolygonCollider2D confiner,GameObject nextCamera)
        {
            CinemachineVirtualCamera cam = GetActiveVirtualCamera();
             currentCamera = cam.GetComponent<CinemachineVirtualCamera>();
            if (currentCamera != null)
            {
                if (nextCamera.TryGetComponent<CinemachineVirtualCamera>(out var next))
                {
                    currentCamera.Priority = 10;
                    next.Priority = 20;
                    currentCamera = next;
                }
                currentCamera.GetComponent<CinemachineConfiner>().m_BoundingShape2D = confiner;
                currentCamera.GetComponent<CinemachineConfiner>().InvalidatePathCache();
                currentConfiner = confiner;
            }
        }
        public void LeavingConfiner(PolygonCollider2D confiner)
        {
            if (confiner == prevConfiner)
                prevConfiner = null;
            if (confiner == currentConfiner)
                currentConfiner = null;
            if (!prevConfiner && !currentConfiner)
            {
                currentCamera = GetSkyCam();
                currentCamera.Priority = 20;
            }
        }

        private void Awake()
        {
             ServiceLocator.Instance.Register<CamerasController>(this);
        }
        public CinemachineVirtualCamera GetSkyCam()
        {
            return Cameras[2].GetComponent<CinemachineVirtualCamera>();
        }
        public GameObject ActiveCam(int index)
        {
            if (Cameras == null)
                throw new NullReferenceException("[CamerasController] ");
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
       
            if (Cameras[LastActiveCam] != null && Cameras[LastActiveCam].activeSelf)
            {
                return Cameras[LastActiveCam];
            }
            else
            {
                for (int i = 0; i < Cameras.Count; i++)
                {

                    if (Cameras[i] !=null && Cameras[i].activeSelf)
                        return Cameras[i];
                }
            }
            return CameraUtils.GetActiveVirtualCamera2().gameObject;
            //return null;
        }
        public CinemachineVirtualCamera GetActiveVirtualCamera()
        {
            return GetActiveCamera().GetComponent<CinemachineVirtualCamera>();
        }
      
        public void GameObjectFocus(GameObject gameObject, float time = 1f)
        {
            StartCoroutine(gameObjectFocus(new GameObject[] {gameObject }, time));
        }
        public void GameObjectsFocus(GameObject[] gameObjects, float time = 1f)
        {
            StartCoroutine(gameObjectFocus(gameObjects , time));
        }
        private IEnumerator gameObjectFocus(GameObject[] gameObjects, float time)
        {
            Cinemachine.CinemachineVirtualCamera vCam = GetActiveVirtualCamera();
            CinemachineConfiner confiner;
            bool confinerEnabled = true;
            Transform startFollow = vCam.Follow;
            //Hack para contornar um problema quando se inicia uma cena onde existam muitos camera focus
            if (startFollow.gameObject.name == "Camera")
            {
                if (vCam.TryGetComponent<CinemachineConfiner>(out confiner))
                {
                    confinerEnabled = confiner.enabled;
                    confiner.enabled = false;
                }
                for (int i = 0; i < gameObjects.Length; i++)
                {
                    vCam.Follow = gameObjects[i].transform;
                    yield return new WaitForSeconds(time);

                }
                yield return new WaitForSeconds(time);
                if (confiner != null)
                {
                    confiner.enabled = confinerEnabled;
                }
                vCam.Follow = startFollow;
            }
            else
                yield return null;
            //vCam.Follow = GameManager.Instance.GetPlayer.transform.Find("Camera");

        }
        public void ShakeCameraAndGamepadEffect(int times = 1,bool gamepadRumble = false)
        {
            Transform camera = GetActiveCamera().transform;
            if (camera != null)
            {
                StartCoroutine(shakeCamera(camera, times,gamepadRumble));
            }
        }
        private IEnumerator ShakeCamera2(float frequency,float amplitude,float duration)
        {
            CinemachineVirtualCamera cam = GetActiveVirtualCamera();
            var noise = cam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            noise.m_FrequencyGain = frequency;
            noise.m_AmplitudeGain = amplitude;
            yield return new WaitForSeconds(duration);
            noise.m_AmplitudeGain = 0f;
        }
        //Este método deve ser removido em breve
        private IEnumerator shakeCamera(Transform camera, int times,bool rumble)
        {
            if (rumble && Gamepad.current!=null)
            {
                Gamepad.current.SetMotorSpeeds(0.5f, 0f);
            }
            for (int i = 0; i < times; i++)
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
            }
            if (rumble && Gamepad.current != null)
            {
                Gamepad.current.SetMotorSpeeds(0f, 0f);
            }
            yield return null;
        }

        internal void SetOffSet(Vector2 offsetXY)
        {
            Cinemachine.CinemachineVirtualCamera vCam = GetActiveVirtualCamera();
            var transposer = vCam.GetCinemachineComponent<CinemachineTransposer>();
            Vector3 offset = transposer.m_FollowOffset;
            transposer.m_FollowOffset = new Vector3(offsetXY.x,  offsetXY.y, offset.z);
        }
    }
}

