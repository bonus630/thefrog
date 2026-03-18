using System;
using System.Collections.Generic;
using br.com.bonus630.thefrog.Shared;
using UnityEngine;

namespace br.com.bonus630.thefrog.Manager
{
    public class PlayerManager : MonoBehaviour, IService
    {
        public PlayerStates PlayerStates { get; private set; }

        private readonly Dictionary<GameEventName, Action> eventActions = new();

        private Action GameEventChanged = null;
  
        public event Action UpdatePlayerEvent;

        private void Awake()
        {
            eventActions.Add(GameEventName.Gravity,         () => { PlayerStates.HasGravity = true; });
            eventActions.Add(GameEventName.Dash,            () => { PlayerStates.HasDash = true; });
            eventActions.Add(GameEventName.FireBall,        () => { PlayerStates.HasFireball = true; });
            eventActions.Add(GameEventName.LightningBolt,   () => { PlayerStates.HasLightning = true; });
            eventActions.Add(GameEventName.RollingWind,     () => { PlayerStates.HasWind = true; });
            eventActions.Add(GameEventName.NPCTutorial,     () => { PlayerStates.HasWallJump = true; });
            eventActions.Add(GameEventName.FeatherTouch,    () => { PlayerStates.FallsControl = true; });
            eventActions.Add(GameEventName.MagicGlass,      () => { PlayerStates.HasVision = true; });
            this.PlayerStates = RecoverStates();
            GameManager.Instance.eventManager.GameEventCompleted += OnGameEventCompleted;
            GameManager.Instance.GameStatesRestaured += OnGameStatesRestaured;
            ServiceLocator.Instance.GetAsync<IHourProvider>(HourProviderCallBack);
        }
        //vamos deixar as chamadas de gamemanager aqui por enquanto, depois vamos centralizar tudo em um event bus
        public void UpdatePlayer()
        {
            this.PlayerStates.Speed += 0.1f;
            this.PlayerStates.JumpForce += 0.1f;
            UpdatePlayerEvent?.Invoke();
        }
        
        public void UpdateShurykens(int shurykens)
        {
            this.PlayerStates.Shurykens += shurykens;
            GameManager.Instance.UpdateShurykens();
        }
        public void UpdateHeart(int hearts) => GameManager.Instance.UpdateHeart(hearts);
        
        public void PlayerDie() => GameManager.Instance.GameOver();
        public void ActiveSkill(bool active)=> GameManager.Instance.ActiveSkill(active);
        public void GameEventChange(Action callback) => this.GameEventChanged = callback;
        private void OnGameEventChanged() => this.GameEventChanged?.Invoke();
        private void HourProviderCallBack(IHourProvider hourProvider)
        {
            PlayerStates.Hour = hourProvider.Hour;
            //Debug.Log("[PlayerManager][HourProviderCallBack] hour: " + this.PlayerStates.Hour);
            hourProvider.OnHourChanged += (hour) => 
            {
                PlayerStates.Hour = hour;
                //Debug.Log("[PlayerManager][HourProviderCallBack] event hour: " + this.PlayerStates.Hour);
            };
        }
        private void OnGameStatesRestaured()
        {
            this.PlayerStates = GameManager.Instance.PlayerStates;
            //Debug.Log("[PlayerManager][OnGameStatesRestaured] hour: " + this.PlayerStates.Hour);
        }

        private void OnGameEventCompleted(GameEvent obj)
        {
            // Debug.Log("[PlayerManager] GameEvent:" + obj.Name);
            if (eventActions.TryGetValue(obj.Name, out Action action))
            {
                action?.Invoke();
                OnGameEventChanged();
            }
        }
        private void OnDisable()
        {
            GameManager.Instance.eventManager.GameEventCompleted -= OnGameEventCompleted;
            GameManager.Instance.GameStatesRestaured -= OnGameStatesRestaured;
        }
        private PlayerStates RecoverStates()
        {
            if(GameManager.Instance == null || GameManager.Instance.PlayerStates == null)
                    return new PlayerStates();
            return GameManager.Instance.PlayerStates;
        }
    }
}
