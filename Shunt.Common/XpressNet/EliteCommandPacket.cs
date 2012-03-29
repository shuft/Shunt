// -----------------------------------------------------------------------
// <copyright file="EliteCommandPacket.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Shunt.Common.XpressNet
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class EliteCommandPacket : XpressNetPacket
    {
        public EliteCommandPacket(List<byte> bytes)
            : base(bytes)
        {
        }

        public int LocoAddress
        {
            get
            {
                return (int)this.Bytes[3];
            }
        }
    }
}