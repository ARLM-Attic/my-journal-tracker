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
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    using Evernote.EDAM.NoteStore;
    using Evernote.EDAM.Type;

    /// <summary>
    /// The abstract evernote access.
    /// </summary>
    public interface IAbstractEvernoteAccess
    {
        /// <summary>
        /// all notebooks from the evernote account.
        /// </summary>
        /// <returns>
        /// The List of Notebooks
        /// </returns>
        List<Notebook> ListNotebooks();

        /// <summary>
        /// The create notebook.
        /// </summary>
        /// <param name="notebook">
        /// The notebook.
        /// </param>
        /// <returns>
        /// The <see cref="Notebook"/>.
        /// </returns>
        Notebook CreateNotebook(Notebook notebook);

        /*
        func listNotebooksWithSuccess( success: ((notebooks:[AnyObject]!) -> Void)?, failure:((error:NSError!) -> Void)?)
        func createNotebook(notebook:EDAMNotebook?, success:((notebook:EDAMNotebook!) -> Void)?, failure:((error:NSError!)-> Void)?)
        func createNote(note:EDAMNote?, success:((note:EDAMNote!)->Void)?, failure:((error:NSError!)->Void)?)
        func findNotesWithFilter(filter: EDAMNoteFilter?, offset: Int32, maxNotes: Int32, success: ((EDAMNoteList!) -> Void)?, failure: ((NSError!) -> Void)?)
    
        func getNoteContentWithGuid(guid: String?, success: ((String!) -> Void)?, failure: ((NSError!) -> Void)?)
        func updateNote(note: EDAMNote?, success: ((EDAMNote!) -> Void)?, failure: ((NSError!) -> Void)?)
        */

        /// <summary>
        /// Create an new Note
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The newly created <see cref="Note"/>.
        /// </returns>
        Note CreateNote(Note note);

        /// <summary>
        /// The update note.
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        Note UpdateNote(Note note);

        /// <summary>
        /// The find notes.
        /// </summary>
        /// <param name="filter">
        /// The filter.
        /// </param>
        /// <param name="offset">
        /// The offset.
        /// </param>
        /// <param name="maxNotes">
        /// The max notes.
        /// </param>
        /// <returns>
        /// The <see cref="NoteList"/>.
        /// </returns>
        NoteList FindNotes(NoteFilter filter, int offset, int maxNotes);

        /// <summary>
        /// The get note content with guid.
        /// </summary>
        /// <param name="guid">
        /// The guid.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        string GetNoteContentWithGuid(string guid);
    }
}