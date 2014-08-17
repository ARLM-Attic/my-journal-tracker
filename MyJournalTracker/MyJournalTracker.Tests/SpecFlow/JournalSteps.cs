// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="JournalSteps.cs">
//   todo: license
// </copyright>
// <summary>
//   Step definitions for the Journal.feature file.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Tests.SpecFlow
{
    using System.Diagnostics.CodeAnalysis;

    using MyJournalTracker.Helper;
    using MyJournalTracker.Logic;
    using MyJournalTracker.Model;
    using MyJournalTracker.Utility;

    using NUnit.Framework;

    using TechTalk.SpecFlow;

    /// <summary>
    /// Step definitions for the Journal.feature file.
    /// </summary>
    [Binding]
    public class JournalSteps
    {
        #region Fields

        /// <summary>
        /// The test entry.
        /// </summary>
        private Entry entry;

        /// <summary>
        /// The path drop box.
        /// </summary>
        private string pathDropBox;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="JournalSteps"/> class. 
        /// Create a test Dropbox directory
        /// </summary>
        public JournalSteps()
        {
            var pu = new PathUtility();
            this.pathDropBox = PathUtility.CreateTemporaryDirectory();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="JournalSteps"/> class.
        /// Deletes the test Dropbox directory
        /// </summary>
        ~JournalSteps()
        {
            PathUtility.DeleteDirectory(this.pathDropBox);
        }

        #endregion

        #region Public Methods and Operators

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", 
            Justification = "Reviewed. Suppression is OK here.")]
        [Given(@"I enter a new entry on the (.*) at (.*) with the following text")]
        public void GivenIEnterANewEntryOnTheJuliAtWithTheFollowingText(string date, string time, string entryText)
        {
            var c = new Converter();
            this.entry = new Entry { EntryText = entryText };
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", 
            Justification = "Reviewed. Suppression is OK here.")]
        [Then(@"a new doentry file is visible in my dropbox folder")]
        public void ThenANewDoentryFileIsVisibleInMyDropboxFolder()
        {
            Assert.AreEqual(1, PathUtility.CountFiles(this.pathDropBox));
        }

        [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1600:ElementsMustBeDocumented", 
            Justification = "Reviewed. Suppression is OK here.")]
        [When(@"I click on Save")]
        public void WhenIClickOnSave()
        {
            var ds = new DropboxSupport();
            ds.Save(this.entry, this.pathDropBox);
        }

        #endregion
    }
}