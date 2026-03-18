using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public class Follow : MonoBehaviour
    {
        [field: SerializeField] public Transform Target { get; set; }
        [field: SerializeField] public Vector3 Offset { get; set; }

        void Start()
        {
           // Offset = transform.position - Target.position;
        }

        void LateUpdate()
        {
            if (Target == null)
                return;
            transform.position = Target.position + Offset;
        }
    }
}
