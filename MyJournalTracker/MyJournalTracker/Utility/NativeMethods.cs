// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="NativeMethods.cs">
//   todo: license
// </copyright>
// <summary>
//   FxCop requires all marshalled functions to be in a class called NativeMethods.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.Runtime.InteropServices;

    /// <summary>
    /// FxCop requires all marshalled functions to be in a class called NativeMethods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// The delete object.
        /// </summary>
        /// <param name="hObject">
        /// The h object.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        [DllImport("gdi32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool DeleteObject(IntPtr hObject);
    }
}