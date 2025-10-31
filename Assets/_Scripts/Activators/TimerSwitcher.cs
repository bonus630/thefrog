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
            GetComponents<Effector2D>(effectors);
            if (Timer > 0)
                useTimer = true;
            if (IsActived)
            {
                IsOn = true;
                Switch();
            }
            else
            {
                IsOn = false;
                Switch();
            }
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
                    IsOn = !IsOn;
                    Switch();
                    leftTime = Timer;
                }
            }
        }
        public void Switch()
        {
           
            IsActived = IsOn;
            if (IsOn)
            {
                effects.Play();
                audioSource.Play();
            }
            else
            {
                audioSource.Stop();
                effects.Stop();
            }

            collider2.enabled = IsOn;
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
            IsOn = true;
            Switch();
        }

        public override void Deactive()
        {
            useTimer = false;
            IsOn = false;
            Switch();
        }
    }
}
