using UnityEngine;
using UnityEngine.Rendering;

namespace br.com.bonus630.thefrog
{
    public class textureDecoder : MonoBehaviour
    {
        [SerializeField] Camera cam;
        [SerializeField] RenderTexture minimapRT;
        [SerializeField] UnityEngine.UI.RawImage minimapImage;

        void Start()
        {
            minimapImage.texture = tex;
        }
        Texture2D tex;

        void Awake()
        {
            tex = new Texture2D(minimapRT.width, minimapRT.height, TextureFormat.ARGB32, false);
        }

        public void Capture()
        {
            cam.Render();
            RenderTexture.active = cam.targetTexture;
           
            tex.ReadPixels(new Rect(0, 0, minimapRT.width, minimapRT.height), 0, 0);
            tex.Apply();
            RenderTexture.active = null;
        }
        private void Update()
        {
            Capture();
        }
    }
}
