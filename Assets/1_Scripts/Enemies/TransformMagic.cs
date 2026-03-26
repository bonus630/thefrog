using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    public class TransformMagic : MonoBehaviour
    {
        [SerializeField] GameObject toTransform;
        [SerializeField] GameObject transformEffect;
        [SerializeField] float time = 2f;

        CircleCollider2D coll;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            coll = GetComponent<CircleCollider2D>();
        }

        // Update is called once per frame
        void Update()
        {
            time -= Time.deltaTime;
            if(time<0)
            {
                GetComponent<Animator>().SetTrigger("explode");
            }
        }
        public void Transformation()
        {
            var hit = Physics2D.CircleCastAll(transform.position, 4f, Vector2.zero);
            if(hit!=null)
            {
                for (int i = hit.Length - 1; i >= 0; i--)
                {
                    if (hit[i].collider.gameObject.TryGetComponent<IProjectilies>(out var p))
                    {
                        Instantiate(transformEffect, p.transform.position, Quaternion.identity);
                        Instantiate(toTransform, p.transform.position, Quaternion.identity);
                        Destroy(p.gameObject);
                    }
                }
            }
            //IProjectilies[] t = FindObjectsByType<IProjectilies>(FindObjectsSortMode.None);
            //Debug.Log("[TransformMagic] t count:"+t.Length);
            //for (int i = t.Length - 1; i >= 0; i--)
            //{
            //    if (coll.IsTouching(t[i].GetComponent<CapsuleCollider2D>()))
            //    {

            //        Instantiate(toTransform, t[i].transform.position, Quaternion.identity);
            //        Destroy(t[i].gameObject);
            //    }
            //}
            Destroy(gameObject);
        }
        //private void OnTriggerEnter2D(Collider2D collision)
        //{
        //    if(collision.TryGetComponent<IProjectilies>(out var p))
        //    {
        //        Instantiate(toTransform, p.transform.position, Quaternion.identity);
        //        Destroy(p.gameObject);
        //    }
        //}
    }
}
