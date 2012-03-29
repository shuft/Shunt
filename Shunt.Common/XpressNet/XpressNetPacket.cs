// -----------------------------------------------------------------------
// <copyright file="XpressNetMessage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Shunt.Common.XpressNet
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class XpressNetPacket
    {
        public XpressNetPacket(List<byte> bytes)
        {
            this.Bytes = bytes;
        }

        public List<byte> Bytes { get; set; }

        public byte HeaderByte
        {
            get
            {
                return this.Bytes[0];
            }
        }
    }
}