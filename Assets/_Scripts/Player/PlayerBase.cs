using UnityEngine;

namespace br.com.bonus630.thefrog.Player
{
    [RequireComponent(typeof(Player))]
    public abstract class PlayerBase : MonoBehaviour
    {
        protected Player player;
        protected Animator anim;
        protected AudioSource audioSource;
        protected Rigidbody2D rb;

        protected virtual void Awake()
        {
            player = GetComponent<Player>();
            rb = GetComponent<Rigidbody2D>();
            anim = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }


    }
}
