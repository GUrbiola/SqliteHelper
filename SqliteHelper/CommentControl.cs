namespace SqliteHelper
{
    public class CommentControl : UserControl
    {
        private TextBox textBox = null!;
        private Panel resizeGrip = null!;
        private ContextMenuStrip contextMenu = null!;
        public DiagramComment Comment { get; private set; }
        private const int ResizeGripSize = 16;
        private bool isResizing = false;
        private Point resizeStartPoint;
        private Size resizeStartSize;

        public event EventHandler? DeleteRequested;

        public CommentControl(DiagramComment comment)
        {
            Comment = comment;
            InitializeControl();
        }

        private void InitializeControl()
        {
            Size = Comment.Size;
            Location = Comment.Position;
            BackColor = Color.LightYellow;
            BorderStyle = BorderStyle.FixedSingle;
            MinimumSize = new Size(100, 60);

            // Create text box with margin for resize grip
            textBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                BackColor = Color.LightYellow,
                Location = new Point(2, 2),
                Size = new Size(Width - 4, Height - 4 - ResizeGripSize),
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom,
                Text = Comment.Text
            };

            textBox.TextChanged += (s, e) => Comment.Text = textBox.Text;
            textBox.KeyDown += TextBox_KeyDown;

            // Create resize grip
            resizeGrip = new Panel
            {
                Size = new Size(ResizeGripSize, ResizeGripSize),
                BackColor = Color.Gray,
                Cursor = Cursors.SizeNWSE,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            resizeGrip.Location = new Point(Width - ResizeGripSize - 1, Height - ResizeGripSize - 1);
            resizeGrip.MouseDown += ResizeGrip_MouseDown;
            resizeGrip.MouseMove += ResizeGrip_MouseMove;
            resizeGrip.MouseUp += ResizeGrip_MouseUp;
            resizeGrip.Paint += ResizeGrip_Paint;

            // Create context menu
            contextMenu = new ContextMenuStrip();
            var deleteItem = new ToolStripMenuItem("Delete Comment");
            deleteItem.Click += (s, e) => DeleteRequested?.Invoke(this, EventArgs.Empty);
            contextMenu.Items.Add(deleteItem);

            ContextMenuStrip = contextMenu;
            textBox.ContextMenuStrip = contextMenu;

            Controls.Add(textBox);
            Controls.Add(resizeGrip);
        }

        private void ResizeGrip_Paint(object? sender, PaintEventArgs e)
        {
            // Draw grip lines
            using (Pen pen = new Pen(Color.DarkGray, 2))
            {
                e.Graphics.DrawLine(pen, ResizeGripSize - 4, ResizeGripSize - 4, ResizeGripSize - 4, ResizeGripSize - 4);
                e.Graphics.DrawLine(pen, ResizeGripSize - 8, ResizeGripSize - 4, ResizeGripSize - 4, ResizeGripSize - 8);
                e.Graphics.DrawLine(pen, ResizeGripSize - 12, ResizeGripSize - 4, ResizeGripSize - 4, ResizeGripSize - 12);
            }
        }

        private void ResizeGrip_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isResizing = true;
                resizeStartPoint = PointToScreen(e.Location);
                resizeStartSize = Size;
                Cursor = Cursors.SizeNWSE;
            }
        }

        private void ResizeGrip_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                Point currentPoint = PointToScreen(e.Location);
                int deltaX = currentPoint.X - resizeStartPoint.X;
                int deltaY = currentPoint.Y - resizeStartPoint.Y;

                int newWidth = Math.Max(MinimumSize.Width, resizeStartSize.Width + deltaX);
                int newHeight = Math.Max(MinimumSize.Height, resizeStartSize.Height + deltaY);

                Size = new Size(newWidth, newHeight);
            }
        }

        private void ResizeGrip_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                isResizing = false;
                Cursor = Cursors.Default;
                UpdateComment();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            if (!isResizing)
                Cursor = Cursors.SizeAll;
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            if (!isResizing)
                Cursor = Cursors.Default;
        }

        private void TextBox_KeyDown(object? sender, KeyEventArgs e)
        {
            // Allow Ctrl+Delete to delete the comment
            if (e.Control && e.KeyCode == Keys.Delete)
            {
                DeleteRequested?.Invoke(this, EventArgs.Empty);
                e.Handled = true;
            }
        }

        public void UpdateComment()
        {
            Comment.Text = textBox.Text;
            Comment.Position = Location;
            Comment.Size = Size;
        }
    }
}
