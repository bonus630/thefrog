using System.Runtime.CompilerServices;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class PlayerPointsEntry : MonoBehaviour
    {
        [SerializeField] ScreenEffects screenEffects;
        [SerializeField] GameObject[] points;
        public Vector3 this[int index] { get => points[index].transform.position; }
        private void Awake()
        {
            screenEffects.screenFader.fadeImage.color = Color.black;
        }
        void Start()
        {
            screenEffects.FadeIn();
        }
    }
}
