// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestingHelper.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests.Helper
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Windows;

    /// <summary>
    /// The testing helper.
    /// </summary>
    internal static class TestingHelper
    {
        /// <summary>
        /// The string from testdata.
        /// </summary>
        /// <param name="testfilename">
        /// The testfilename.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        internal static string StringFromTestFileNamed(string testfilename)
        {
            var assembly = Assembly.GetExecutingAssembly();
            
            // var n = assembly.GetManifestResourceNames();
            var resourceName = "MyJournalTracker.Tests.UnitTests.Evernote.Test_data." + testfilename;
            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                Debug.Assert(stream != null, "resource " + testfilename + "not found!");
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}