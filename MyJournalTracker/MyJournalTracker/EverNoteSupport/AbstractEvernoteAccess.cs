// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AbstractEvernoteAccess.cs" company="">
//   
// </copyright>
// <summary>
//   The abstract evernote access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.EverNoteSupport
{
    /// <summary>
    /// The abstract evernote access.
    /// </summary>
    public interface IAbstractEvernoteAccess
    {
        /*
        func listNotebooksWithSuccess( success: ((notebooks:[AnyObject]!) -> Void)?, failure:((error:NSError!) -> Void)?)
        func createNotebook(notebook:EDAMNotebook?, success:((notebook:EDAMNotebook!) -> Void)?, failure:((error:NSError!)-> Void)?)
        func createNote(note:EDAMNote?, success:((note:EDAMNote!)->Void)?, failure:((error:NSError!)->Void)?)
        func findNotesWithFilter(filter: EDAMNoteFilter?, offset: Int32, maxNotes: Int32, success: ((EDAMNoteList!) -> Void)?, failure: ((NSError!) -> Void)?)
    
        func getNoteContentWithGuid(guid: String?, success: ((String!) -> Void)?, failure: ((NSError!) -> Void)?)
        func updateNote(note: EDAMNote?, success: ((EDAMNote!) -> Void)?, failure: ((NSError!) -> Void)?)
        */
    }
}