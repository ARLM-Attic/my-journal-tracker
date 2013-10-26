// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="PathUtilityTests.cs">
//   todo: license
// </copyright>
// <summary>
//   The path utility tests.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Tests.UnitTests
{
    using MyJournalTracker.Utility;

    using NUnit.Framework;

    /// <summary>
    /// The path utility tests.
    /// </summary>
    [TestFixture]
    public class PathUtilityTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// Test for the <see cref="PathUtility.CreateTemporaryDirectory"/> function
        /// </summary>
        /// <remarks>
        /// There should be no files in the newly created directory
        /// </remarks>
        [Test]
        public void GetTemporaryDirectoryTest()
        {
            var pu = new PathUtility();
            var path = PathUtility.CreateTemporaryDirectory();
            Assert.IsFalse(string.IsNullOrEmpty(path));
            Assert.AreEqual(0, PathUtility.CountFiles(path));
        }

        #endregion
    }
}