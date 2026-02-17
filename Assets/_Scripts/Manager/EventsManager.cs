using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-101)]
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
            InitEvents();
        }
        private void InitEvents()
        {
            foreach(GameEventName gameEvent in Enum.GetValues(typeof(GameEventName)))
            {
                if (gameEvent == GameEventName.None)
                    continue;
                events.Add(new GameEvent(gameEvent));
            }
        }
        public GameEvent CurrentEvent()
        {
            return events[currentEventIndex];
        }

        public bool EventCompleted(GameEventName eventName, bool playSound,bool firesEvent = true)
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
                if (!eventGame.Completed && firesEvent)
                {
                    GameEventCompleted?.Invoke(eventGame);
                }
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
        public bool AnyEventCompleted(GameEventName events)
        {
            if (events == GameEventName.None)
                return false;

            foreach (var ev in this.events)
            {
                if (ev.Completed && (events & ev.Name) != 0)
                    return true;
            }
            return false;
        }
        public bool AllEventsCompleted(GameEventName events)
        {
            if (events == GameEventName.None)
                return false;

            foreach (GameEventName flag in Enum.GetValues(typeof(GameEventName)))
            {
                if (flag == GameEventName.None)
                    continue;

                if (events.HasFlag(flag))
                {
                    var ev = GetEvent(flag);
                    if (ev == null || !ev.Completed)
                        return false;
                }
            }
            return true;
        }
        public void LoadEvents(Datas eventsDatas)
        {
           // Debug.Log("[EventsManager]eventos: " + eventsDatas.Count);
            for (int i = 0; i < eventsDatas.Count; i++)
            {
               // if (!completedEvents.Contains(eventsDatas[i].ToString()))
               // {
                    //completedEvents.Add(eventsDatas[i].ToString());
                    GameEventName eventName = GameEventName.None;
                    if (Enum.TryParse(eventsDatas[i].ToString(), out eventName))
                    {
                        GameEvent eventGame = GetEvent(eventName);
                        if (eventGame != null && !eventName.Equals(GameEventName.None))
                        {
                            EventCompleted(eventGame.Name, false,false);
                            //eventGame.Completed = true;
                           // Debug.Log("[EventsManager]Evento carregado como verdadeiro: " + eventGame.Name);
                        }
                    }
                //}
            }
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
        public GameEvent(GameEventName name)
        {
            Name = name;
            Unlocked = false;
            Completed = false;
        }
        public GameEventName Name { get; set; }
        public bool Unlocked { get; set; }
        public bool Completed { get; set; }
       // public List<GameEvent> Requires { get; set; }
       // public List<GameEvent> Unlockes { get; set; }
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
        public bool Equals(GameEventName other)
        {
            return Name == other;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name);
        }
    }
    [Flags]
    public enum GameEventName
    {
        NPCFirstTalk=       0x1,
        KillPig=            0x2,
        PlayerCheckWall=    0x4,
        NPCTutorial=        0x8,
        Shuryken=           0x10,
        Gravity=            0x20,
        Teleport=           0x40,
        HeartContainer=     0x80,
        DuckPath=           0x100,      
        MysticScroll=       0x200,
        FeatherTouch=       0x400,
        FireBall=           0x800,
        AppleTreeFounded=   0x1000,
        KoarFounded=        0x2000,
        Dash=               0x4000,
        DefeatWizard=       0x8000,
        LightningBolt=      0x10000,
        LadyLaments=        0x20000,
        RollingWind=        0x40000,
        PurifyWater=        0x80000,
        PrisionerTip=       0x100000,
        MagicGlass=         0x200000,
        None = 0
     
    }
}
