// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="ConverterTests.cs">
//   todo: license
// </copyright>
// <summary>
//   The converter tests.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Tests.UnitTests
{
    using System;

    using MyJournalTracker.Helper;

    using NUnit.Framework;

    /// <summary>
    /// The converter tests.
    /// </summary>
    [TestFixture]
    public class ConverterTests
    {
        #region Public Methods and Operators

        /// <summary>
        /// The convert date time test.
        /// </summary>
        [Test]
        public void ConvertDateTimeTest()
        {
            var c = new Converter();
            DateTime dt = c.ConvertInputDateTime("17-07-2013", "15:20");
            Assert.AreEqual(new DateTime(2013, 7, 17, 15, 20, 0), dt);
        }

        #endregion
    }
}