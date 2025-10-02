using System;
using System.Collections.Generic;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerStates PlayerStates { get; private set; }

        private Dictionary<GameEventName, Action> eventActions = new();

        private void Start()
        {
            eventActions.Add(GameEventName.Gravity, () => { PlayerStates.HasGravity = true; });
            eventActions.Add(GameEventName.Dash, () => { PlayerStates.HasDash = true; });
            eventActions.Add(GameEventName.FireBall, () => { PlayerStates.HasFireball = true; });
            eventActions.Add(GameEventName.LightningBolt, () => { PlayerStates.HasLightning = true; });
            eventActions.Add(GameEventName.RollingWind, () => { PlayerStates.HasWind = true; });
            eventActions.Add(GameEventName.NPCTutorial, () => { PlayerStates.HasWallJump = true; });
            eventActions.Add(GameEventName.FeatherTouch, () => { PlayerStates.FallsControl = true; });
            GameManager.Instance.eventManager.GameEventCompleted += OnGameEventCompleted;
            GameManager.Instance.GameStatesRestaured += OnGameStatesRestaured;
        }

        private void OnGameStatesRestaured()
        {
            this.PlayerStates = GameManager.Instance.PlayerStates;
        }

        private void OnGameEventCompleted(GameEvent obj)
        {
            if (eventActions.TryGetValue(obj.Name, out Action action))
                action?.Invoke();
        }
        public void UpdatePlayer(Action callback)
        {
            callback.Invoke();
            this.PlayerStates.Speed += 0.1f;
            this.PlayerStates.JumpForce += 0.1f;
        }
        public void UpdateShurykens(int shurykens)
        {
            this.PlayerStates.Shurykens += shurykens;
            GameManager.Instance.UpdateShurykens();
        }
    }
}
