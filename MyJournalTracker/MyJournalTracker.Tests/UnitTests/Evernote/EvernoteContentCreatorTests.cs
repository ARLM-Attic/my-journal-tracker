// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteContentCreatorTests.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests.Evernote
{
    using System;

    using MyJournalTracker.Helper;
    using MyJournalTracker.Storage;

    using NUnit.Framework;

    /// <summary>
    /// The evernote content creator tests.
    /// </summary>
    [TestFixture]
    public class EvernoteContentCreatorTests
    {
        /// <summary>
        /// The the function <see cref="EverNoteContentCreator.RetrieveNoteBookGuidOfTheYear"/>
        /// </summary>
        [Test]
        public void TestReadNotebookOfTheYear()
        {
            TimeCapsule.TestableTime = new DateTime(2013, 11, 20);
            var testAccess = new UnitTestAccess();
            var contentCreator = new EverNoteContentCreator(testAccess);
            Assert.AreEqual(0, testAccess.ListNotebooks().Count, "First: The list of notebooks must be empty");
            
            var notebookGuid = contentCreator.RetrieveNoteBookGuidOfTheYear();

            // After I have retrieved a notebook of the year, a notebook must be created
            var notebook = testAccess.ListNotebooks().Find(n => n.Guid == notebookGuid);
            Assert.AreEqual("2013", notebook.Name);
        }
    }
}