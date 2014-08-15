// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteAccessException.cs" company="">
//   
// </copyright>
// <summary>
//   The evernote access exception.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.EverNoteSupport
{
    using System;

    /// <summary>
    ///     The evernote access exception.
    /// </summary>
    public class EvernoteAccessException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EvernoteAccessException"/> class.
        /// </summary>
        /// <param name="exceptionText">
        /// The exception text.
        /// </param>
        public EvernoteAccessException(string exceptionText) : base(exceptionText)
        {
        }
    }
}
