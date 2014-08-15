// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteHelperFunction.cs" company="">
//   
// </copyright>
// <summary>
//   The evernote helper function.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Storage
{
    using System;

    /// <summary>
    ///     The evernote helper function.
    /// </summary>
    public static class EvernoteHelperFunction
    {
        #region Public Methods and Operators

        /// <summary>
        /// The to evernote time stamp.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long ToEvernoteTimeStamp(this DateTime dateTime)
        {
            var unixDate = new DateTime(1970, 1, 1);
            TimeSpan span = dateTime - unixDate;
            return span.Milliseconds;
        }

        #endregion
    }
}