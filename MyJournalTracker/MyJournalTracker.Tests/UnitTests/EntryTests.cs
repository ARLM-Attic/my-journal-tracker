// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="EntryTests.cs">
//   todo: license
// </copyright>
// <summary>
//   This is a test class for EntryTest and is intended
//   to contain all EntryTest Unit Tests
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Tests.UnitTests
{
    using System.IO;

    using MyJournalTracker.Model;
    using MyJournalTracker.Utility;

    using NUnit.Framework;

    /// <summary>
    /// This is a test class for EntryTest and is intended
    /// to contain all EntryTest Unit Tests
    /// </summary>
    [TestFixture]
    public class EntryTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// A test for Delete
        /// </summary>
        [Test]
        public void DeleteTest()
        {
            var ds = new DropboxSupport();
            var target = new Entry();
            const string FolderPath = ".";

            ds.Save(target, FolderPath);
            Assert.IsTrue(File.Exists(target.FileName));

            ds.Delete(target, FolderPath);
            Assert.IsFalse(File.Exists(target.FileName));
        }

        /// <summary>
        /// A test for Save
        /// </summary>
        [Test]
        public void SaveTest()
        {
            var ds = new DropboxSupport();
            var target = new Entry();
            ds.Save(target, ".");

            Assert.IsFalse(target.IsDirty);
            Assert.IsTrue(File.Exists(target.FileName));

            var loaded = ds.LoadFromFile(target.FileName);
            Assert.AreEqual(target, loaded);
        }

        /// <summary>
        /// A test for Save
        /// </summary>
        [Test]
        public void SaveTest1()
        {
            var ds = new DropboxSupport();
            var target = new Entry { Starred = true, EntryText = "This is the body.\n나는 바디다." };
            ds.Save(target, ".");

            Assert.IsFalse(target.IsDirty);
            Assert.IsTrue(File.Exists(target.FileName));

            var loaded = ds.LoadFromFile(target.FileName);
            Assert.AreEqual(target, loaded);
        }

        #endregion
    }
}