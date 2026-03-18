using br.com.bonus630.thefrog.Effects;
using br.com.bonus630.thefrog.Shared;
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
        SpriteAfterImageEffect spriteAfterImage;
        void Start()
        {
            spriteAfterImage = SpriteAfterImageEffect.Create(sprite)
                                                                            .WithLimit(limit)
                                                                            .WithSpawnInterval(delayTime)
                                                                            .WithLifeTime(lifeTime)
                                                                            .WithFadeSpeed(fadeSpeed);
            spriteAfterImage.Activate();
            effectID = EffectManager.instance.AddEffect(spriteAfterImage);


        }
        private void OnDisable()
        {
            spriteAfterImage?.Deactivate();
        }

    }
}
