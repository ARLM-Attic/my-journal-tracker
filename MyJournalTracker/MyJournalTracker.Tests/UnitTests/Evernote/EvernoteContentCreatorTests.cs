// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteContentCreatorTests.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests.Evernote
{
    using System;

    using MyJournalTracker.EverNoteSupport;
    using MyJournalTracker.Helper;

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

        /// <summary>
        /// The test read from template.
        /// </summary>
        [Test]
        public void TestReadFromTemplate()
        {
            var paragraph = EvernoteContentHelper.ReadTemplate("paragraph");
            Assert.IsNotEmpty(paragraph);
        }

        /// <summary>
        /// The test substitute with template.
        /// </summary>
        [Test]
        public void TestSubstituteWithTemplate()
        {
            var contentCreator = new EverNoteContentCreator();
            var template = contentCreator.SubstituteWithTemplate("paragraph", value: "this is me here!");
            Assert.AreEqual("<p style=\"font-family:'Helvetica'; line-height:1.8em; font-size:1em;\">this is me here!</p>", template);
        }

        /// <summary>
        /// Test function <see cref="EverNoteContentCreator.CreateEvernoteContent"/>
        /// </summary>
        [Test]
        public void TestCreateEvernoteContent()
        {
            var contentCreator = new EverNoteContentCreator();
            var evernoteContent = contentCreator.CreateEvernoteContent("this is a simple <b>markup</b>");
            Assert.IsTrue(evernoteContent.Contains("this is a simple <b>markup</b>"));
            Assert.IsTrue(evernoteContent.Contains("<en-note>"));
            Assert.IsTrue(evernoteContent.Contains("</en-note>"));
        }
    }
}