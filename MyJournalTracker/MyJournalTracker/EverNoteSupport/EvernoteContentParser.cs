// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteContentParser.cs" company="">
//   
// </copyright>
// <summary>
//   The note tags.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.EverNoteSupport
{
    using System.Diagnostics;
    using System.Diagnostics.CodeAnalysis;
    using System.Text.RegularExpressions;

    /// <summary>
    /// The evernote content parser.
    /// </summary>
    /// <remarks>
    /// #Accepts the GUID(string) of the note you want to append
    /// def appendNote(guid):
    /// #Get the note to be updated using the note's guid http://dev.evernote.com/documentation/reference/NoteStore.html#Fn_NoteStore_getNote
    /// note = client.get_note_store().getNote(guid, True, True, False, False)
    /// #Regular expressions used to replicate ENML tags.  These same tags will be used to "rebuild" the note with the existing note metadata
    /// xmlTag          = re.search('\?xml.*?>', note.content).group()
    /// docTag          = re.search('\!DOCTYPE.*?>', note.content).group()
    /// noteOpenTag     = re.search('\s*en-note.*?>', note.content).group()
    /// noteCloseTag    = re.search('\s*/en-note.*?>', note.content).group()
    /// breakTag        = '<br />'
    ///  #Rebuild the note using the new content
    /// content           =  note.content.replace(xmlTag, "").replace(noteOpenTag, "").replace(noteCloseTag, "").replace(docTag, "").strip()
    /// content           += breakTag + " ".join(sys.argv[1:])
    /// template          =  Template ('$xml $doc $openTag $body $closeTag')
    /// note.content      =  template.substitute(xml=xmlTag,doc=docTag,openTag=noteOpenTag,body=content,closeTag=noteCloseTag)
    /// #Update the note
    /// client.get_note_store().updateNote(note)
    /// #Return updated note (object) to the function
    /// </remarks>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Reviewed. Suppression is OK here.")]
    public class EvernoteContentParser
    {
        #region Fields

        /// <summary>
        /// The evernote content.
        /// </summary>
        private readonly string evernoteContent;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EvernoteContentParser"/> class.
        /// </summary>
        /// <param name="content">
        /// The content.
        /// </param>
        public EvernoteContentParser(string content)
        {
            Debug.Assert(!string.IsNullOrEmpty(content), "ther must be a non empty string given to the EvernoteContentParser");
            this.evernoteContent = content;
        }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The do parsing.
        /// </summary>
        /// <returns>
        /// The <see cref="NoteTags"/>.
        /// </returns>
        public NoteTags DoParsing()
        {
            string xmlTag = this.SearchTag("<\\?xml.*?>");
            string docTag = this.SearchTag("<\\!DOCTYPE.*?>");
            string noteOpenTag = this.SearchTag("<en-note.*?>");
            string noteCloseTag = this.SearchTag("</en-note.*?>");

            string content =
                this.evernoteContent.Replace(xmlTag, string.Empty)
                    .Replace(docTag, string.Empty)
                    .Replace(noteOpenTag, string.Empty)
                    .Replace(noteCloseTag, string.Empty);

            return new NoteTags
                       {
                           XmlTag = xmlTag,
                           DocTag = docTag,
                           NoteOpenTag = noteOpenTag,
                           NoteCloseTag = noteCloseTag,
                           ExtractedContent = content
                       };
        }

        /// <summary>
        /// The search tag.
        /// </summary>
        /// <param name="pattern">
        /// The pattern.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string SearchTag(string pattern)
        {
            var regex = new Regex(pattern);
            Match match = regex.Match(this.evernoteContent);
            Debug.Assert(match.Success, "malformed evernote content. couldn't resolve tag: " + pattern);
            return match.Groups[1].Value;
        }

        #endregion

        /// <summary>
        /// The note tags.
        /// </summary>
        public class NoteTags
        {
            #region Fields

            /// <summary>
            /// The doc tag.
            /// </summary>
            public string DocTag = "<!DOCTYPE en-note SYSTEM \"http://xml.evernote.com/pub/enml2.dtd\">";

            /// <summary>
            /// The extracted content.
            /// </summary>
            public string ExtractedContent = string.Empty;

            /// <summary>
            /// The note close tag.
            /// </summary>
            public string NoteCloseTag = "</en-note>";

            /// <summary>
            /// The note open tag.
            /// </summary>
            public string NoteOpenTag = "<en-note>";

            /// <summary>
            /// The xml tag.
            /// </summary>
            public string XmlTag = "<?xml version=\"1.0\" encoding=\"UTF-8\"?>";

            #endregion
        }
    }
}