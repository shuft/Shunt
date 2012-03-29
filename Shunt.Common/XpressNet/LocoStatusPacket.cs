// -----------------------------------------------------------------------
// <copyright file="LocoStatus.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Shunt.Common.XpressNet
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class LocoStatusPacket : XpressNetPacket
    {
        public LocoStatusPacket(List<byte> bytes)
            : base(bytes)
        {
            byte idByte = this.Bytes[1];

            this.Free = !Utility.IsBitSet(idByte, 3);
            this.SpeedStep = this.DetermineSpeedStep(idByte);

            byte speedByte = this.Bytes[2];

            this.Direction = this.DetermineDirection(speedByte);
            this.Speed = this.DetermineSpeed(speedByte);

            byte functionByteA = this.Bytes[3];
            byte functionByteB = this.Bytes[4];

            this.StatusF0 = Utility.IsBitSet(functionByteA, 4);
            this.StatusF1 = Utility.IsBitSet(functionByteA, 0);
            this.StatusF2 = Utility.IsBitSet(functionByteA, 1);
            this.StatusF3 = Utility.IsBitSet(functionByteA, 2);
            this.StatusF4 = Utility.IsBitSet(functionByteA, 3);

            this.StatusF5 = Utility.IsBitSet(functionByteB, 0);
            this.StatusF6 = Utility.IsBitSet(functionByteB, 1);
            this.StatusF7 = Utility.IsBitSet(functionByteB, 2);
            this.StatusF8 = Utility.IsBitSet(functionByteB, 3);
            this.StatusF9 = Utility.IsBitSet(functionByteB, 4);
            this.StatusF10 = Utility.IsBitSet(functionByteB, 5);
            this.StatusF11 = Utility.IsBitSet(functionByteB, 6);
            this.StatusF12 = Utility.IsBitSet(functionByteB, 7);
        }

        public int Speed { get; set; }

        public int Address { get; set; }

        public LocoDirection Direction { get; set; }

        public bool Free { get; set; }

        public SpeedStep SpeedStep { get; set; }

        public bool StatusF0 { get; set; }

        public bool StatusF1 { get; set; }

        public bool StatusF2 { get; set; }

        public bool StatusF3 { get; set; }

        public bool StatusF4 { get; set; }

        public bool StatusF5 { get; set; }

        public bool StatusF6 { get; set; }

        public bool StatusF7 { get; set; }

        public bool StatusF8 { get; set; }

        public bool StatusF9 { get; set; }

        public bool StatusF10 { get; set; }

        public bool StatusF11 { get; set; }

        public bool StatusF12 { get; set; }

        private SpeedStep DetermineSpeedStep(byte idByte)
        {
            if (Utility.IsBitSet(idByte, 2))
            {
                return SpeedStep.OneHundredAndTwentyEight;
            }
            else
            {
                if (Utility.IsBitSet(idByte, 1))
                {
                    return SpeedStep.TwentyEight;
                }
                else
                {
                    if (Utility.IsBitSet(idByte, 0))
                    {
                        return SpeedStep.TwentySeven;
                    }
                    else
                    {
                        return SpeedStep.Fourteen;
                    }
                }
            }
        }

        private LocoDirection DetermineDirection(byte speedByte)
        {
            if (Utility.IsBitSet(speedByte, 7))
            {
                return LocoDirection.Forward;
            }
            else
            {
                return LocoDirection.Reverse;
            }
        }

        private int DetermineSpeed(byte speedByte)
        {
            bool[] bits = new bool[] { false, false, false, false,
                                        false, false, false, false};

            switch (this.SpeedStep)
            {
                case SpeedStep.Fourteen:
                    bits[0] = Utility.IsBitSet(speedByte, 0);
                    bits[1] = Utility.IsBitSet(speedByte, 1);
                    bits[2] = Utility.IsBitSet(speedByte, 2);
                    bits[3] = Utility.IsBitSet(speedByte, 3);
                    break;
                case SpeedStep.TwentySeven:
                    bits[0] = Utility.IsBitSet(speedByte, 0);
                    bits[1] = Utility.IsBitSet(speedByte, 1);
                    bits[2] = Utility.IsBitSet(speedByte, 2);
                    bits[3] = Utility.IsBitSet(speedByte, 3);
                    bits[4] = Utility.IsBitSet(speedByte, 4);
                    break;
                case SpeedStep.TwentyEight:
                    bits[0] = Utility.IsBitSet(speedByte, 0);
                    bits[1] = Utility.IsBitSet(speedByte, 1);
                    bits[2] = Utility.IsBitSet(speedByte, 2);
                    bits[3] = Utility.IsBitSet(speedByte, 3);
                    bits[4] = Utility.IsBitSet(speedByte, 4);
                    break;
                case SpeedStep.OneHundredAndTwentyEight:
                    bits[0] = Utility.IsBitSet(speedByte, 0);
                    bits[1] = Utility.IsBitSet(speedByte, 1);
                    bits[2] = Utility.IsBitSet(speedByte, 2);
                    bits[3] = Utility.IsBitSet(speedByte, 3);
                    bits[4] = Utility.IsBitSet(speedByte, 4);
                    bits[5] = Utility.IsBitSet(speedByte, 5);
                    bits[6] = Utility.IsBitSet(speedByte, 6);
                    break;
            }

            byte temp = new byte();

            int bitIndex = 0;

            for (int i = 0; i < bits.Length; i++)
            {
                if (bits[i])
                {
                    temp |= (byte)(((byte)1) << bitIndex);
                }

                bitIndex++;
            }

            return (int)temp;
        }
    }
}