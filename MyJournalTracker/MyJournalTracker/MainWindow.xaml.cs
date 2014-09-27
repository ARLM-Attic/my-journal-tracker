// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="MainWindow.xaml.cs">
//   todo: license
// </copyright>
// <summary>
//   Interaction logic MainWindow.xaml
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
namespace MyJournalTracker
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media.Imaging;

    using Microsoft.Win32;

    using MyJournalTracker.Logic;
    using MyJournalTracker.Model;
    using MyJournalTracker.Utility;

    /// <summary>
    ///    Interaction logic MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Fields

        /// <summary>
        /// The controller for this class
        /// </summary>
        private readonly MainWindowController controller = new MainWindowController();

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            Entry.NotebookList = this.controller.RetrieveNotebookNames();
            this.InitializeComponent();
            this.TextBox.Focus();
            this.TextBox.SelectAll();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the edited entry.
        /// </summary>
        /// <value>
        /// The edited entry.
        /// </value>
        private Entry EditedEntry
        {
            get
            {
                return (Entry)this.DataContext;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// The execute attach picture to entry.
        /// </summary>
        private void ExecuteAttachPictureToEntry()
        {
            var od = new OpenFileDialog
                         {
                             Filter =
                                 "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
                         };
            var result = od.ShowDialog();
            if (result != true)
            {
                return;
            }

            this.EditedEntry.EntryPicture = new BitmapImage(new Uri(od.FileName));
        }

        /// <summary>
        /// The execute save entry.
        /// </summary>
        private void ExecuteSaveEntry()
        {
            if (!string.IsNullOrEmpty(this.notebookComboBox.Text)
                && string.IsNullOrEmpty(this.EditedEntry.EntryNotebookName))
            {
                MessageBoxResult messageBoxResult = MessageBox.Show(
                    "Do you want to create a new notebook with the title \"" + this.notebookComboBox.Text + "\"",
                    "Warning!",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);
                if (messageBoxResult == MessageBoxResult.No)
                {
                    this.notebookComboBox.Text = string.Empty;
                    return;
                }

                this.EditedEntry.EntryNotebookName = this.notebookComboBox.Text;
            }

            try
            {
                this.controller.CreateNewEntry((Entry)this.DataContext);
                
                // create new entry but save the old notebook name
                this.DataContext = new Entry { EntryNotebookName = this.EditedEntry.EntryNotebookName };
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "Error while saving", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// The execute snipping.
        /// </summary>
        private void ExecuteSnipping()
        {
            this.Hide();
            var image = SnippingTool.SnippingTool.Snip();
            this.Show();
            if (image == null)
            {
                return;
            }

            this.EditedEntry.EntryPicture = image.ToBitmapImage();
        }

        /// <summary>
        /// Handler for the Settings Button click event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void PictureButtonClick(object sender, RoutedEventArgs e)
        {
            this.PictureMouseUp(sender, null);
        }

        /// <summary>
        /// The picture mouse up.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void PictureMouseUp(object sender, MouseButtonEventArgs e)
        {
            this.ExecuteAttachPictureToEntry();
        }

        /// <summary>
        /// Handler for the Settings Button click event.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The <see cref="RoutedEventArgs"/> instance containing the event data.
        /// </param>
        private void SaveButtonClick(object sender, RoutedEventArgs e)
        {
            this.ExecuteSaveEntry();
        }

        /// <summary>
        /// The save entry.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SaveEntry(object sender, ExecutedRoutedEventArgs e)
        {
            this.ExecuteSaveEntry();
        }

        /// <summary>
        /// Create a picture with the snipping tool and good old Windows Forms
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SnipButtonClick(object sender, RoutedEventArgs e)
        {
            this.ExecuteSnipping();
        }

        /// <summary>
        /// The snip picture.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void SnipPicture(object sender, ExecutedRoutedEventArgs e)
        {
            this.ExecuteSnipping();
        }

        #endregion
    }
}