using System;
using System.IO;
using UnityEngine;

namespace br.com.bonus630.thefrog.Utils
{
    public class ThumbGenerator 
    {
        public string CreateEncodeThumb(Camera cam,GameObject target, float factor)
        {
            cam.gameObject.transform.position = new Vector3(target.transform.position.x,target.transform.position.y,cam.gameObject.transform.position.z);
           // return "";
            int width = (int)(Screen.width * factor);
            int height = (int)(Screen.height * factor);
            //RenderTexture rt = RenderTexture.GetTemporary(width, height,0,RenderTextureFormat.ARGB32);
           /// RenderTexture.active = rt;
            //cam.targetTexture = rt;
            cam.Render();
            RenderTexture.active = cam.targetTexture;
            Texture2D texture = new Texture2D(width,height, TextureFormat.ARGB32, false);
            texture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            texture.Apply();
            byte[] buffer = texture.EncodeToPNG();
            //cam.targetTexture.Release();
            //RenderTexture.ReleaseTemporary(rt);
            RenderTexture.active = null;
            
            string base64 = Convert.ToBase64String(buffer);

            return base64;
        }
        public Sprite DecodeThumb(string base64Image)
        {
            byte[] buffer = Convert.FromBase64String(base64Image);
            Texture2D texture = new Texture2D(102, 72);
            texture.LoadImage(buffer);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            return sprite;
        }
    }
}
