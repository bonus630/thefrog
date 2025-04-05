using System.Collections.Generic;
using UnityEngine;
namespace br.com.bonus630.thefrog.Manager
{
    [System.Serializable]
    public class PlayerStates
    {
        public PlayerStates()
        {

        }
        public PlayerStates(PlayerPosition playerPosition, Datas CollectablesID)
        {
            this.PlayerPosition = playerPosition;
            this.CollectablesID = CollectablesID;
        }
        public PlayerStates(PlayerPosition playerPosition, Datas CollectablesID, Datas CompletedGameEvents, Datas ChestsID)
        {
            this.PlayerPosition = playerPosition;
            this.CollectablesID = CollectablesID;
            this.ChestsID = ChestsID;
            this.CompletedGameEvents = CompletedGameEvents;
        }
        public PlayerPosition PlayerPosition;
        public Datas CollectablesID;
        public Datas ChestsID;
        public bool HasDoubleJump;
        public bool HasWallJump;
        public bool HasDash;
        public bool HasGravity;
        public bool HasFireball;
        public bool FallsControl;
        //For Debug
        public int Shurykens;
        //Hearts é o maxlife
        public int Hearts = 2;
        public int Hour = 6;
        public int Collectables;

        public float Speed = 4.3f;
        public float JumpForce = 12.3f;

        public Datas CompletedGameEvents;
    }
    [System.Serializable]
    public class Datas
    {
        public List<string> datas = new List<string>();
        public Datas()
        {

        }
        public void Add(string data)
        {
            if (!this.datas.Contains(data))
                this.datas.Add(data);
        }
        public int Count => this.datas.Count;
        public bool Contains(string data) => this.datas.Contains(data);
        public string this[int index] { get => this.datas[index]; set => this.datas[index] = value; }

    }
    [System.Serializable]
    public class PlayerPosition
    {
        public int CheckPointID = -1;
        public Vector2 Position;

        public PlayerPosition() { }
        public PlayerPosition(Vector2 position)
        {
            Position = position;
        }
    }
}
