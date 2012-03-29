// -----------------------------------------------------------------------
// <copyright file="EventArgs_T_.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Shunt.Common
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public sealed class EventArgs<T> : EventArgs
    {
        public EventArgs(T data)
        {
            this.Data = data;
        }

        public T Data
        {
            get;
            set;
        }
    }
}