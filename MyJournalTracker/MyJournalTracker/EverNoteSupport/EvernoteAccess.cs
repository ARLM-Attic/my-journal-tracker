﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EvernoteAccess.cs" company="">
//   
// </copyright>
// <summary>
//   The evernote access.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.EverNoteSupport
{
    using System;
    using System.Collections.Generic;

    using Evernote.EDAM.NoteStore;
    using Evernote.EDAM.Type;
    using Evernote.EDAM.UserStore;

    using Thrift.Protocol;
    using Thrift.Transport;

    /// <summary>
    ///     The evernote access.
    /// </summary>
    public class EvernoteAccess : IAbstractEvernoteAccess
    {
        /// <summary>
        /// The url of the evernote host.
        /// </summary>
        /// <remarks>
        /// Initial development is performed on our sandbox server. To use the production 
        /// service, change "sandbox.evernote.com" to "www.evernote.com" and replace your
        /// developer token above with a token from 
        /// https://www.evernote.com/api/DeveloperToken.action
        /// </remarks>
        ///private const string EvernoteHostUrl = "sandbox.evernote.com";
        private const string EvernoteHostUrl = "www.evernote.com";

        /// <summary>
        /// The note store.
        /// </summary>
        private readonly NoteStore.Client noteStore;

        /// <summary>
        /// The oauth token to access the EverNote API
        /// </summary>
        private readonly string authToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvernoteAccess"/> class.
        /// </summary>
        /// <param name="authToken">
        /// The auth token.
        /// </param>
        /// <exception cref="EvernoteAccessException">
        /// An exception will be raise, if the version of the evernote api is outdated
        /// </exception>
        public EvernoteAccess(string authToken)
        {
            this.authToken = authToken;
            var userStoreUrl = new Uri("https://" + EvernoteHostUrl + "/edam/user");

            // var userStoreUrl = new Uri("https://www.evernote.com/shard/s9/notestore");
            TTransport userStoreTransport = new THttpClient(userStoreUrl);
            TProtocol userStoreProtocol = new TBinaryProtocol(userStoreTransport);
            var userStore = new UserStore.Client(userStoreProtocol);

            var versionOk = userStore.checkVersion(
                "Evernote EDAMTest (C#)",
                Evernote.EDAM.UserStore.Constants.EDAM_VERSION_MAJOR,
                Evernote.EDAM.UserStore.Constants.EDAM_VERSION_MINOR);
            
            if (!versionOk)
            {
                throw new EvernoteAccessException("the Evernote API version isn't up to date!");
            }

            // Get the URL used to interact with the contents of the user's account
            // When your application authenticates using OAuth, the NoteStore URL will
            // be returned along with the auth token in the final OAuth request.
            // In that case, you don't need to make this call.
            var noteStoreUrl = userStore.getNoteStoreUrl(authToken);

            TTransport noteStoreTransport = new THttpClient(new Uri(noteStoreUrl));
            TProtocol noteStoreProtocol = new TBinaryProtocol(noteStoreTransport);
            this.noteStore = new NoteStore.Client(noteStoreProtocol);
        }

        /// <summary>
        /// all notebooks from the evernote account.
        /// </summary>
        /// <returns>
        /// The List of Notebooks
        /// </returns>
        public List<Notebook> ListNotebooks()
        {
            return this.noteStore.listNotebooks(this.authToken);
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
            return this.noteStore.createNotebook(this.authToken, notebook);
        }

        /// <summary>
        /// The create note.
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note CreateNote(Note note)
        {
            return this.noteStore.createNote(this.authToken, note);
        }

        /// <summary>
        /// The update note.
        /// </summary>
        /// <param name="note">
        /// The note.
        /// </param>
        /// <returns>
        /// The <see cref="Note"/>.
        /// </returns>
        public Note UpdateNote(Note note)
        {
            return this.noteStore.updateNote(this.authToken, note);
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
            return this.noteStore.findNotes(this.authToken, filter, offset, maxNotes);
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
            return this.noteStore.getNoteContent(this.authToken, guid);
        }
    }
}