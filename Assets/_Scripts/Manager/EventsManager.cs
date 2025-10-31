using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    [DefaultExecutionOrder(-100)]
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

            events.Add(new GameEvent(GameEventName.FireBall));
            events.Add(new GameEvent(GameEventName.PlayerCheckWall));
            
            events.Add(new GameEvent(GameEventName.AppleTreeFounded));
            events.Add(new GameEvent(GameEventName.FeatherTouch));
            events.Add(new GameEvent(GameEventName.KoarFounded));
            events.Add(new GameEvent(GameEventName.Dash));
            events.Add(new GameEvent(GameEventName.LightningBolt));
           
            events.Add(new GameEvent(GameEventName.DefeatWizard));
            events.Add(new GameEvent(GameEventName.RollingWind));
            events.Add(new GameEvent(GameEventName.PurifyWater));
            events.Add(new GameEvent(GameEventName.LadyLaments));
            events.Add(new GameEvent(GameEventName.PrisionerTip));
            events.Add(new GameEvent(GameEventName.MagicGlass));

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
           // Debug.Log("eventos: " + eventsDatas.Count);
            for (int i = 0; i < eventsDatas.Count; i++)
            {
               // if (!completedEvents.Contains(eventsDatas[i].ToString()))
               // {
                    //completedEvents.Add(eventsDatas[i].ToString());
                    GameEventName eventName = GameEventName.None;
                    if (Enum.TryParse(eventsDatas[i].ToString(), out eventName))
                    {
                        GameEvent eventGame = GetEvent(eventName);
                        if (eventGame != null)
                        {
                            EventCompleted(eventGame.Name, false);
                            //eventGame.Completed = true;
                           // Debug.Log("Evento carregado como verdadeiro: " + eventGame.Name);
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
