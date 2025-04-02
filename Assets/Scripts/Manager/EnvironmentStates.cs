using System;


namespace br.com.bonus630.thefrog.Manager
{
    [Serializable]
    public class EnvironmentStates
    {
        public PlayerStates playerStates;
        public int NPCVirtualGuyApples;
        public int NPCVirtualGuyDialogue;
        public int NPC_WallJump_Tutorial;

        public EnvironmentStates()
        {

        }
        public EnvironmentStates(PlayerStates _playerStates)
        {
            this.playerStates = _playerStates;
        }

    }
}

