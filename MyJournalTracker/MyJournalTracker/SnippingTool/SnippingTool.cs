// --------------------------------------------------------------------------------------------------------------------
// <copyright company="André Claaßen" file="SnippingTool.cs">
//   todo: license
// </copyright>
// <summary>
//   The snipping form.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace MyJournalTracker.SnippingTool
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Windows.Forms;

    /// <summary>
    /// The snipping form.
    /// </summary>
    /// <remarks>
    /// code copied from stackoverflow
    /// http://stackoverflow.com/questions/4005910/make-net-snipping-tool-compatible-with-multiple-monitors
    /// </remarks>
    public sealed partial class SnippingTool : Form
    {
        #region Static Fields

        /// <summary>
        /// The bitmap size.
        /// </summary>
        private static Size bitmapSize;

        /// <summary>
        /// The graph.
        /// </summary>
        private static Graphics graph;

        /// <summary>
        /// The screen, where I snipping from
        /// </summary>
        private static Screen screen;

        #endregion

        #region Fields

        /// <summary>
        /// The rectangle for selection
        /// </summary>
        private Rectangle selectionRectangle;

        /// <summary>
        /// The starting Point 
        /// </summary>
        private Point startingPoint;

        #endregion

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SnippingTool"/> class.
        /// </summary>
        /// <param name="screenShot">
        /// The screen shot.
        /// </param>
        public SnippingTool(Image screenShot)
        {
            this.BackgroundImage = screenShot;
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.None;
            this.DoubleBuffered = true;
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets or sets the image.
        /// </summary>
        public Image Image { get; set; }

        #endregion

        #region Public Methods and Operators

        /// <summary>
        /// The snipping progress
        /// </summary>
        /// <returns>
        /// The <see cref="Image"/>.
        /// </returns>
        public static Image Snip()
        {
            var multiScreenSize = FindMultiScreenSize();

            var bmp = new Bitmap(
                multiScreenSize.MaxRight - multiScreenSize.MinX,
                multiScreenSize.MaxBottom - multiScreenSize.MinY,
                System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
            var gr = Graphics.FromImage(bmp);

            graph = gr;

            gr.SmoothingMode = SmoothingMode.None;
            bitmapSize = bmp.Size;

            using (var snipper = new SnippingTool(bmp))
            {
                snipper.Location = new Point(multiScreenSize.MinX, multiScreenSize.MinY);
                snipper.StartPosition = FormStartPosition.Manual;

                if (snipper.ShowDialog() == DialogResult.OK)
                {
                    return snipper.Image;
                }
            }

            return null;
        }

        #endregion

        #region Methods

        /// <summary>
        /// The on load.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            var multiScreenSize = FindMultiScreenSize();
            this.Size = new Size(
                multiScreenSize.MaxRight - multiScreenSize.MinX, multiScreenSize.MaxBottom - multiScreenSize.MinY);

            graph.CopyFromScreen(multiScreenSize.MinX, multiScreenSize.MinY, 0, 0, bitmapSize);
        }

        /// <summary>
        /// The on mouse down.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            // Start the snip on mouse down'
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            this.startingPoint = e.Location;
            this.selectionRectangle = new Rectangle(e.Location, new Size(0, 0));
            this.Invalidate();
        }

        /// <summary>
        /// The on mouse move.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseMove(MouseEventArgs e)
        {
            // Modify the selection on mouse move'
            if (e.Button != MouseButtons.Left)
            {
                return;
            }

            var x1 = Math.Min(e.X, this.startingPoint.X);
            var y1 = Math.Min(e.Y, this.startingPoint.Y);
            var x2 = Math.Max(e.X, this.startingPoint.X);
            var y2 = Math.Max(e.Y, this.startingPoint.Y);
            this.selectionRectangle = new Rectangle(x1, y1, x2 - x1, y2 - y1);
            this.Invalidate();
        }

        /// <summary>
        /// The on mouse up.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnMouseUp(MouseEventArgs e)
        {
            // Complete the snip on mouse-up'
            if (this.selectionRectangle.Width <= 0 || this.selectionRectangle.Height <= 0)
            {
                return;
            }

            this.Image = new Bitmap(this.selectionRectangle.Width, this.selectionRectangle.Height);
            using (var gr = Graphics.FromImage(this.Image))
            {
                gr.DrawImage(
                    this.BackgroundImage,
                    new Rectangle(0, 0, this.Image.Width, this.Image.Height),
                    this.selectionRectangle,
                    GraphicsUnit.Pixel);
            }

            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// The on paint.
        /// </summary>
        /// <param name="e">
        /// The e.
        /// </param>
        protected override void OnPaint(PaintEventArgs e)
        {
            // Draw the current selection'
            using (Brush br = new SolidBrush(Color.FromArgb(120, Color.White)))
            {
                var x1 = this.selectionRectangle.X;
                var x2 = this.selectionRectangle.X + this.selectionRectangle.Width;
                var y1 = this.selectionRectangle.Y;
                var y2 = this.selectionRectangle.Y + this.selectionRectangle.Height;
                e.Graphics.FillRectangle(br, new Rectangle(0, 0, x1, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x2, 0, this.Width - x2, this.Height));
                e.Graphics.FillRectangle(br, new Rectangle(x1, 0, x2 - x1, y1));
                e.Graphics.FillRectangle(br, new Rectangle(x1, y2, x2 - x1, this.Height - y2));
            }

            using (var pen = new Pen(Color.Red, 3))
            {
                e.Graphics.DrawRectangle(pen, this.selectionRectangle);
            }
        }

        /// <summary>
        /// The processed command key
        /// </summary>
        /// <param name="msg">
        /// The windows message
        /// </param>
        /// <param name="keyData">
        /// The key data.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            // Allow canceling the snip with the Escape key'
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// The find multi screen size.
        /// </summary>
        /// <returns>
        /// The <see cref="MultiScreenSize"/>.
        /// </returns>
        private static MultiScreenSize FindMultiScreenSize()
        {
            var minX = Screen.AllScreens[0].Bounds.X;
            var minY = Screen.AllScreens[0].Bounds.Y;

            var maxRight = Screen.AllScreens[0].Bounds.Right;
            var maxBottom = Screen.AllScreens[0].Bounds.Bottom;

            foreach (var subscreen in Screen.AllScreens)
            {
                if (subscreen.Bounds.X < minX)
                {
                    minX = subscreen.Bounds.X;
                }

                if (subscreen.Bounds.Y < minY)
                {
                    minY = subscreen.Bounds.Y;
                }

                if (subscreen.Bounds.Right > maxRight)
                {
                    maxRight = subscreen.Bounds.Right;
                }

                if (subscreen.Bounds.Bottom > maxBottom)
                {
                    maxBottom = subscreen.Bounds.Bottom;
                }
            }

            var multiScreenSize = default(MultiScreenSize);
            multiScreenSize.MinX = minX;
            multiScreenSize.MinY = minY;
            multiScreenSize.MaxBottom = maxBottom;
            multiScreenSize.MaxRight = maxRight;
            return multiScreenSize;
        }

        #endregion

        /// <summary>
        /// The multi screen size.
        /// </summary>
        private struct MultiScreenSize
        {
            #region Fields

            /// <summary>
            /// The max bottom.
            /// </summary>
            public int MaxBottom;

            /// <summary>
            /// The max right.
            /// </summary>
            public int MaxRight;

            /// <summary>
            /// The min x.
            /// </summary>
            public int MinX;

            /// <summary>
            /// The min y.
            /// </summary>
            public int MinY;

            #endregion
        }
    }
}