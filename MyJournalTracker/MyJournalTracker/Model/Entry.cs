// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="Entry.cs">
//   todo: license
// </copyright>
// <summary>
//   A class representing a journal entry.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Windows.Media.Imaging;
    using System.Xml;

    using MyJournalTracker.Annotations;
    using MyJournalTracker.Utility;

    /// <summary>
    /// A class representing a journal entry.
    /// </summary>
    public class Entry : IEquatable<Entry>, INotifyPropertyChanged
    {
        #region Fields

        /// <summary>
        /// The unknown key values
        /// </summary>
        private readonly Dictionary<string, KeyValuePair<string, string>> unknownKeyValues =
            new Dictionary<string, KeyValuePair<string, string>>();

        /// <summary>
        /// The bitmap image for the associated EntryPicture of the journal entry
        /// </summary>
        private BitmapSource entryPicture;

        /// <summary>
        /// The entry text
        /// </summary>
        private string entryText;

        /// <summary>
        /// Indicates whether this entry is starred or not
        /// </summary>
        private bool starred;

        /// <summary>
        /// The UTC date time
        /// </summary>
        private DateTime utcDateTime;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        public Entry()
            : this(DateTime.UtcNow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        public Entry(DateTime dateTime)
            : this(dateTime, Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        /// <param name="uuid">
        /// The UUID.
        /// </param>
        public Entry(Guid uuid)
            : this(DateTime.UtcNow, uuid)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <param name="uuid">
        /// The UUID.
        /// </param>
        public Entry(DateTime dateTime, Guid uuid)
            : this(dateTime, string.Empty, false, uuid, true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entry"/> class.
        /// </summary>
        /// <param name="dateTime">
        /// The date time.
        /// </param>
        /// <param name="entryText">
        /// The entry text.
        /// </param>
        /// <param name="starred">
        /// if set to <c>true</c> [starred].
        /// </param>
        /// <param name="uuid">
        /// The UUID.
        /// </param>
        /// <param name="isDirty">
        /// if set to <c>true</c> [is dirty].
        /// </param>
        public Entry(DateTime dateTime, string entryText, bool starred, Guid uuid, bool isDirty)
        {
            this.UtcDateTime = dateTime;
            this.EntryText = entryText;
            this.Starred = starred;
            this.UUID = uuid;
            this.IsDirty = isDirty;
        }

        #endregion

        #region Public Events

        /// <summary>
        /// Notify, when a property changed
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the creation date as string.
        /// </summary>
        /// <value>
        /// The creation date.
        /// </value>
        public string CreationDate
        {
            get
            {
                return this.UtcDateTime.ToString("u").Replace(' ', 'T');
            }
        }

        /// <summary>
        /// Gets or sets the Image for the associated EntryPicture of the journal entry
        /// </summary>
        /// <remarks>
        /// The dirty tag will be set, if the new text is different
        /// </remarks>
        /// <value>
        /// The entry text.
        /// </value>
        public BitmapSource EntryPicture
        {
            get
            {
                return this.entryPicture;
            }

            set
            {
                if (this.entryPicture == null || !this.entryPicture.Equals(value))
                {
                    this.entryPicture = value;
                    this.IsDirty = true;
                    this.OnPropertyChanged("EntryPicture");
                }
            }
        }

        /// <summary>
        /// Gets or sets the entry text.
        /// </summary>
        /// <remarks>
        /// The dirty tag will be set, if the new text is different
        /// </remarks>
        /// <value>
        /// The entry text.
        /// </value>
        public string EntryText
        {
            get
            {
                return this.entryText;
            }

            set
            {
                if (this.entryText != value)
                {
                    this.entryText = value;
                    this.IsDirty = true;
                    this.OnPropertyChanged("EntryText");
                }
            }
        }

        /// <summary>
        /// Gets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        public string FileName
        {
            get
            {
                return this.UUIDString + ".doentry";
            }
        }

        /// <summary>
        /// Sets the frame for the sample EntryPicture in Blend
        /// </summary>
        public BitmapFrame Frame
        {
            set
            {
                this.EntryPicture = new BitmapImage(new Uri(value.ToString()));
            }
        }

        /// <summary>
        /// Gets a value indicating whether the entry has a associated EntryPicture
        /// </summary>
        /// <value>
        ///   <c>true</c> if the entry has a EntryPicture; otherwise, <c>false</c>.
        /// </value>
        public bool HasPicture
        {
            get
            {
                return this.EntryPicture != null;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty { get; private set; }

        /// <summary>
        /// Gets the local time.
        /// </summary>
        /// <value>
        /// The local time.
        /// </value>
        public DateTime LocalTime
        {
            get
            {
                return this.UtcDateTime.ToLocalTime();
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Entry"/> is starred.
        /// </summary>
        /// <value>
        ///   <c>true</c> if starred; otherwise, <c>false</c>.
        /// </value>
        public bool Starred
        {
            get
            {
                return this.starred;
            }

            set
            {
                if (this.starred != value)
                {
                    this.starred = value;
                    this.IsDirty = true;
                }
            }
        }

        /// <summary>
        /// Gets the UUID.
        /// Once set, should never be changed.
        /// </summary>
        /// <value>
        /// The UUID.
        /// </value>
        public Guid UUID { get; private set; }

        /// <summary>
        /// Gets the UUID string.
        /// </summary>
        /// <value>
        /// The UUID string.
        /// </value>
        public string UUIDString
        {
            get
            {
                return this.UUID.ToString("N").ToUpper();
            }
        }

        /// <summary>
        /// Gets the unknown key values.
        /// </summary>
        /// <value>
        /// The unknown key values.
        /// </value>
        public Dictionary<string, KeyValuePair<string, string>> UnknownKeyValues
        {
            get
            {
                return this.unknownKeyValues;
            }
        }

        /// <summary>
        /// Gets or sets the UTC date time.
        /// </summary>
        /// <value>
        /// The UTC date time.
        /// </value>
        public DateTime UtcDateTime
        {
            get
            {
                return this.utcDateTime;
            }

            set
            {
                var temp = value;
                temp = temp.AddTicks(-(temp.Ticks % 10000000));

                if (temp.Kind != DateTimeKind.Utc)
                {
                    temp = temp.ToUniversalTime();
                }

                if (this.utcDateTime != temp)
                {
                    this.utcDateTime = temp;
                    this.IsDirty = true;
                }
            }
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// Loads an entry from the given filename.
        /// </summary>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// An <see cref="Entry"/> object representing the given file.
        /// </returns>
        public static Entry LoadFromFile(string path)
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
        /// <param name="folderPath">
        /// The folder path.
        /// </param>
        public void Delete(string folderPath)
        {
            var path = Path.Combine(folderPath, this.FileName);

            var finfo = new FileInfo(path);
            if (finfo.Exists)
            {
                finfo.Delete();
            }
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="right">
        /// The <see cref="System.Object"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object right)
        {
            if (ReferenceEquals(right, null))
            {
                return false;
            }

            if (ReferenceEquals(this, right))
            {
                return true;
            }

            if (this.GetType() != right.GetType())
            {
                return false;
            }

            return this.Equals(right as Entry);
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="right">
        /// The <see cref="Entry"/> to compare with this instance.
        /// </param>
        /// <returns>
        /// <c>true</c> if the specified <see cref="Entry"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(Entry right)
        {
            if (this.UtcDateTime != right.UtcDateTime)
            {
                return false;
            }

            if (this.EntryText != right.EntryText)
            {
                return false;
            }

            if (this.Starred != right.Starred)
            {
                return false;
            }

            if (this.UUID != right.UUID)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.UUID.GetHashCode();
        }

        /// <summary>
        /// Saves this entry into the specified folder path.
        /// </summary>
        /// <param name="folderPath">
        /// The folder path.
        /// </param>
        public void Save(string folderPath)
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
            this.AppendKeyValue(doc, dict, "Creation Date", "date", this.CreationDate);
            this.AppendKeyValue(doc, dict, "Entry Text", "string", this.EntryText);
            this.AppendKeyValue(doc, dict, "Starred", this.Starred.ToString().ToLower(), null);
            this.AppendKeyValue(doc, dict, "UUID", "string", this.UUIDString);

            // Handle unknown key values. (just keep them.)
            foreach (var kvp in this.UnknownKeyValues)
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
                        Path.Combine(folderPath, this.FileName), false, new UTF8Encoding()))
                {
                    streamWriter.Write(builder.ToString());

                    // Now it's not dirty!
                    this.IsDirty = false;
                }
            }
        }

        /// <summary>
        /// Saves the EntryPicture.
        /// </summary>
        /// <param name="photoJournalPath">
        /// The photo journal path.
        /// </param>
        public void SavePicture(string photoJournalPath)
        {
            var pathName = Path.Combine(photoJournalPath, this.UUIDString + ".jpg");
            var jpg = new JpegBitmapEncoder { QualityLevel = 90 };
            jpg.Frames.Add(BitmapFrame.Create(this.entryPicture));
            var fs = new FileStream(pathName, FileMode.Create);
            jpg.Save(fs);
            fs.Close();
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format("Date: {0}, Entry Text: \"{1}\"", this.CreationDate, this.EntryText);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Called when [property changed].
        /// </summary>
        /// <param name="propertyName">
        /// Name of the property.
        /// </param>
        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
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