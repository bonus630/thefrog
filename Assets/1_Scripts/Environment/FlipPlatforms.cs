using System.Collections.Generic;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{
    public class FlipPlatforms : MonoBehaviour
    {
        [SerializeField] GameObject[] points;
        [SerializeField] GameObject platform;

        private void Awake()
        {

            for (int i = 0; i < points.Length; i++)
            {
                GameObject plat = Instantiate(platform, points[i].transform.position, platform.transform.rotation);
                FlipPlatform flipPlatform;
                if (plat.TryGetComponent<FlipPlatform>(out flipPlatform))
                {
                    flipPlatform.MaxTime = 2f;
                    flipPlatform.Points = points;
                    flipPlatform.StartPoint = i;
                }
            }
        }

    }

}
