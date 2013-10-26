// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="Logger.cs">
//   todo: license
// </copyright>
// <summary>
//   A utility class used for error logging.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.IO;

    /// <summary>
    /// A utility class used for error logging.
    /// </summary>
    internal class Logger
    {
        #region Constants

        /// <summary>
        /// The log file name.
        /// </summary>
        private const string LogFile = "error.log";

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Logs the specified log message.
        /// </summary>
        /// <param name="logMessage">
        /// The log message.
        /// </param>
        public static void Log(string logMessage)
        {
            using (StreamWriter w = File.AppendText(LogFile))
            {
                w.Write("\r\nLog Entry : ");
                w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(), DateTime.Now.ToLongDateString());
                w.WriteLine("  :");
                w.WriteLine("  :{0}", logMessage);
                w.WriteLine("-------------------------------");
            }
        }

        #endregion
    }
}