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

    using Utility;

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
            const string testDataScreenshotPng = @".\Test Data\Screenshot.png";
            Assert.IsTrue(File.Exists(testDataScreenshotPng));

            var destinationJpegFile = PathUtility.CreateTemporaryPathname();        
            bc.ConvertToJpeg(testDataScreenshotPng, destinationJpegFile);
        }
    }
}