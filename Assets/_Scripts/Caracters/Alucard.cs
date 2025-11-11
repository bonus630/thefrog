using br.com.bonus630.thefrog.Effects;
using UnityEngine;

namespace br.com.bonus630.thefrog.Caracters
{
    public class Alucard : MonoBehaviour
    {
        [SerializeField] SpriteRenderer sprite;
        [SerializeField] int limit = 6;
        [SerializeField] float delayTime = 0.08f;
        [SerializeField] float lifeTime = 0.4f;
        [SerializeField] float fadeSpeed = 3f;
        ushort effectID;

        void Start()
        {
            SpriteAfterImageEffect spriteAfterImage = new SpriteAfterImageEffect(sprite,limit,delayTime,lifeTime,fadeSpeed);
            spriteAfterImage.Activate();
            effectID = EffectManager.instance.AddEffect(spriteAfterImage);
        }

        
    }
}
