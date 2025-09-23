using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;
namespace br.com.bonus630.thefrog.Activators
{
    public class TimerSwitcher :  IActivator
    {

        [SerializeField] ParticleSystem effects;
        [SerializeField] Collider2D collider2;
        [SerializeField] AudioSource audioSource;
        [SerializeField] Animator animator;
        [SerializeField] float Timer;
        [SerializeField] bool IsActived = true;
        private bool useTimer = false;

        private float leftTime;
        public bool IsOn { get; private set; } = true;
        private readonly int OnID = Animator.StringToHash("On");
        private  List<Effector2D> effectors= new List<Effector2D>();


        void Awake()
        {
            if (Timer > 0)
                useTimer = true;
            if (!IsActived)
            {
                IsOn = true;
                Switch();
            }
            leftTime = Timer;
            GetComponents<Effector2D>(effectors);
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
           // Debug.Log("Effectors:" + effectors.Count);
            for (int i = 0; i < effectors.Count; i++)
            {
                effectors[i].enabled = IsOn;
            }
            animator.SetBool(OnID, collider2.enabled);
        }

        public override void Activate()
        {
            useTimer = false;
            IsOn = false;
            Switch();
        }

        public override void Deactive()
        {
            useTimer = false;
            IsOn = true;
            Switch();
        }
    }
}
