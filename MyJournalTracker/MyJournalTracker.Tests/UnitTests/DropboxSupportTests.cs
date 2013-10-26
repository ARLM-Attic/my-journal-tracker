// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="DropboxSupportTests.cs">
//   todo: license
// </copyright>
// <summary>
//   The Dropbox support tests.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Tests.UnitTests
{
    using System.IO;

    using MyJournalTracker.Utility;

    using NUnit.Framework;

    /// <summary>
    /// The Dropbox support tests.
    /// </summary>
    /// <remarks>
    /// Currently, the unit tests are only working, if you have dropbox installed and a valid dropbox path for the DayOne application
    /// <example>
    /// C:/Users/username/Dropbox/Apps/Day One/Journal.dayone/entries
    /// </example>
    /// </remarks>
    [TestFixture]
    public class DropboxSupportTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// test for the function <see cref="DropboxSupport.GetDropboxPath"/>.
        /// </summary>
        [Test]
        public void GetDropboxPathTest()
        {
            var dropboxPath = DropboxSupport.GetDropboxPath();
            Assert.IsTrue(Directory.Exists(dropboxPath));
        }

        /// <summary>
        /// test for the function <see cref="DropboxSupport.GetJournalDayOnePath"/>.
        /// </summary>
        [Test]
        public void GetJournalPathTest()
        {
            var journalDayOnePath = DropboxSupport.GetJournalDayOnePath(DropboxSupport.JournalPathItem.JournalPathRoot);
            Assert.IsTrue(Directory.Exists(journalDayOnePath));
        }
        
        #endregion
    }
}