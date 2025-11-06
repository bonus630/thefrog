using System.Collections.Generic;
using System.Runtime.CompilerServices;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Activators
{
    [RequireComponent(typeof(Collider2D))]
    public class DisableEnableChildrens : IActivator
    {
        [field: SerializeField] public List<IActivator> ExternActivators { get; set; }

        //private const float MAXINTERVAL = 0.1f;
        //private float timer = 0;

        Collider2D coll;
        public override void Activate()
        {
            for (int i = 0; i < ExternActivators.Count; i++)
            {
                ExternActivators[i].Activate(); 
            }
            for(int i = 0;i < transform.childCount;i++)
            {
                transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        public override void Deactive()
        {
            for (int i = 0; i < ExternActivators.Count; i++)
            {
                ExternActivators[i].Deactive();
            }
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.SetActive(false);
            }
        }
        public void Change(bool flag)
        {
            if (flag)
                Activate();
            else
                Deactive();
        }
        void Start()
        {
            coll = GetComponent<Collider2D>();
            Vector3 point = GameManager.Instance.GetPlayer.transform.position;
            Change(coll.OverlapPoint(point));

        }

        //void Update()
        //{
        //    if(timer<=0)
        //    {

        //    }
        //}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.CompareTag("Player"))
            {
                Change(true);
            }
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Change(false);
            }
        }
        
    }
}
