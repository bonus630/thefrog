using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    public interface IPlayer
    {
        int CurrentLife { get; set; }
        float Speed { get; set; }
        float JumpForce { get; set; }
        bool InputsOn { get; set; }
        bool InGround { get; set; }
        void ReadDialogue();
        void Alert();
        void Hit();
        void ChangeNumberShurykens(int Shurykens);
        void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f);
        void ChangeGravity(float gravityDirection, float speed = 0.05f);
        void KnockUp(Vector2 repulse);
        bool FooterTouching(Collider2D coll);

    }
}
