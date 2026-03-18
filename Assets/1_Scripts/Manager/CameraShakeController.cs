using Cinemachine;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class CameraShakeController 
    {
        private CinemachineVirtualCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin noise;

        public CameraShakeController(CinemachineVirtualCamera camera)
        {
            virtualCamera = camera;
            noise = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }
        public void StartShake(float amplitude, float frequency)
        {
            noise.m_AmplitudeGain = amplitude;
            noise.m_FrequencyGain = frequency;
        }

        public void StopShake()
        {
            if (noise == null)
                return;
            noise.m_AmplitudeGain = 0f;
            noise.m_FrequencyGain = 0f;
        }
    }
}
