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

    using Evernote.EDAM.NoteStore;
    using Evernote.EDAM.Type;

    using MyJournalTracker.EverNoteSupport;
    using MyJournalTracker.Helper;
    using MyJournalTracker.Model;

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
            var template = EvernoteContentHelper.ReadTemplate(templateName);
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
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note SaveNote(Note note)
        {
            note.Title = this.timeHelper.GetCurrentDateInLocaleFormat();
            note.ContentLength = note.Content.Length;

            if (string.IsNullOrEmpty(note.Guid))
            {
                this.evernoteAccess.CreateNote(note);
            }
            else
            {
                this.evernoteAccess.UpdateNote(note);
            }

            return note;
        }

        /// <summary>
        /// The read note of the current day.
        /// </summary>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note ReadNoteOfTheCurrentDay()
        {
            var notebookGuid = this.RetrieveNoteBookGuidOfTheYear();
            var dateValue = "\"" + this.timeHelper.GetCurrentDateInLocaleFormat() + "\"";
            var noteFilter = new NoteFilter { NotebookGuid = notebookGuid, Ascending = false, Words = dateValue };

            var notes = this.evernoteAccess.FindNotes(noteFilter, 0, 1);
            if (notes.Notes.Count > 0)
            {
                var note = notes.Notes[0];
                note.Content = this.evernoteAccess.GetNoteContentWithGuid(note.Guid);
                return note;
            }

            return new Note();
        }

        /// <summary>
        /// The append journal entry.
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <param name="entry">
        /// The entry.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note AppendJournalEntry(Note note, Entry entry)
        {
            var lines = EvernoteContentHelper.SplitIntoLines(entry.EntryText);
            var noteTags = EvernoteContentHelper.SplitContentInTags(note);
            var fullnote = noteTags.ExtractedContent;

            foreach (var line in lines)
            {
                var noteLine = this.SubstituteWithTemplate("paragraph", line);
                fullnote += noteLine;
            }

            if (entry.EntryPicture != null)
            {
                var resource = EvernoteContentHelper.CreateResourceFromImage(entry.EntryPicture);
                if (note.Resources == null)
                {
                    note.Resources = new List<Resource>();
                }

                note.Resources.Add(resource);
                fullnote += "<p>" + EvernoteContentHelper.EnMediaTagWithResource(resource, 0, 0) + "</p>";
            }

            var time = new TimeHelper().GetCurrentTimeInLocaleFormat();
            fullnote += this.SubstituteWithTemplate("timeDivider", time);
            note.Content = this.CreateEvernoteContent(fullnote);

            return note;
        }

        /// <summary>
        /// The save journal entry to evernote.
        /// </summary>
        /// <param name="entry">
        /// The entry.
        /// </param>
        public void SaveJournalEntryToEvernote(Entry entry)
        {
            var note = this.ReadNoteOfTheCurrentDay();
            note = this.AppendJournalEntry(note, entry);
            this.SaveNote(note);
        }
    }
}