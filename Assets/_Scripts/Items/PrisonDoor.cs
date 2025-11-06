using System;
using System.Collections;
using br.com.bonus630.thefrog.Manager;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Items
{
    [SelectionBase]
    public class PrisonDoor : IActivator
    {
        [SerializeField] Collider2D doorCollider;
        [SerializeField] Transform doorGrid;
        [SerializeField] float operationSpeed = 0.05f;
        [SerializeField] AudioClip startSound;
        [SerializeField] AudioClip loopSound;
        [SerializeField] AudioClip endSound;
        [SerializeField] string DoorID;

        [field: SerializeField] public bool IsOpened { get; set; } = false;

        Vector3 closePosition = Vector3.zero;
        Vector3 openPosition = Vector3.up * 0.5f;
        AudioSource audioSource;
        bool useSound = true;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
            if (GameManager.Instance.IsActived(this.DoorID))
                setOpenClose(doorGrid, openPosition, true, true);
        }

        public override void Activate()
        {
            if (this.Actived)
                return;
            StopAllCoroutines();
            audioSource.Stop();
            StartCoroutine(OpenClose(doorGrid, openPosition, operationSpeed, true));
            if (useSound)
                StartCoroutine(PlayAudioSequence());
        }

        public override void Deactive()
        {
            if (!this.Actived)
                return;
            StopAllCoroutines();
            audioSource.Stop();
            StartCoroutine(OpenClose(doorGrid, closePosition, operationSpeed, false));
            if (useSound)
                StartCoroutine(PlayAudioSequence());
        }

        private IEnumerator OpenClose(Transform transform, Vector3 destine, float speed = 5f, bool opening = true)
        {
            while (Vector3.Distance(transform.localPosition, destine) > 0.00001f)
            {
                Vector3 t = Vector3.MoveTowards(transform.localPosition, destine, speed * Time.deltaTime);
                transform.localPosition = t;
                yield return null;
            }
            setOpenClose(transform, destine, opening);
            audioSource.loop = false;
            audioSource.clip = endSound;
            audioSource.Play();
        }
        private void setOpenClose(Transform transform, Vector3 destine, bool opened, bool useSound = true)
        {
            transform.localPosition = destine;
            IsOpened = opened;
            this.Actived = IsOpened;
            doorCollider.enabled = IsOpened;
            this.useSound = useSound;
            GameManager.Instance.SetActived(this.DoorID, opened);
        }
        private IEnumerator PlayAudioSequence()
        {
            // 1. Toca o som inicial
            audioSource.loop = false;
            audioSource.clip = startSound;
            audioSource.Play();

            // espera o som inicial terminar
            yield return new WaitForSeconds(startSound.length);

            // 2. Toca o loop
            audioSource.loop = true;
            audioSource.clip = loopSound;
            audioSource.Play();

            //// espera até quase o fim da viagem (deixa espaço pro áudio final)
            //yield return new WaitForSeconds(travelTime - loopSound.length);

            //// 3. Toca o som final
            //audioSource.loop = false;
            //audioSource.clip = endSound;
            //audioSource.Play();
        }

    }
}
