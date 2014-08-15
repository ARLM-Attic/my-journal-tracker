// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeCapsule.cs" company="">
//   
// </copyright>
// <summary>
//   The time capsule.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Helper
{
    using System;
    using System.Globalization;

    /// <summary>
    ///     The time capsule.
    /// </summary>
    public static class TimeCapsule
    {
        /// <summary>
        /// The testable time.
        /// </summary>
        private static DateTime? testableTime;

        /// <summary>
        /// The testable culture info.
        /// </summary>
        private static CultureInfo testableCultureInfo;

        /// <summary>
        /// Sets a testable time for unit testing
        /// </summary>
        public static DateTime TestableTime
        {
            set
            {
                testableTime = value;
            }
        }

        /// <summary>
        /// Gets or sets a testable culture for unit testing
        /// </summary>
        public static CultureInfo TestableCultureInfo
        {
            get
            {
                return testableCultureInfo ?? CultureInfo.CurrentUICulture;
            }

            set
            {
                testableCultureInfo = value;
            }
        }

        /// <summary>
        /// Get the current Time and Date. 
        /// </summary>
        /// <returns>
        /// The <see cref="DateTime"/>.
        /// </returns>
        public static DateTime GetCurrentTime()
        {
            return testableTime != null ? testableTime.Value : DateTime.Now;
        }
    }
}