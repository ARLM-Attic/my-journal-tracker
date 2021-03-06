﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTestAccess.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests.Evernote
{
    using System;
    using System.Collections.Generic;

    using global::Evernote.EDAM.NoteStore;
    using global::Evernote.EDAM.Type;

    using MyJournalTracker.EverNoteSupport;

    /// <summary>
    /// The unit test access.
    /// </summary>
    public class UnitTestAccess : IAbstractEvernoteAccess
    {
        /// <summary>
        /// The list of simulated notebooks.
        /// </summary>
        /// <remarks>
        /// The list is empty. if you create a notebook via the <see cref="CreateNotebook"/> function
        /// a notebook will be added to the list.
        /// </remarks>
        private readonly List<Notebook> notebooks = new List<Notebook>();

        /// <summary>
        /// The (fake) list of notebooks for unit testing purposes
        /// </summary>
        /// <returns>
        /// a empty list.
        /// </returns>
        public List<Notebook> ListNotebooks()
        {
            return this.notebooks;
        }

        /// <summary>
        /// The create notebook.
        /// </summary>
        /// <param name="notebook">
        /// The notebook.
        /// </param>
        /// <returns>
        /// The <see cref="Notebook"/>.
        /// </returns>
        public Notebook CreateNotebook(Notebook notebook)
        {
            var newNotebook = notebook;
            if (notebook == null)
            {
                newNotebook = new Notebook();
            }

            newNotebook.Guid = Guid.NewGuid().ToString();
            this.notebooks.Add(newNotebook);

            return new Notebook();
        }

        /// <summary>
        /// simulate a note created in the EvernoteStore
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note CreateNote(Note note)
        {
            return note;
        }

        /// <summary>
        /// simulate a note updated in the EvernoteStore
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note UpdateNote(Note note)
        {
            return note;
        }

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
        public NoteList FindNotes(NoteFilter filter, int offset, int maxNotes)
        {
            return new NoteList();
        }

        /// <summary>
        /// The get note content with guid.
        /// </summary>
        /// <param name="guid">
        /// The guid.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        public string GetNoteContentWithGuid(string guid)
        {
            return "test content";
        }
    }
}