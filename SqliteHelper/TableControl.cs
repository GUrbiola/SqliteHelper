namespace SqliteHelper
{
    public class TableControl : UserControl
    {
        public TableDefinition TableDefinition { get; private set; }
        private const int HeaderHeight = 30;
        private const int RowHeight = 25;
        private const int MinWidth = 200;

        public TableControl(TableDefinition table)
        {
            TableDefinition = table;
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
            Cursor = Cursors.SizeAll;
            UpdateSize();
        }

        public void UpdateTable(TableDefinition table)
        {
            TableDefinition = table;
            UpdateSize();
            Invalidate();
        }

        private void UpdateSize()
        {
            int width = MinWidth;
            int height = HeaderHeight + (TableDefinition.Columns.Count * RowHeight) + 10;

            using (Graphics g = CreateGraphics())
            {
                foreach (var column in TableDefinition.Columns)
                {
                    string columnText = GetColumnDisplayText(column);
                    int textWidth = (int)g.MeasureString(columnText, Font).Width + 20;
                    width = Math.Max(width, textWidth);
                }
            }

            Size = new Size(width, height);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            Graphics g = e.Graphics;
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            // Draw border
            using (Pen borderPen = new Pen(Color.Black, 2))
            {
                g.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }

            // Draw header
            using (SolidBrush headerBrush = new SolidBrush(Color.SteelBlue))
            {
                g.FillRectangle(headerBrush, 1, 1, Width - 2, HeaderHeight);
            }

            // Draw table name
            using (Font headerFont = new Font(Font.FontFamily, 10, FontStyle.Bold))
            using (SolidBrush textBrush = new SolidBrush(Color.White))
            {
                StringFormat sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };
                g.DrawString(TableDefinition.Name, headerFont, textBrush, new Rectangle(0, 0, Width, HeaderHeight), sf);
            }

            // Draw columns
            int y = HeaderHeight + 5;
            using (SolidBrush textBrush = new SolidBrush(Color.Black))
            {
                foreach (var column in TableDefinition.Columns)
                {
                    string columnText = GetColumnDisplayText(column);
                    g.DrawString(columnText, Font, textBrush, 10, y);
                    y += RowHeight;
                }
            }
        }

        private string GetColumnDisplayText(ColumnDefinition column)
        {
            string text = $"{column.Name} : {column.DataType}";

            List<string> attributes = new List<string>();
            if (column.IsPrimaryKey) attributes.Add("PK");
            if (column.IsAutoIncrement) attributes.Add("AI");
            if (column.IsNotNull) attributes.Add("NN");

            if (attributes.Count > 0)
            {
                text += $" [{string.Join(", ", attributes)}]";
            }

            return text;
        }
    }
}
