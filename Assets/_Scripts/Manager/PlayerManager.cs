using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class PlayerManager : MonoBehaviour
    {
        public PlayerStates PlayerStates { get; private set; }

        private Dictionary<GameEventName, Action> eventActions = new();

        private void Awake()
        {
            eventActions.Add(GameEventName.Gravity, () => { PlayerStates.HasGravity = true; });
            eventActions.Add(GameEventName.Dash, () => { PlayerStates.HasDash = true; });
            eventActions.Add(GameEventName.FireBall, () => { PlayerStates.HasFireball = true; });
            eventActions.Add(GameEventName.LightningBolt, () => { PlayerStates.HasLightning = true; });
            eventActions.Add(GameEventName.RollingWind, () => { PlayerStates.HasWind = true; });
            eventActions.Add(GameEventName.NPCTutorial, () => { PlayerStates.HasWallJump = true; });
            eventActions.Add(GameEventName.FeatherTouch, () => { PlayerStates.FallsControl = true; });
            eventActions.Add(GameEventName.MagicGlass, () => { PlayerStates.HasVision = true; });
            this.PlayerStates = GameManager.Instance.PlayerStates;
            GameManager.Instance.eventManager.GameEventCompleted += OnGameEventCompleted;
            GameManager.Instance.GameStatesRestaured += OnGameStatesRestaured;
            ServiceLocator.Instance.GetAsync<IHourProvider>(HourProviderCallBack);
        }

        private void OnGameStatesRestaured()
        {
           // Debug.Log("[PlayerManager] ongamerestaured:");
            this.PlayerStates = GameManager.Instance.PlayerStates;
        }

        private void OnGameEventCompleted(GameEvent obj)
        {
            Debug.Log("[PlayerManager] GameEvent:" + obj.Name);
            if (eventActions.TryGetValue(obj.Name, out Action action))
                action?.Invoke();
        }
        public void UpdatePlayer()
        {
            this.PlayerStates.Speed += 0.1f;
            this.PlayerStates.JumpForce += 0.1f;
        }
        public void UpdateShurykens(int shurykens)
        {
            this.PlayerStates.Shurykens += shurykens;
            GameManager.Instance.UpdateShurykens();
        }
        private void HourProviderCallBack(IHourProvider hourProvider)
        {
            PlayerStates.Hour = hourProvider.Hour;
            hourProvider.OnHourChanged += (hour) => { PlayerStates.Hour = hour; };
        }
    }
}
