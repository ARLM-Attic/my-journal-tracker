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

        /*
            let everNoteAccess:AbstractEvernoteAccess
    let everNoteContentHelper = EverNoteContentHelper()
    let timeHelper = TimeHelper()
           
    public init()
    {
        everNoteAccess = EverNoteAccess()
    }
    
    public init(access:AbstractEvernoteAccess)
    {
        everNoteAccess = access
    }

    public func substituteWithTemplate(templateName:String, value:String) -> String
    {
        let template = self.everNoteContentHelper.readTemplate(templateName)
        return template.stringByReplacingOccurrencesOfString("%1", withString: value, options: NSStringCompareOptions.LiteralSearch, range: nil)
    }
    
    func createEvernoteContent(text:String) -> String
    {
        return
            "<?xml version=\"1.0\" encoding=\"UTF-8\"?>" +
            "<!DOCTYPE en-note SYSTEM \"http://xml.evernote.com/pub/enml2.dtd\">" +
            "<en-note>" +
            "\(text)" +
            "</en-note>"
    }
    
    func createNewNoteWithText(notebookGuid:String, resource:EDAMResource?, text:String, image:UIImage?, success: (note:EDAMNote) -> Void, failure: (error:NSError) -> Void)
    {
        // Create an EverNote note object
        
        let noteContent = createEvernoteContent(text)
        
        let note = EDAMNote(guid: nil, title: timeHelper.getCurrentDateInLocaleFormat(), content: noteContent, contentHash: nil, contentLength: CInt(countElements(noteContent)), created: 0, updated: 0, deleted: 0, active: true, updateSequenceNum: 0, notebookGuid: notebookGuid, tagGuids: nil, resources: nil, attributes: nil, tagNames: nil )
        
        
        if (resource)
        {
            if (!note.resources)
            {
                note.resources = NSMutableArray()
            }
            note.resources.addObject(resource)
        }
        
        
        // Create note in the store

        everNoteAccess.createNote(note,
            {
                (note) -> Void in
                NSLog("Note created: \(note.content)")
                success(note: note)
            },
            {
                (error) -> Void in
                NSLog("Error: %@", error.description)
                failure(error: error)
            })
    }

    func updateNoteWithText(note:EDAMNote, resource:EDAMResource?, text:String, image:UIImage?, success: (note:EDAMNote) -> Void, failure: (error:NSError) -> Void)
    {
        note.content = createEvernoteContent(text)
        
        NSLog("This content will be send to everote: \(note.content)")
        NSLog("title of the note: \(note.title)")
        
        if (resource)
        {
            if (!note.resources)
            {
                note.resources = NSMutableArray()
            }
            note.resources.addObject(resource)
        }
        
        everNoteAccess.updateNote(note,
            {
                (note) -> Void in
                NSLog("Note updated: \(note.content)")
                success(note: note)
            },
            {
                (error) -> Void in
                NSLog("Error: %@", error.description)
                failure(error: error)
            })
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