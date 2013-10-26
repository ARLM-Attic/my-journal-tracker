// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="ImageExtensions.cs">
//   todo: license
// </copyright>
// <summary>
//   The image extensions.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// The image extensions.
    /// </summary>
    public static class ImageExtensions
    {
        #region Public Methods and Operators

        /// <summary>
        /// Returns a WPF <see cref="BitmapImage"/> to the passed 
        /// GDI+ <see cref="Image"/>
        /// </summary>
        /// <param name="image">
        /// The GDI+ image.
        /// </param>
        /// <returns>
        /// a WPF BitmapImage
        /// </returns>
        public static BitmapSource ToBitmapImage(this Image image)
        {
            var bitmap = new Bitmap(image);

            var bitSrc = bitmap.ToBitmapSource();

            bitmap.Dispose();

            return bitSrc;
        }

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>
        /// Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">
        /// The source bitmap.
        /// </param>
        /// <returns>
        /// A BitmapSource
        /// </returns>
        public static BitmapSource ToBitmapSource(this Bitmap source)
        {
            BitmapSource bitSrc;

            var bitmapHandle = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    bitmapHandle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                NativeMethods.DeleteObject(bitmapHandle);
            }

            return bitSrc;
        }

        #endregion
    }
}