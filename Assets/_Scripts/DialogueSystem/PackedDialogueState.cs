using UnityEngine;

namespace br.com.bonus630.thefrog.DialogueSystem
{

    public struct PackedDialogState
    {
        private int value;

        public PackedDialogState(int value)
        {
            this.value = value;
        }

        public int RawValue => value;

        public int DialogIndex
        {
            get => value & 0xFFFF;
            set
            {
                this.value &= ~0xFFFF;
                this.value |= (value & 0xFFFF);
            }
        }

        public bool IsRead(int index)
        {
            int flags = (value >> 16) & 0xFFFF;
            return (flags & (1 << index)) != 0;
        }

        public void SetRead(int index)
        {
            int flags = (value >> 16) & 0xFFFF;
            flags |= (1 << index);

            value &= 0xFFFF;
            value |= (flags << 16);
        }
    }
    public struct NpcDialogState
    {
        public int CurrentDialog;
        public int ReadFlags; // bitmask
    }
    public static class DialogStateUtils
    {
        const int DialogIndexMask = 0x0000FFFF;
        const uint FlagsMask = 0xFFFF0000;

        public static int Encode(NpcDialogState state)
        {
            int value = 0;

            value |= (state.CurrentDialog & 0xFFFF);
            value |= (state.ReadFlags & 0xFFFF) << 16;

            return value;
        }
        public static NpcDialogState Decode(int value)
        {
            NpcDialogState state;

            state.CurrentDialog = value & 0xFFFF;
            state.ReadFlags = (value >> 16) & 0xFFFF;

            return state;
        }
        public static int GetDialogIndex(int value)
        {
            return value & DialogIndexMask;
        }
        public static int SetDialogIndex(int value, int dialogIndex)
        {
            value &= ~DialogIndexMask;          // limpa parte baixa
            value |= (dialogIndex & 0xFFFF);    // escreve novo índice
            return value;
        }
        public static bool IsDialogRead(int value, int dialogIndex)
        {
            int flags = (value >> 16) & 0xFFFF;
            return (flags & (1 << dialogIndex)) != 0;
        }
        public static int SetDialogRead(int value, int dialogIndex)
        {
            int flags = (value >> 16) & 0xFFFF;
            flags |= (1 << dialogIndex);

            value &= DialogIndexMask;        // preserva dialogIndex
            value |= (flags << 16);          // escreve flags

            return value;
        }

        public static int ClearDialogRead(int value, int dialogIndex)
        {
            int flags = (value >> 16) & 0xFFFF;

            flags &= ~(1 << dialogIndex); // limpa o bit

            value &= DialogIndexMask;     // preserva CurrentDialog
            value |= (flags << 16);       // reescreve flags

            return value;
        }

    }

}
