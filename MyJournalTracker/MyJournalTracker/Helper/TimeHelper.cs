// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The time helper.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Helper
{
    using System.Globalization;

    /// <summary>
    /// The time helper retrieves various date and time formats for fully compatibily with vJournal ánd YourDay
    /// </summary>
    public class TimeHelper
    {
        /// <summary>
        /// Gets the current year in 4-digit format
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetCurrentYear()
        {
            return TimeCapsule.GetCurrentTime().Year.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// The current date in locale format compatible with vJournal and YourDay
        /// </summary>
        /// <returns>
        /// The formated date as long date
        /// </returns>
        public string GetCurrentDateInLocaleFormat()
        {
            var date = TimeCapsule.GetCurrentTime();

            // try to extract the day name of the .NET long format.
            // :hack:
            var fullFormat = date.ToString("D", TimeCapsule.TestableCultureInfo.DateTimeFormat);
            var i = fullFormat.IndexOf(',');
            return i >= 0 ? fullFormat.Substring(i + 2) : fullFormat;
        }

        /// <summary>
        /// The get current time in locale format.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetCurrentTimeInLocaleFormat()
        {
            var formattedTime = TimeCapsule.GetCurrentTime().ToShortTimeString();
            return formattedTime;
        }
    }
}