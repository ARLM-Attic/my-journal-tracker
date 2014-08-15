﻿// --------------------------------------------------------------------------------------------------------------------
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
    }
}