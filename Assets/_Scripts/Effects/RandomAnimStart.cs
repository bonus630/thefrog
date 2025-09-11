using UnityEngine;

namespace br.com.bonus630.thefrog.Effects
{
    [RequireComponent(typeof(Animator))]
    public class RandomAnimStart : MonoBehaviour
    {
        void Start()
        {
            var anim = GetComponent<Animator>();
            anim.Play(0,-1,Random.Range(0,1));
            anim.speed = Random.Range(0.9f,1.1f);
        }

        
    }
}
