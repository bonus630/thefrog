using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

namespace br.com.bonus630.thefrog.Enemies
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class EnemyFlyBase : EnemyBase
    {
        [SerializeField] Sprite fallSprite;
        [SerializeField] AudioClip[] deadClips;
        protected AudioSource audioSource;
        private bool continueToDie = false;


        protected override void Start()
        {
            animator = GetComponent<Animator>();
            coll = GetComponent<Collider2D>();
            audioSource = GetComponent<AudioSource>();
        }

        public override void Hit(float hit)
        {
            this.life = this.life - hit;
            animator.SetTrigger(HitID);
            if (life < 0.1f)
                StartCoroutine(DieFall());

            //coll.enabled = false;
            //gameObject.layer = 0;
            //enabled = false;
            //   IsEnable = false;
            //  Invoke(nameof(Restore), 1f);
        }
        public void ContinueToDie()
        {
            if (life < 0.1f)
                continueToDie = true;
        }
        IEnumerator DieFall()
        {
            if (!continueToDie)
                yield return null;
            Debug.Log("Sprite: " + fallSprite);
            audioSource.loop = false;
            audioSource.PlayOneShot(deadClips[UnityEngine.Random.Range(0, deadClips.Length)]);
            speed = 0;
            gameObject.layer = 0;
            coll.enabled = false;
            animator.enabled = false;
            GetComponent<SpriteRenderer>().sprite = fallSprite;
            GetComponent<SpriteRenderer>().flipY = true;

            Vector3 startPos = transform.position;
            Vector3 endPos = new Vector3(
                transform.position.x,
                transform.position.y - 10,
                transform.position.z
            );

            float duration = 1.2f;
            float time = 0;

            while (time < duration)
            {
                time += Time.deltaTime;
                float t = time / duration;

                // queda principal
                Vector3 pos = Vector3.Lerp(startPos, endPos, t);

                // curva só no começo
                float curve = Mathf.Sin(t * Mathf.PI) * (1f - t);
                pos.x += curve * 2f; // ajusta aqui

                transform.position = pos;

                yield return null;
            }

            Destroy(gameObject, 0.2f);

        }
    }
}
