using System;
using System.Drawing;
using System.Windows.Forms;

namespace SqliteHelper48
{
    public class CommentControl : UserControl
    {
        private Panel dragPanel = null!;
        private Label titleLabel = null!;
        private TextBox textBox = null!;
        private Panel resizeGrip = null!;
        private ContextMenuStrip contextMenu = null!;
        public DiagramComment Comment { get; private set; }
        private const int ResizeGripSize = 16;
        private const int DragPanelHeight = 24;
        private bool isResizing = false;
        private bool isDragging = false;
        private Point resizeStartPoint;
        private Size resizeStartSize;
        private Point dragStartPoint;

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

            // Create drag panel (title bar)
            dragPanel = new Panel
            {
                Height = DragPanelHeight,
                Dock = DockStyle.Top,
                BackColor = Color.FromArgb(255, 250, 205), // Lighter yellow
                Cursor = Cursors.SizeAll
            };

            // Create title label in drag panel
            titleLabel = new Label
            {
                Text = "Comment",
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0),
                Font = new Font("Segoe UI", 8.25f, FontStyle.Bold),
                ForeColor = Color.DarkGoldenrod
            };

            dragPanel.Controls.Add(titleLabel);

            // Wire up drag events for both panel and label
            dragPanel.MouseDown += DragPanel_MouseDown;
            dragPanel.MouseMove += DragPanel_MouseMove;
            dragPanel.MouseUp += DragPanel_MouseUp;
            titleLabel.MouseDown += DragPanel_MouseDown;
            titleLabel.MouseMove += DragPanel_MouseMove;
            titleLabel.MouseUp += DragPanel_MouseUp;

            // Create text box - will fill remaining space below drag panel
            textBox = new TextBox
            {
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.None,
                BackColor = Color.LightYellow,
                Location = new Point(2, DragPanelHeight + 2),
                Size = new Size(Width - 4, Height - DragPanelHeight - 4 - ResizeGripSize),
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
            dragPanel.ContextMenuStrip = contextMenu;

            // Add controls in correct order: dragPanel first (top), then textBox, then resizeGrip (front)
            Controls.Add(dragPanel);
            Controls.Add(textBox);
            Controls.Add(resizeGrip);

            resizeGrip.BringToFront();
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

        private void DragPanel_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = e.Location;
                dragPanel.Cursor = Cursors.Hand;
            }
        }

        private void DragPanel_MouseMove(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentLocation = Location;
                currentLocation.Offset(e.Location.X - dragStartPoint.X, e.Location.Y - dragStartPoint.Y);
                Location = currentLocation;
            }
        }

        private void DragPanel_MouseUp(object? sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                isDragging = false;
                dragPanel.Cursor = Cursors.SizeAll;
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
