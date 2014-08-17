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
    using System.Windows;

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