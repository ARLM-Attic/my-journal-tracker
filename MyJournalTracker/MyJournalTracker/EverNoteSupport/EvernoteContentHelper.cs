// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteContentHelper.cs" company="">
//   
// </copyright>
// <summary>
//   The evernote helper function.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.EverNoteSupport
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Reflection;
    using System.Security.Cryptography;
    using System.Windows.Media.Imaging;

    using Evernote.EDAM.Type;

    /// <summary>
    ///     The evernote helper function.
    /// </summary>
    public static class EvernoteContentHelper
    {
        #region Public Methods and Operators

        /// <summary>
        /// The create resource from image.
        /// </summary>
        /// <param name="bitmapSource">
        /// The bitmap source.
        /// </param>
        /// <returns>
        /// The <see cref="Resource"/>.
        /// </returns>
        public static Resource CreateResourceFromImage(BitmapSource bitmapSource)
        {
            var resource = new Resource();
            var ms = new MemoryStream();
            var jpg = new JpegBitmapEncoder { QualityLevel = 90 };
            jpg.Frames.Add(BitmapFrame.Create(bitmapSource));
            jpg.Save(ms);
            resource.Mime = "image/jpeg";
            var data = new Data
                           {
                               Body = ms.ToArray()
                           };

            data.BodyHash = new MD5CryptoServiceProvider().ComputeHash(data.Body);

            resource.Data = data;
            return resource;
        }

        /// <summary>
        /// The en media tag with resource.
        /// </summary>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <param name="width">
        /// The width.
        /// </param>
        /// <param name="height">
        /// The height.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string EnMediaTagWithResource(Resource resource, long width, long height)
        {
            var hashHex = BitConverter.ToString(resource.Data.BodyHash).Replace("-", string.Empty).ToLower();

            var sizeAtr = width > 0 && height > 0
                                 ? string.Format("height=\"{0}\" width=\"{1}\" ", height, width)
                                 : string.Empty;
            return string.Format(
                "<en-media type=\"{0}\" {1}hash=\"{2}\"/>", 
                resource.Mime, 
                sizeAtr, 
                hashHex);
        }

        /// <summary>
        /// The extract note text.
        /// </summary>
        /// <param name="evernoteContent">
        /// The evernote content.
        /// </param>
        /// <returns>
        /// The <see cref="EvernoteContentParser.NoteTags"/>.
        /// </returns>
        public static EvernoteContentParser.NoteTags ExtractNoteText(string evernoteContent)
        {
            // "<?xml version="1.0" encoding="UTF-8"?><!DOCTYPE en-note SYSTEM "http://xml.evernote.com/pub/enml2.dtd"><en-note>das ist eine ordentliche Testnotiz</en-note>"
            EvernoteContentParser.NoteTags content = new EvernoteContentParser(evernoteContent).DoParsing();
            return content;
        }

        /// <summary>
        /// The read template.
        /// </summary>
        /// <param name="templateName">
        /// The template name.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string ReadTemplate(string templateName)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string resourceName = "MyJournalTracker.EverNoteSupport.Templates." + templateName + ".xml";
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                Debug.Assert(stream != null, "resource " + templateName + "not found!");
                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// The split into lines.
        /// </summary>
        /// <param name="noteWithLines">
        /// The note with lines.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public static string[] SplitIntoLines(string noteWithLines)
        {
            return noteWithLines.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// The to evernote time stamp.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <returns>
        /// The <see cref="long"/>.
        /// </returns>
        public static long ToEvernoteTimeStamp(this DateTime dateTime)
        {
            var unixDate = new DateTime(1970, 1, 1);
            TimeSpan span = dateTime - unixDate;
            return span.Milliseconds;
        }

        /// <summary>
        /// The split content in tags.
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="EvernoteContentParser.NoteTags"/>.
        /// </returns>
        public static EvernoteContentParser.NoteTags SplitContentInTags(Note note)
        {
            var parser = new EvernoteContentParser(note.Content);
            return parser.DoParsing();
        }

    #endregion
   }
}