using System.Collections;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class AnimationBasicEvents : MonoBehaviour
    {

        public void SelfDestroy(float time)
        {
            Destroy(gameObject, time);
        }
    }
}