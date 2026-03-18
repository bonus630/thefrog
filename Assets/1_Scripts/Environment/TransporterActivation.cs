using System.Collections;
using System.Collections.Generic;
using System.Linq;
using br.com.bonus630.thefrog.Manager;
using UnityEngine;
namespace br.com.bonus630.thefrog.Environment
{

    public class TransporterActivation : MonoBehaviour
    {
        [SerializeField] List<Vector2> startPosition;
        [SerializeField] private bool active;
        [SerializeField] Transporter transporter;
        [SerializeField] string ID;
        SpriteRenderer render;
        float time;
        [ContextMenu("Adicionar posição atual ao StartPosition")]
        private void AddCurrentPositionToStartPosition()
        {
            if (startPosition == null)
                startPosition = new();
            startPosition.Add(transform.position);
            //Debug.Log($"StartPosition agora tem {startPosition.Count} elementos");
        }
        void Start()
        {
            render = GetComponent<SpriteRenderer>();
            //Debug.Log($"[TransporterActivation][Start] name: {gameObject.name} isActived:{GameManager.Instance.IsActived(ID)}");
            if (GameManager.Instance.IsActived(ID))
            {
                VisualActivation();
                transform.position = startPosition[startPosition.Count - 1];
                transporter.active = true;
                transporter.Init();
                this.enabled = false;
                return;
            }
            transporter.OnePass += Transporter_OnePass;
        }

        private void Transporter_OnePass()
        {
            //talvez preciso remover o primeiro ponto?
            //transporter.destines = startPosition.ToArray();
            transporter.active = true;
            transporter.Init();
            transporter.OnePass -= Transporter_OnePass;
            GameManager.Instance.EnvironmentStates.Activeds.Add(ID);
            this.enabled = false;
        }
        private void StartUpTransport(List<Vector2> destines)
        {
            VisualActivation();
            transporter.DestinesIntern = destines;
            transporter.active = true;
            transporter.SetPositions();
        }
        public void Toggle()
        {
            if (Time.time < time)
                return;
            time = Time.time + 0.1f;
            active = !active;
            //  Debug.Log("Estou no transporter toggle: " + gameObject.name+" "+active);

            if (active)
            {
                StartUpTransport(startPosition);
                transporter.going = true;

            }
            else
            {
                render.color = Color.grey;
                if (TryGetComponent<Rigidbody2D>(out var rb))
                {
                    rb.bodyType = RigidbodyType2D.Dynamic;
                    rb.gravityScale = 20;

                }
                if (TryGetComponent<Collider2D>(out var col))
                {
                    col.usedByEffector = false;
                }
            }

        }

        private void VisualActivation()
        {
            render.color = Color.white;

            if (TryGetComponent<Rigidbody2D>(out var rb))
            {
                rb.gravityScale = 0;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }
            if (TryGetComponent<Collider2D>(out var col))
            {
                col.usedByEffector = true;
            }
            transform.rotation = Quaternion.identity;
        }

    }
}
