using System;
using System.IO;
using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public class ThumbGenerator
    {
        private readonly int width;
        private readonly int height;

        public ThumbGenerator(float factor)
        {
            width = Mathf.FloorToInt(Screen.width * factor);
            height = Mathf.FloorToInt(Screen.height * factor);
        }
        public string CreateEncodeThumb(Camera cam, GameObject target)
        {
            cam.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, cam.gameObject.transform.position.z);
            cam.Render();
            RenderTexture.active = cam.targetTexture;
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            byte[] buffer = texture.EncodeToPNG();
            RenderTexture.active = null;
            string base64 = Convert.ToBase64String(buffer);

            return base64;
        }
        public byte[] CreatePNGTexture(Camera cam, GameObject target)
        {
            cam.gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, cam.gameObject.transform.position.z);
            cam.Render();
            RenderTexture.active = cam.targetTexture;
            Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            byte[] buffer = texture.EncodeToPNG();
            RenderTexture.active = null;
            return buffer;
        }

        public Sprite DecodeThumb(string base64Image)
        {
            byte[] buffer = Convert.FromBase64String(base64Image);
            Texture2D texture = new Texture2D(width, height);
            texture.LoadImage(buffer);
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}
