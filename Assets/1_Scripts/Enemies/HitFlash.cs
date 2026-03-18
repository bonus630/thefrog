using UnityEngine;

namespace br.com.bonus630.thefrog.Enemies
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class HitFlash : MonoBehaviour
    {
        Material mat;
        Color color = Color.white;
        
        void Start()
        {
            mat = GetComponent<SpriteRenderer>().material;
        }
        public void EnableHit()
        {
            mat.SetColor("_FlashColor", color);
            mat.SetInt("_FlashAmount", 1);
        }
        public void DisableHit()
        {
            mat.SetColor("_FlashColor", Color.white);
            mat.SetInt("_FlashAmount", 0);
        }
        public void SetColorHit(Color color) => this.color = color;
       
    }
}
