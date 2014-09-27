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
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.Win32;

    using MyJournalTracker.EverNoteSupport;
    using MyJournalTracker.Model;

    /// <summary>
    /// The main window controller.
    /// </summary>
    public class MainWindowController
    {
        /// <summary>
        /// The evernote content creator.
        /// </summary>
        private EverNoteContentCreator evernoteContentCreator = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowController"/> class.
        /// </summary>
        public MainWindowController()
        {
        }

        /// <summary>
        /// The evernote content creator.
        /// </summary>
        public EverNoteContentCreator EvernoteContentCreator
        {
            get
            {
                if (this.evernoteContentCreator == null)
                {
                    var token = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\AndreClaassen\MyJournalTracker", "developerKey", null);
                    Debug.Assert(!string.IsNullOrEmpty(token), "please register your developer token firstt");
                    this.evernoteContentCreator = new EverNoteContentCreator(token);
                }

                return this.evernoteContentCreator;
            }
        }

        public IEnumerable<string> RetrieveNotebookNames()
        {
            return this.EvernoteContentCreator.RetrieveNotebookNames();
        }

        /// <summary>
        /// Creates a new journal <see cref="Entry"/>
        /// </summary>
        /// <param name="entry">
        /// The new entry
        /// </param>
        public void CreateNewEntry(Entry entry)
        {
           this.SaveEntryInEvernote(entry);
           ///// SaveEntryInDropbox(entry);
        }

        /////// <summary>
        /////// Gets the default journal path.
        /////// </summary>
        /////// <param name="pathItem">
        /////// The path item.
        /////// </param>
        /////// <returns>
        /////// path to the journal app.
        /////// </returns>
        ////private static string GetDefaultJournalPath(DropboxSupport.JournalPathItem pathItem)
        ////{
        ////    var journalPath = Properties.Settings.Default.JournalPath;
        ////    if (string.IsNullOrEmpty(journalPath))
        ////    {
        ////        journalPath = DropboxSupport.GetJournalDayOnePath(pathItem);
        ////    }

        ////    return journalPath;
        ////}

        /// <summary>
        /// The save entry in evernote.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        private void SaveEntryInEvernote(Entry entry)
        {
            this.EvernoteContentCreator.SaveJournalEntryToEvernote(entry);
        }

        /////// <summary>
        /////// The save entry in dropbox.
        /////// </summary>
        /////// <param name="entry">
        /////// The entry.
        /////// </param>
        ////private void SaveEntryInDropbox(Entry entry)
        ////{
        ////    var entrySaver = new DropboxSupport();
        ////    entrySaver.Save(entry, GetDefaultJournalPath(DropboxSupport.JournalPathItem.JournalPathEntries));
        ////    if (entry.HasPicture)
        ////    {
        ////        entrySaver.SavePicture(entry, GetDefaultJournalPath(DropboxSupport.JournalPathItem.JournalPathPhotos));
        ////    }
        ////}
    }
}