// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="DropboxSupport.cs">
//   todo: license
// </copyright>
// <summary>
//   This class gives you some support functions for dropbox
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Windows.Media.Imaging;
    using System.Xml;

    using MyJournalTracker.Model;

    /// <summary>
    /// This class gives you some support functions for dropbox
    /// </summary>
    public class DropboxSupport
    {
        #region Enums

        /// <summary>
        /// The journal path item.
        /// </summary>
        public enum JournalPathItem
        {
            /// <summary>
            /// The journal path root
            /// </summary>
            JournalPathRoot, 

            /// <summary>
            /// The journal path to the xml entry fields
            /// </summary>
            JournalPathEntries, 

            /// <summary>
            /// The journal path to the photo files
            /// </summary>
            JournalPathPhotos, 
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Auto detect Dropbox path to the dropbox folder
        /// </summary>
        /// <returns>
        /// pathname of the user specific dropbox path
        /// </returns>
        /// <exception cref="System.IO.FileNotFoundException">Couldn't locate the Dropbox folder or the Dropbox host.db file. Maybe, you haven't installed dropbox</exception>
        public static string GetDropboxPath()
        {
            var appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var dropboxPath = Path.Combine(appDataPath, "Dropbox\\host.db");

            if (!File.Exists(dropboxPath))
            {
                throw new FileNotFoundException(
                    "Couldn't locate the Dropbox folder or the Dropbox host.db file. Maybe, you haven't installed dropbox");
            }

            var lines = File.ReadAllLines(dropboxPath);
            var base64String = Convert.FromBase64String(lines[1]);
            var folderPath = Encoding.ASCII.GetString(base64String);

            return folderPath;
        }

        /// <summary>
        /// Auto detect the path of the Journal Day One Application Folder in Dropbox
        /// </summary>
        /// <param name="pathItem">
        /// The <see cref="JournalPathItem"/>
        /// </param>
        /// <returns>
        /// pathname of the journal day one application folder in Dropbox.
        /// </returns>
        /// <exception cref="System.IO.DirectoryNotFoundException">
        /// Couldn't detect the Day One folder. Maybe, you haven't used the iOS Day One App before
        /// </exception>
        public static string GetJournalDayOnePath(JournalPathItem pathItem)
        {
            var dropboxPath = GetDropboxPath();
            var journalPath = Path.Combine(dropboxPath, "Apps\\Day One\\Journal.dayone");

            switch (pathItem)
            {
                case JournalPathItem.JournalPathRoot:
                    break;
                case JournalPathItem.JournalPathEntries:
                    journalPath = Path.Combine(journalPath, "entries");
                    break;
                case JournalPathItem.JournalPathPhotos:
                    journalPath = Path.Combine(journalPath, "photos");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("pathItem");
            }

            if (!Directory.Exists(journalPath))
            {
                throw new DirectoryNotFoundException(
                    "Couldn't detect the Day One folder. Maybe, you haven't used the iOS Day One App before");
            }

            return journalPath;
        }

        /// <summary>
        /// The save.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <param name="folderPath">
        /// The folder path.
        /// </param>
        public void Save(Entry entry, string folderPath)
        {
            var doc = new XmlDocument();

            // <?xml ...?>
            var decl = doc.CreateXmlDeclaration("1.0", "UTF-8", null);
            doc.AppendChild(decl);

            // http://stackoverflow.com/questions/11135343/xml-documenttype-method-createdocumenttype-crashes-if-dtd-is-absent-net-c-sharp
            doc.XmlResolver = null;

            // <!DOCTYPE ...>
            var doctype = doc.CreateDocumentType(
                "plist", "-//Apple//DTD PLIST 1.0//EN", "http://www.apple.com/DTDs/PropertyList-1.0.dtd", null);
            doc.AppendChild(doctype);

            // <plist version="1.0">
            var root = doc.CreateElement("plist");
            doc.AppendChild(root);

            var attrVersion = doc.CreateAttribute("version");
            root.Attributes.Append(attrVersion);
            attrVersion.Value = "1.0";

            // <dict>
            var dict = doc.CreateElement("dict");
            root.AppendChild(dict);

            // key values
            this.AppendKeyValue(doc, dict, "Creation Date", "date", entry.CreationDate);
            this.AppendKeyValue(doc, dict, "Entry Text", "string", entry.EntryText);
            this.AppendKeyValue(doc, dict, "Starred", entry.Starred.ToString().ToLower(), null);
            this.AppendKeyValue(doc, dict, "UUID", "string", entry.UUIDString);

            // Handle unknown key values. (just keep them.)
            foreach (var kvp in entry.UnknownKeyValues)
            {
                this.AppendKeyValue(doc, dict, kvp.Key, kvp.Value.Key, kvp.Value.Value);
            }

            // Write to the stringbuilder first, and then write it to the file.
            var builder = new StringBuilder();
            using (StringWriter stringWriter = new UTF8StringWriter(builder))
            {
                stringWriter.NewLine = "\n";
                doc.Save(stringWriter);

                // Some tricks to make the result exactly the same as the original one.
                stringWriter.WriteLine();
                builder.Replace("utf-8", "UTF-8");
                builder.Replace("    <", "\t<");
                builder.Replace("  <", "<");

                using (
                    var streamWriter = new StreamWriter(
                        Path.Combine(folderPath, entry.FileName), false, new UTF8Encoding()))
                {
                    streamWriter.Write(builder.ToString());

                    // Now it's not dirty!
                    entry.IsDirty = false;
                }
            }
        }

        /// <summary>
        /// Saves the EntryPicture.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <param name="photoJournalPath">
        /// The photo journal path.
        /// </param>
        public void SavePicture(Entry entry, string photoJournalPath)
        {
            var pathName = Path.Combine(photoJournalPath, entry.UUIDString + ".jpg");
            var jpg = new JpegBitmapEncoder { QualityLevel = 90 };
            jpg.Frames.Add(BitmapFrame.Create(entry.EntryPicture));
            var fs = new FileStream(pathName, FileMode.Create);
            jpg.Save(fs);
            fs.Close();
        }

        /// <summary>
        /// Loads an entry from the given filename.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// An <see cref="Entry"/> object representing the given file.
        /// </returns>
        public Entry LoadFromFile(string path)
        {
            try
            {
                using (var sr = new StreamReader(path, Encoding.UTF8))
                {
                    var newEntry = new Entry();

                    var fileContent = sr.ReadToEnd().TrimStart();

                    var doc = new XmlDocument();
                    doc.LoadXml(fileContent);

                    var dictNode = doc.SelectSingleNode("//dict");
                    Debug.Assert(
                        dictNode != null && dictNode.ChildNodes.Count % 2 == 0,
                        "A dict node must have even number of children (key, value)");
                    for (var i = 0; i < dictNode.ChildNodes.Count; i += 2)
                    {
                        var keyNode = dictNode.ChildNodes[i];
                        Debug.Assert(keyNode.Name == "key", "key element must appear");

                        var valueNode = dictNode.ChildNodes[i + 1];

                        switch (keyNode.InnerText)
                        {
                            case "Creation Date":
                                {
                                    newEntry.UtcDateTime = DateTime.Parse(valueNode.InnerText).ToUniversalTime();
                                    break;
                                }

                            case "Entry Text":
                                {
                                    newEntry.EntryText = valueNode.InnerText;
                                    break;
                                }

                            case "Starred":
                                {
                                    newEntry.Starred = valueNode.Name == "true";
                                    break;
                                }

                            case "UUID":
                                {
                                    newEntry.UUID = new Guid(valueNode.InnerText);
                                    break;
                                }

                            default:
                                newEntry.UnknownKeyValues.Add(
                                    keyNode.InnerText,
                                    new KeyValuePair<string, string>(valueNode.Name, valueNode.InnerText));
                                break;
                        }
                    }

                    newEntry.IsDirty = false;

                    return newEntry;
                }
            }
            catch (Exception e)
            {
                // Write to a log file.
                var builder = new StringBuilder();
                builder.AppendLine("An error occurred while reading entry \"" + path + "\"");
                builder.AppendLine(e.Message);
                builder.AppendLine(e.StackTrace);

                Logger.Log(builder.ToString());

                return null;
            }
        }

        /// <summary>
        /// Deletes this entry from the specified folder path.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <param name="folderPath">
        /// The folder path.
        /// </param>
        public void Delete(Entry entry, string folderPath)
        {
            var path = Path.Combine(folderPath, entry.FileName);

            var finfo = new FileInfo(path);
            if (finfo.Exists)
            {
                finfo.Delete();
            }
        }

        /// <summary>
        /// Appends the key value.
        /// </summary>
        /// <param name="doc">
        /// The xml document.
        /// </param>
        /// <param name="dict">
        /// The xml element corresponding to dictionary part.
        /// </param>
        /// <param name="keyString">
        /// The key string.
        /// </param>
        /// <param name="valueType">
        /// Type of the value.
        /// </param>
        /// <param name="valueString">
        /// The value string.
        /// </param>
        private void AppendKeyValue(
            XmlDocument doc, XmlElement dict, string keyString, string valueType, string valueString)
        {
            var key = doc.CreateElement("key");
            dict.AppendChild(key);
            key.InnerText = keyString;

            var value = doc.CreateElement(valueType);
            dict.AppendChild(value);
            if (valueString != null)
            {
                value.InnerText = valueString;
            }
        }

        #endregion
    }
}