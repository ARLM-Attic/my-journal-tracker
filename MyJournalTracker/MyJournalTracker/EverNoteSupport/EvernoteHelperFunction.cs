// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteHelperFunction.cs" company="">
//   
// </copyright>
// <summary>
//   The evernote helper function.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.EverNoteSupport
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;

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
            var span = dateTime - unixDate;
            return span.Milliseconds;
        }

        /// <summary>
        /// The read template.
        /// </summary>
        /// <param name="templateName">
        /// The template name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReadTemplate(string templateName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "MyJournalTracker.EverNoteSupport.Templates." + templateName + ".xml";
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Debug.Assert(stream != null, "resource " + templateName + "not found!");
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }


        #endregion
    }
}