using System;
namespace br.com.bonus630.thefrog.Manager
{
    [Serializable]
    public class EnvironmentStates
    {
        public int index;
        public int run = 1;
        public PlayerStates playerStates;
        public int NPCVirtualGuyApples;
        public int NPCVirtualGuyDialogue;
        public int NPC_WallJump_Tutorial;
        public float GameTimeInSeconds;
        public Datas Activeds;
        public EnvironmentStates()
        {

        }
        public EnvironmentStates(PlayerStates _playerStates)
        {
            this.playerStates = _playerStates;
            Activeds = new Datas();
        }
        public static EnvironmentStates Reset()
        {
            var env = new EnvironmentStates();
            env.playerStates = new PlayerStates();
            return env;
        }
    }
}

