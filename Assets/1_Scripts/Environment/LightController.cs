using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;


namespace br.com.bonus630.thefrog.Environment
{
    public class LightController : IActivator
    {
        [SerializeField] List<t> lights;

        public override void Activate()
        {
            for (int i = 0; i < lights.Count; i++)
            {
                lights[i].prevIntensity = lights[i].intensity;
                lights[i].light.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = lights[i].intensity;  
            }
        }

        public override void Deactive()
        {
            for (int i = 0; i < lights.Count; i++)
                lights[i].light.GetComponent<UnityEngine.Rendering.Universal.Light2D>().intensity = lights[i].prevIntensity;
        }
    }
    [Serializable]
    public class t
    {
        public GameObject light;
        public float intensity;
        [HideInInspector]
        public float prevIntensity;
    }
}
