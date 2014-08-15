// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UnitTestAccess.cs" company="André Claaßen">
//   todo: license
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.Tests.UnitTests.Evernote
{
    using System;
    using System.Collections.Generic;

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
    }
}