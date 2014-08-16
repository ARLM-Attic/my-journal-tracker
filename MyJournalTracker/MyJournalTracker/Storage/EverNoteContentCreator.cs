// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EverNoteContentCreator.cs" company="">
//   
// </copyright>
// <summary>
//   The ever note content creator.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Storage
{
    using System;
    using System.Collections.Generic;
    using System.Windows.Media.Imaging;

    using Evernote.EDAM.Type;

    using MyJournalTracker.EverNoteSupport;
    using MyJournalTracker.Helper;

    /// <summary>
    /// The ever note content creator.
    /// </summary>
    public class EverNoteContentCreator
    {
        /// <summary>
        /// The time helper.
        /// </summary>
        private readonly TimeHelper timeHelper = new TimeHelper();

        /// <summary>
        /// The evernote access.
        /// </summary>
        private readonly IAbstractEvernoteAccess evernoteAccess;

        /// <summary>
        /// Initializes a new instance of the <see cref="EverNoteContentCreator"/> class.
        /// </summary>
        /// <param name="authToken">
        /// The auth token.
        /// </param>
        public EverNoteContentCreator(string authToken)
        {
            this.evernoteAccess = new EvernoteAccess(authToken);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EverNoteContentCreator"/> class for Unit-Testing
        /// </summary>
        /// <param name="abstractEvernoteAccess">
        /// The abstract Evernote Access.
        /// </param>
        public EverNoteContentCreator(IAbstractEvernoteAccess abstractEvernoteAccess)
        {
            this.evernoteAccess = abstractEvernoteAccess;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EverNoteContentCreator"/> class.
        /// </summary>
        public EverNoteContentCreator()
        {
        }

        /// <summary>
        /// The read note book of the year.
        /// </summary>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string RetrieveNoteBookGuidOfTheYear()
        {
            var currentYear = this.timeHelper.GetCurrentYear();
            var notebooks = this.evernoteAccess.ListNotebooks();
            var notebook = notebooks.Find(nb => nb.Name == currentYear);

            // notebook of the year found. return guid
            if (notebook != null)
            {
                return notebook.Guid;
            }

            // No Notebook of the year found. Create it.
            notebook = new Notebook
                           {
                               Guid = Guid.NewGuid().ToString(),
                               Name = currentYear,
                               ServiceCreated = TimeCapsule.GetCurrentTime().ToEvernoteTimeStamp()
                           };

            this.evernoteAccess.CreateNotebook(notebook);
            return notebook.Guid;
        }

        /// <summary>
        /// The substitute with template.
        /// </summary>
        /// <param name="templateName">
        /// The template name.
        /// </param>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string SubstituteWithTemplate(string templateName, string value)
        {
            var template = EvernoteHelperFunction.ReadTemplate(templateName);
            return template.Replace("%1", value);
        }

        /// <summary>
        /// create the correct en-note evernote content markaup
        /// </summary>
        /// <param name="text">
        /// the raw xhtml content
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string CreateEvernoteContent(string text)
        {
            return "<?xml version=\"1.0\" encoding=\"UTF-8\"?>"
                   + "<!DOCTYPE en-note SYSTEM \"http://xml.evernote.com/pub/enml2.dtd\">" + "<en-note>" + text
                   + "</en-note>";
        }

        /// <summary>
        /// This function crates a new Note with text and resource3
        /// </summary>
        /// <param name="notebookGuid">
        /// The notebook guid of the notebook of  the year
        /// </param>
        /// <param name="text">
        /// The text.
        /// </param>
        /// <param name="resource">
        /// The resource.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note CreateNewNoteWithText(string notebookGuid, string text, Resource resource)
        {
            var noteContent = this.CreateEvernoteContent(text);

            var note = new Note
                           {
                               Title = this.timeHelper.GetCurrentDateInLocaleFormat(),
                               Content = noteContent,
                               ContentLength = noteContent.Length
                           };

            if (resource != null)
            {
                note.Resources = new List<Resource> { resource };
            }
        
            // Create note in the store
            this.evernoteAccess.CreateNote(note);
            return note;
        }

    public Note UpdateNoteWithText(Note note, string text, Resource resource )
    {
        note.Content = CreateEvernoteContent(text);
        
        if (resource != null)
        {
            if (note.Resources == null)
            {
                note.Resources = new List<Resource> ();
            }
            note.Resources.Add(resource);
        }

        // Create note in the store
        this.evernoteAccess.UpdateNote(note);
        return note;
    }
    

    
    public func readNoteBookOfTheYear(readFunction:(String)->Void)
    {
        let currentYear = timeHelper.getCurrentYear()

        var notebookGuid = ""
        
        everNoteAccess.listNotebooksWithSuccess(
            { (notebooks) -> Void in
                for n:AnyObject in notebooks
                {
                    let notebook = n as EDAMNotebook
                    NSLog("Examine notebook %@", notebook.name)
                    
                    if (notebook.name == self.timeHelper.getCurrentYear())
                    {
                        readFunction(notebook.guid)
                        return
                    }
                }
   
                let currentTimeStamp = NSDate().enedamTimestamp()
                
                // No Notebook of the year found. Create it.
                
                let notebook = EDAMNotebook(guid: nil, name: currentYear,
                    updateSequenceNum: 0, defaultNotebook: false, serviceCreated: currentTimeStamp, serviceUpdated: currentTimeStamp, publishing: nil, published: false, stack: nil, sharedNotebookIds: nil, sharedNotebooks: nil, businessNotebook: nil, contact: nil, restrictions: nil   )
                
                self.everNoteAccess.createNotebook(notebook, success:
                    {
                        (newNotebook) -> Void in
                        readFunction(newNotebook.guid)
                    },
                    failure:
                    {
                        (error) -> Void in
                        NSLog("Error creating notebook: %@", error)
                    }
                )
             },
            {
                (error) -> Void in
                NSLog("Error: %@", error.description)
            })
    }
    
    func readExistingNote(notebookGuid:String, note:EDAMNote, readFunction:(String, EDAMNote?, (ennoteAttributes:String!, content:String))->Void)
    {
        self.everNoteAccess.getNoteContentWithGuid(note.guid,
            success:
            {
                (content) -> Void in
                let contentHelper = EverNoteContentHelper()
                
                let noteContent = contentHelper.extractNoteText(content)
                NSLog("content read: %@", content)
                readFunction(notebookGuid, note, noteContent)
            },
            failure:
            {
                (error) -> Void in
                NSLog("error in getnotecontent %@", error )
            })
    }
    
    func readNoteOfTheCurrentDay(readFunction:(notebookGuid:String, EDAMNote?, (ennoteAttributes:String!, content:String))->Void)
    {
        readNoteBookOfTheYear({
            (notebookGuidOfTheYear) -> Void in
            
            let dateValue = "\"" + self.timeHelper.getCurrentDateInLocaleFormat() + "\""
            
            NSLog("Notebook of the year: %@", notebookGuidOfTheYear)
            NSLog("Note of the day: %@", dateValue)
            
            let noteFilter = EDAMNoteFilter(order: 0, ascending: false, words: dateValue, notebookGuid: notebookGuidOfTheYear, tagGuids: nil, timeZone: nil, inactive: false, emphasized: nil)
            let guidNote:String? = nil
            self.everNoteAccess.findNotesWithFilter(noteFilter, offset: 0, maxNotes: 1,
                success:
                { (
                    notelist) -> Void in
                    if (notelist.notes.count > 0)
                    {
                        let n:EDAMNote = notelist.notes[0] as EDAMNote
                        NSLog("Note count %d ", notelist.notes.count)
                        NSLog("Note title %@", n.title)
                        self.readExistingNote(notebookGuidOfTheYear, note: n, readFunction)
                    }
                    else
                    {
                        readFunction(notebookGuid: notebookGuidOfTheYear, nil, (nil,""))
                    }
                 },
                failure: {
                    (error) -> Void in
                    NSLog("Error: %@", error.description)
                } )
            })
    }
     
    public func appendJournalText(journalText: String, image: UIImage?, newFullNote: (guid:String, note:EDAMNote?, resource:EDAMResource?, fullnote:String)->Void)
    {
        readNoteOfTheCurrentDay({
            (notebookGuid, note, content) -> Void in
                let evernoteContentHelper = EverNoteContentHelper()
                var fullnote = content.content
                let time = self.timeHelper.getCurrentTimeInLocaleFormat()
                
                let lines = evernoteContentHelper.splitIntoLines(journalText)
                for line in lines
                {
                    let noteLine = self.substituteWithTemplate("paragraph", value: line)
                    NSLog("subst line : \(noteLine)")
                    fullnote += noteLine
                }
            
                var resource:EDAMResource? = nil
            
                if (image)
                {
                    resource = evernoteContentHelper.createResourceFromImage(image!)
                    fullnote += "<p>" + evernoteContentHelper.enMediaTagWithResource(resource!, width: 0, height: 0) + "</p>"
                }
                fullnote += self.substituteWithTemplate("timeDivider", value: time)
            
                newFullNote(guid: notebookGuid, note: note, resource: resource, fullnote: fullnote)
            })
    }
    
    public func saveJournalEntryToEvernote(journalText: String, image: UIImage?, success: (note:EDAMNote) -> Void, failure: (error:NSError) -> Void)
    {
        appendJournalText(journalText, image: image,
            {
                (notebookGuid, note, resource, fullText) -> Void in

                NSLog("Trying to save Journal entry: \(notebookGuid) - \(note) text: \(fullText)")
                if (note == nil)
                {
                    self.createNewNoteWithText(notebookGuid, resource: resource, text: fullText, image: image, success, failure)
                }
                else
                {
                    self.updateNoteWithText(note!, resource: resource, text: fullText, image: image, success, failure)
                }
            })
    }
         */
        }
    }