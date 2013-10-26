// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="DropboxSupport.cs">
//   todo: license
// </copyright>
// <summary>
//   This class gives you some support functions for dropbox
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// This class gives you some support functions for dropbox
    /// </summary>
    public class DropboxSupport
    {
        #region Enums

        /// <summary>
        /// The journal path item.
        /// </summary>
        public enum JournalPathItem
        {
            /// <summary>
            /// The journal path root
            /// </summary>
            JournalPathRoot, 

            /// <summary>
            /// The journal path to the xml entry fields
            /// </summary>
            JournalPathEntries, 

            /// <summary>
            /// The journal path to the photo files
            /// </summary>
            JournalPathPhotos, 
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Auto detect Dropbox path to the dropbox folder
        /// </summary>
        /// <returns>
        /// pathname of the user specific dropbox path
        /// </returns>
        /// <exception cref="System.IO.FileNotFoundException">Couldn't locate the Dropbox folder or the Dropbox host.db file. Maybe, you haven't installed dropbox</exception>
        public static string GetDropboxPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dropboxPath = Path.Combine(appDataPath, "Dropbox\\host.db");

            if (!File.Exists(dropboxPath))
            {
                throw new FileNotFoundException(
                    "Couldn't locate the Dropbox folder or the Dropbox host.db file. Maybe, you haven't installed dropbox");
            }

            var lines = File.ReadAllLines(dropboxPath);
            var base64String = Convert.FromBase64String(lines[1]);
            var folderPath = Encoding.ASCII.GetString(base64String);

            return folderPath;
        }

        /// <summary>
        /// Auto detect the path of the Journal Day One Application Folder in Dropbox
        /// </summary>
        /// <param name="pathItem">
        /// The <see cref="JournalPathItem"/>
        /// </param>
        /// <returns>
        /// pathname of the journal day one application folder in Dropbox.
        /// </returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// Couldn't detect the Day One folder. Maybe, you haven't used the iOS Day One App before
        /// </exception>
        public static string GetJournalDayOnePath(JournalPathItem pathItem)
        {
            var dropboxPath = GetDropboxPath();
            var journalPath = Path.Combine(dropboxPath, "Apps\\Day One\\Journal.dayone");

            switch (pathItem)
            {
                case JournalPathItem.JournalPathRoot:
                    break;
                case JournalPathItem.JournalPathEntries:
                    journalPath = Path.Combine(journalPath, "entries");
                    break;
                case JournalPathItem.JournalPathPhotos:
                    journalPath = Path.Combine(journalPath, "photos");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("pathItem");
            }

            if (!Directory.Exists(journalPath))
            {
                throw new DirectoryNotFoundException(
                    "Couldn't detect the Day One folder. Maybe, you haven't used the iOS Day One App before");
            }

            return journalPath;
        }

        #endregion
    }
}