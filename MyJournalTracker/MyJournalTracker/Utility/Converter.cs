// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="Converter.cs">
//   todo: license
// </copyright>
// <summary>
//   The converter.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Helper
{
    using System;
    using System.Globalization;

    /// <summary>
    /// The converter.
    /// </summary>
    public class Converter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Converts the input date and time strings in a <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="date">
        /// The date in the european format 24.12.2013.
        /// </param>
        /// <param name="time">
        /// The time in the european format 18:44.
        /// </param>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public DateTime ConvertInputDateTime(string date, string time)
        {
            string s = date.Trim() + " " + time.Trim();
            DateTime dt = DateTime.ParseExact(s, @"dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None);
            return dt;
        }

        #endregion
    }
}