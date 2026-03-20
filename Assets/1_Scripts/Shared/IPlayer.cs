using System;
using UnityEngine;

namespace br.com.bonus630.thefrog.Shared
{
    //precisamos generalizar essa interface, desmembra-lá para melhor reaproveitamento
    public interface IPlayer
    {
        event Action<float> GravityChanged;
        event Action<bool> FastFall;

        int CurrentLife { get; set; }

        bool MoveInputOn { get; set; }
        bool InGround { get; set; }
        bool IsNormalGravity { get; }
        Vector3 Position { get; }

        void ReadDialogue();
        void CancelDialogue();
        void Alert();
        void Hit();
        void Hit(int damage);
        void UpdatePlayer();
        void FreezePlayer();
        //mover algumas coisas para uma interface de automação do player fora do script player
        //isso vai permitir melhor controle nas cutscenes
        void FallsControl();
        void ChangeNumberShurykens(int Shurykens);
        void AddForce(Vector2 force, ForceMode2D mode = ForceMode2D.Impulse, float time = 1f,bool removeInput = true);
        void ChangeGravity(float gravityDirection, float speed = 0.05f);
        void RemoveGravity(bool remove);
        void KnockUpOnJump(Vector2 repulse);
        void AllInputsOn(bool inputOn, float delayTime = 0, bool autoSwitch = false, float switchTime = 0);
        void AddAction(SchedulerData action);
        void AddAction(Action action, float time);
        bool FooterTouching(Collider2D coll);
        bool BodyTouching(Collider2D coll);
        bool BodyTouching(LayerMask layers);
        string GetFormattedInputName(string actionName);

    }
}
