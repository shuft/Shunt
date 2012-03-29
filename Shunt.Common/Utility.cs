// -----------------------------------------------------------------------
// <copyright file="Utility.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Shunt.Common
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Utility
    {
        public static bool IsBitSet(byte value, int bitindex)
        {
            return (value & (1 << bitindex)) != 0;
        }
    }
}