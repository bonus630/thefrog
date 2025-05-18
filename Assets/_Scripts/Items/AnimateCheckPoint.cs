using UnityEngine;
using System.Collections.Generic;
using System.Collections;
namespace br.com.bonus630.thefrog.Items
{
    public class AnimateCheckPoint : Checkpoint
    {
        private Animator anim;
        //  Color white;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            anim = GetComponent<Animator>();
            //  white = new Color(1, 1, 1, 0.5f);
        }

        protected override void Check()
        {
           // Debug.Log("animad base checkpoint");
            anim.SetBool("checked", true);
            base.Check();
            StartCoroutine(uncheck());
        }
        IEnumerator uncheck()
        {
            yield return new WaitForSeconds(30);
            anim.SetBool("checked", false);
            base.UnCheck();

        }
    }
}

