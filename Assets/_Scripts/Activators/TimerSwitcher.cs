using UnityEngine;
using UnityEngine.Rendering;
namespace br.com.bonus630.thefrog.Activators
{
    public class TimerSwitcher : MonoBehaviour
    {

        [SerializeField] ParticleSystem effects;
        [SerializeField] Collider2D collider2;
        [SerializeField] AudioSource audioSource;
        [SerializeField] Animator animator;
        [SerializeField] float Timer;

        private float leftTime;
        private bool useTimer = false;
        public bool IsOn { get; private set; } = true;
        private readonly int OnID = Animator.StringToHash("On");

        void Awake()
        {
            if (Timer > 0)
                useTimer = true;

            leftTime = Timer;
        }

        // Update is called once per frame
        void Update()
        {
            if (useTimer)
            {
                leftTime -= Time.deltaTime;
                if (leftTime < 0)
                {
                    Switch();
                    leftTime = Timer;
                }
            }
        }
        public void Switch()
        {
            IsOn = !IsOn;
            if (effects.isPlaying)
                effects.Stop();
            else
                effects.Play();
            if (audioSource.isPlaying)
                audioSource.Stop();
            else
                audioSource.Play();

            collider2.enabled = !collider2.enabled;
            animator.SetBool(OnID, collider2.enabled);
        }
    }
}
