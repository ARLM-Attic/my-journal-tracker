// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteContentHelperTests.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests.Evernote
{
    using System;

    using MyJournalTracker.EverNoteSupport;
    using MyJournalTracker.Tests.UnitTests.Helper;

    using NUnit.Framework;

    /// <summary>
    /// The evernote content helper tests.
    /// </summary>
    [TestFixture]
    public class EvernoteContentHelperTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// The test content helper.
        /// </summary>
        [Test]
        public void TestContentHelper()
        {
            const string TestEvernoteContent = 
                "<?xml version=\"1.0\" encoding=\"UTF-8\"?><!DOCTYPE en-note SYSTEM \"http://xml.evernote.com/pub/enml2.dtd\"><en-note>Das ist eine ordentliche Testnotiz</en-note>";

            var noteText = EvernoteContentHelper.ExtractNoteText(TestEvernoteContent);
            Assert.IsTrue(!string.IsNullOrEmpty(noteText.NoteOpenTag));
            Assert.AreEqual("Das ist eine ordentliche Testnotiz", noteText.ExtractedContent, "The content should be correct extracted");
        }

        /// <summary>
        /// The test extracting content from windows.
        /// </summary>
        [Test]
        public void TestExtractingContentFromWindows()
        {
            var evernoteContentFromWindows = TestingHelper.StringFromTestFileNamed("EvernoteFromWindowsClient.xml");
            var noteTags = EvernoteContentHelper.ExtractNoteText(evernoteContentFromWindows);

            Assert.IsTrue(noteTags.ExtractedContent.IndexOf("jetzt komt noch ein text", System.StringComparison.Ordinal) >= 0);
            Assert.AreEqual("<en-note style=\"word-wrap: break-word; -webkit-nbsp-mode: space; -webkit-line-break: after-white-space;\">", noteTags.NoteOpenTag);
        }


         #endregion

        /// <summary>
        /// The test extracting content with utf 8.
        /// </summary>
        [Test]
        public void TestExtractingContentWithUtf8()
        {
            var evernoteContentFromWindows = TestingHelper.StringFromTestFileNamed("NoteWithUTF8.xml");
            var noteTags = EvernoteContentHelper.ExtractNoteText(evernoteContentFromWindows);

            // Prüfe, ob die href noch unangetastet ist und das & - Zeichen als &amp vorliegt.
            Assert.IsTrue(noteTags.ExtractedContent.IndexOf("href=\"http://maps.google.com/maps?q=51.633144+7.523906&amp;z=14\"", StringComparison.Ordinal) >= 0);
        }

        /*
        func testCurrentYear()
        {
            TimeCapsule().setTestableYear(2013, month: 11,  day:20)
        
            let currentYear = TimeHelper().getCurrentYear()
            XCTAssertEqual("2013", currentYear, "The year should be 2013")
        }

        func testCurrentDate()
        {

            TimeCapsule().setTestableYear(2013, month: 11,  day:20)
        
            let currentDate = TimeHelper().getCurrentDateInLocaleFormat()
            XCTAssertEqual("20. November 2013", currentDate)
        }
    
        func testCurrentTime()
        {
            TimeCapsule().setTestableTime(2013, month: 11, day:20, hour: 11, minute: 15)

            let currentDate = TimeHelper().getCurrentTimeInLocaleFormat()
            XCTAssertEqual("11:15", currentDate)
        }*/
    }
}