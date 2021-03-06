﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="UTF8StringWriter.cs">
//   todo: license
// </copyright>
// <summary>
//   A convenient subclass of <see cref="System.IO.StringWriter" /> which uses UTF-8 encoding by default.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker.Utility
{
    using System;
    using System.IO;
    using System.Text;

    /// <summary>
    /// A convenient subclass of <see cref="System.IO.StringWriter"/> which uses UTF-8 encoding by default.
    /// </summary>
    [Serializable]
    internal class UTF8StringWriter : StringWriter
    {
        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="UTF8StringWriter"/> class.
        /// </summary>
        public UTF8StringWriter()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UTF8StringWriter"/> class.
        /// </summary>
        /// <param name="formatProvider">
        /// An <see cref="T:MyJournalTracker.Utility.UTF8StringWriter"/> object that controls formatting.
        /// </param>
        public UTF8StringWriter(IFormatProvider formatProvider)
            : base(formatProvider)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UTF8StringWriter"/> class.
        /// </summary>
        /// <param name="sb">
        /// The StringBuilder to write to.
        /// </param>
        public UTF8StringWriter(StringBuilder sb)
            : base(sb)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UTF8StringWriter"/> class.
        /// </summary>
        /// <param name="sb">
        /// The StringBuilder to write to.
        /// </param>
        /// <param name="formatProvider">
        /// An <see cref="T:MyJournalTracker.Utility.UTF8StringWriter"/> object that controls formatting.
        /// </param>
        public UTF8StringWriter(StringBuilder sb, IFormatProvider formatProvider)
            : base(sb, formatProvider)
        {
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="T:System.Text.Encoding" /> in which the output is written.
        /// </summary>
        /// <returns>The Encoding in which the output is written.</returns>
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }

        #endregion
    }
}