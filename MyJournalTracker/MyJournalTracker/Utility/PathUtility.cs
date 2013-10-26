// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="PathUtility.cs">
//   todo: license
// </copyright>
// <summary>
//   The path utility provides a number of convenience methods
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System.IO;

    /// <summary>
    /// The path utility provides a number of convenience methods
    /// </summary>
    public class PathUtility
    {
        #region Public Methods and Operators

        /// <summary>
        /// count the number of files in the directory with the given pathname
        /// </summary>
        /// <param name="pathname">
        /// The pathname.
        /// </param>
        /// <returns>
        /// number of files in the directory
        /// </returns>
        public static int CountFiles(string pathname)
        {
            var files = Directory.GetFiles(pathname);
            return files.Length;
        }

        /// <summary>
        /// This function creates a randomized filename in the temp directory
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateTemporaryDirectory()
        {
            var tempDirectory = CreateTemporaryPathname();
            Directory.CreateDirectory(tempDirectory);
            return tempDirectory;
        }

        /// <summary>
        /// Delete the directory and all files at the given path
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        public static void DeleteDirectory(string path)
        {
            var directory = new DirectoryInfo(path);
            foreach (var file in directory.GetFiles())
            {
                file.Delete();
            }

            foreach (var subDirectory in directory.GetDirectories())
            {
                subDirectory.Delete(true);
            }
        }

        /// <summary>
        /// The get temporary pathname.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string CreateTemporaryPathname()
        {
            return Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
        }

        #endregion
    }
}