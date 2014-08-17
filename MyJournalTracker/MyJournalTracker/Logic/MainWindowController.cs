// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="MainWindowController.cs">
//   todo: license
// </copyright>
// <summary>
//   The main window controller.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Logic
{
    using System.Diagnostics;

    using Microsoft.Win32;

    using MyJournalTracker.EverNoteSupport;
    using MyJournalTracker.Model;
    using MyJournalTracker.Utility;

    /// <summary>
    /// The main window controller.
    /// </summary>
    public class MainWindowController
    {
        #region Public Methods and Operators

        /// <summary>
        /// Creates a new journal <see cref="Entry"/>
        /// </summary>
        /// <param name="entry">
        /// The new entry
        /// </param>
        public void CreateNewEntry(Entry entry)
        {
            SaveEntryInEvernote(entry);
            // SaveEntryInDropbox(entry);
        }

        /// <summary>
        /// The save entry in evernote.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        private static void SaveEntryInEvernote(Entry entry)
        {
            var token = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\AndreClaassen\MyJournalTracker", "developerKey", null);
            Debug.Assert(!string.IsNullOrEmpty(token), "please register your developer token firstt");

            var evernoteContentCreator = new EverNoteContentCreator(token);
            evernoteContentCreator.SaveJournalEntryToEvernote(entry);
        }

        /// <summary>
        /// The save entry in dropbox.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        private static void SaveEntryInDropbox(Entry entry)
        {
            var entrySaver = new DropboxSupport();
            entrySaver.Save(entry, GetDefaultJournalPath(DropboxSupport.JournalPathItem.JournalPathEntries));
            if (entry.HasPicture)
            {
                entrySaver.SavePicture(entry, GetDefaultJournalPath(DropboxSupport.JournalPathItem.JournalPathPhotos));
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets the default journal path.
        /// </summary>
        /// <param name="pathItem">
        /// The path item.
        /// </param>
        /// <returns>
        /// path to the journal app.
        /// </returns>
        private static string GetDefaultJournalPath(DropboxSupport.JournalPathItem pathItem)
        {
            var journalPath = Properties.Settings.Default.JournalPath;
            if (string.IsNullOrEmpty(journalPath))
            {
                journalPath = DropboxSupport.GetJournalDayOnePath(pathItem);
            }

            return journalPath;
        }

        #endregion
    }
}