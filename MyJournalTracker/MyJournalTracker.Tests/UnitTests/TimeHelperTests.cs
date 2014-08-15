// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TimeHelperTests.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests
{
    using System;
    using System.Globalization;

    using MyJournalTracker.Helper;

    using NUnit.Framework;

    /// <summary>
    /// The time helper tests.
    /// </summary>
    [TestFixture]
    public class TimeHelperTests
    {
        /// <summary>
        /// The test date in this set of unit tests: 11/20/2013
        /// </summary>
        private static readonly DateTime TestTime = new DateTime(2013, 11, 20);

        /// <summary>
        /// The test current year.
        /// </summary>
        [Test]
        public void TestCurrentYear()
        {
            TimeCapsule.TestableTime = TestTime;

            var currentYear = new TimeHelper().GetCurrentYear();
            Assert.AreEqual("2013", currentYear, "The year should be 2013");
        }

        /// <summary>
        /// The test current date.
        /// </summary>
        [Test]
        public void TestCurrentDateGerman()
        {
            TestLocaleFormat("de-DE", "20. November 2013");
        }

        /// <summary>
        /// The test current date.
        /// </summary>
        [Test]
        public void TestCurrentDateEnUs()
        {
            TestLocaleFormat("en-US", "November 20, 2013");
        }

        /// <summary>
        /// The test current time.
        /// </summary>
        [Test]
        public void TestCurrentTime()
        {
            TimeCapsule.TestableTime = new DateTime(2013, 11, 10, 11, 15, 0);
            var currentDate = new TimeHelper().GetCurrentTimeInLocaleFormat();
            Assert.AreEqual("11:15", currentDate);
        }

        /// <summary>
        /// The test locale format.
        /// </summary>
        /// <param name="cultureName">
        /// The culture name.
        /// </param>
        /// <param name="referenceFormat">
        /// The reference format.
        /// </param>
        private static void TestLocaleFormat(string cultureName, string referenceFormat)
        {
            // ReSharper disable once ObjectCreationAsStatement
            InitTestTime(cultureName);

            var currentDate = new TimeHelper().GetCurrentDateInLocaleFormat();
            Assert.AreEqual(referenceFormat, currentDate);
        }

        /// <summary>
        /// The init test time.
        /// </summary>
        /// <param name="cultureName">
        /// The culture name.
        /// </param>
        private static void InitTestTime(string cultureName = "de-DE")
        {
            // ReSharper disable once ObjectCreationAsStatement
            TimeCapsule.TestableTime = TestTime;
            TimeCapsule.TestableCultureInfo = new CultureInfo(cultureName);
        }
    }
}