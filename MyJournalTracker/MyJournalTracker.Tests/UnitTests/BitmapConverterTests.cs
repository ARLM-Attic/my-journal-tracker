// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="BitmapConverterTests.cs">
//   todo: license
// </copyright>
// <summary>
//   The bitmap converter tests.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Tests.UnitTests
{
    using System.IO;

    using MyJournalTracker.Utility;

    using NUnit.Framework;

    /// <summary>
    /// The bitmap converter tests.
    /// </summary>
    [TestFixture]
    public class BitmapConverterTests
    {
        /// <summary>
        /// Converts a png bitmap to a jpeg
        /// </summary>
        [Test]
        public void ConvertToJPegTest()
        {
            var bc = new BitmapConverter();

            // check for test data
            const string TestDataScreenshotPng = @".\Test Data\Screenshot.png";
            Assert.IsTrue(File.Exists(TestDataScreenshotPng));

            var destinationJPEGFile = PathUtility.CreateTemporaryPathname();        
            bc.ConvertToJpeg(TestDataScreenshotPng, destinationJPEGFile);
        }
    }
}