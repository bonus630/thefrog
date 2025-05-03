using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-1)]
    public class EventsManager : MonoBehaviour
    {

        int currentEventIndex = 0;
        public List<GameEvent> events = new List<GameEvent>();
        [SerializeField] List<string> completedEvents = new List<string>();
        [SerializeField] AudioClip eventCompleteSound;
        AudioSource audioSource;
        public event Action<GameEvent> GameEventCompleted;
        void Awake()
        {
            audioSource = GetComponent<AudioSource>();
            GameEvent previewEvent = new GameEvent(GameEventName.NPCFirstTalk, true, false);
            events.Add(previewEvent);
            GameEvent killPig = new GameEvent(GameEventName.KillPig, true, false);
            events.Add(killPig);
            GameEvent firtEvent = new GameEvent(GameEventName.NPCTutorial, false, false);
            firtEvent.Requires = new List<GameEvent>()
        {
            previewEvent,

        };
            events.Add(firtEvent);

            GameEvent secondEvent = new GameEvent(GameEventName.Shuryken, false, false);
            secondEvent.Requires = new List<GameEvent>() { firtEvent };
            events.Add(secondEvent);
            GameEvent thirdEvent = new GameEvent(GameEventName.Gravity, false, false);
            thirdEvent.Requires = new List<GameEvent>() { secondEvent };
            events.Add(thirdEvent);
            GameEvent fourthEvent = new GameEvent(GameEventName.Teleport, false, false);
            fourthEvent.Requires = new List<GameEvent>() { thirdEvent };
            events.Add(fourthEvent);
            GameEvent heartContainer = new GameEvent(GameEventName.HeartContainer, false, false);
            heartContainer.Requires = new List<GameEvent>() { thirdEvent };
            events.Add(heartContainer);
            GameEvent scroll = new GameEvent(GameEventName.MysticScroll, false, false);
            scroll.Requires = new List<GameEvent>() { thirdEvent };
            events.Add(scroll);
            GameEvent duckPath = new GameEvent(GameEventName.DuckPath, false, false);
            duckPath.Requires = new List<GameEvent>() { secondEvent };
            events.Add(duckPath);
            GameEvent fireBall = new GameEvent(GameEventName.FireBall, false, false);
            //fireBall.Requires = new List<GameEvent>() { secondEvent };
            events.Add(fireBall);
            GameEvent appleTree = new GameEvent(GameEventName.AppleTreeFounded, false, false);
            //fireBall.Requires = new List<GameEvent>() { secondEvent };
            events.Add(appleTree);
            GameEvent featherTouch = new GameEvent(GameEventName.FeatherTouch, false, false);
            //fireBall.Requires = new List<GameEvent>() { secondEvent };
            events.Add(featherTouch);  
            GameEvent Dash = new GameEvent(GameEventName.Dash, false, false);
            //fireBall.Requires = new List<GameEvent>() { secondEvent };
            events.Add(Dash);

        }

        public GameEvent CurrentEvent()
        {
            return events[currentEventIndex];
        }

        public bool EventCompleted(GameEventName eventName, bool playSound)
        {
            if (eventName.Equals(GameEventName.None))
                return true;
            GameEvent eventGame = GetEvent(eventName);
            if (eventGame != null)
            {
                //for (int i = 0;i< eventGame.Requires.Count;i++)
                //{
                //    if (!eventGame.Requires[i].Completed)
                //        return false;
                //}
                if (!eventGame.Completed && playSound)
                    GetComponent<AudioSource>().PlayOneShot(eventCompleteSound);
                if (!eventGame.Completed)
                    GameEventCompleted?.Invoke(eventGame);
                eventGame.Completed = true;
                if (!completedEvents.Contains(eventName.ToString()))
                    completedEvents.Add(eventName.ToString());
                return true;
            }
            return false;
        }
        public GameEvent GetEvent(GameEventName eventName)
        {
            return events.FirstOrDefault(r => r.Name.Equals(eventName));
        }
        public void Reset()
        {
            for (int i = 0; i < events.Count; i++)
            {
                events[i].Completed = false;
            }
            completedEvents.Clear();
        }
    }
    public class GameEvent : IEquatable<GameEvent>
    {
        public GameEvent(GameEventName name, bool unlocked, bool completed)
        {
            Name = name;
            Unlocked = unlocked;
            Completed = completed;
        }

        public GameEventName Name { get; set; }
        public bool Unlocked { get; set; }
        public bool Completed { get; set; }
        public List<GameEvent> Requires { get; set; }
        public List<GameEvent> Unlockes { get; set; }
        public override string ToString()
        {
            return Name.ToString();
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as GameEvent);
        }
        public bool Equals(GameEvent other)
        {
            return other is not null &&
                   Name == other.Name;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
    public enum GameEventName
    {
        NPCFirstTalk,
        KillPig,
        NPCTutorial,
        Shuryken,
        Gravity,
        Teleport,
        HeartContainer,
        DuckPath,
        MysticScroll,
        FeatherTouch,
        FireBall,
        AppleTreeFounded,
        Dash,
        None
    }
}
