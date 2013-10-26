// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="BitmapConverter.cs">
//   todo: license
// </copyright>
// <summary>
//   The bitmap converter.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Linq;
    using System.Windows.Media.Imaging;

    /// <summary>
    /// The bitmap converter.
    /// </summary>
    public class BitmapConverter
    {
        #region Public Methods and Operators

        /// <summary>
        /// Method to convert the original picture to a jpeg file in the destination path
        /// </summary>
        /// <param name="originalPicture">
        /// The original picture.
        /// </param>
        /// <param name="destinationPath">
        /// The destination path.
        /// </param>
        public void ConvertToJpeg(string originalPicture, string destinationPath)
        {
            var bi = new BitmapImage();

            bi.BeginInit();
            bi.UriSource = new Uri(Path.GetFullPath(originalPicture));
            bi.EndInit();

            var jpg = new JpegBitmapEncoder { QualityLevel = 90 };
            jpg.Frames.Add(BitmapFrame.Create(bi));

            var fs = new FileStream(Path.GetFullPath(destinationPath), FileMode.Create);
            jpg.Save(fs);
            fs.Close();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Search an encoder for the given image format
        /// </summary>
        /// <param name="format">
        /// The <see cref="ImageFormat"/>
        /// </param>
        /// <returns>
        /// The Encode <see cref="ImageCodecInfo"/> for the given <see cref="ImageFormat"/>
        /// </returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            var codecs = ImageCodecInfo.GetImageDecoders();

            return codecs.FirstOrDefault(codec => codec.FormatID == format.Guid);
        }

        #endregion
    }
}