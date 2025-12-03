using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using ZidUtilities.CommonCode.Win;

namespace SqliteHelper48
{
    public partial class RowEditorDialog : Form
    {
        private readonly List<TableColumnInfo> columnInfo;
        private readonly DataRow? existingRow;
        private readonly Dictionary<string, Control> columnControls = new Dictionary<string, Control>();
        private System.ComponentModel.IContainer? components = null;
        private ZidUtilities.CommonCode.Win.Controls.ThemeManager themeManager1 = null!;

        public Dictionary<string, object?> Values { get; private set; } = new Dictionary<string, object?>();

        public RowEditorDialog(List<TableColumnInfo> columnInfo, DataRow? existingRow = null)
        {
            this.columnInfo = columnInfo;
            this.existingRow = existingRow;

            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            Text = existingRow == null ? "Add New Row" : "Edit Row";
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Count editable columns (exclude autoincrement)
            var editableColumns = columnInfo.Where(c => !c.IsAutoIncrement).ToList();
            int editableCount = editableColumns.Count;

            // Calculate dynamic size based on number of fields
            int rowHeight = 35;
            int headerHeight = 40;
            int buttonHeight = 50;
            int padding = 20;
            int maxHeight = 600;

            int calculatedHeight = headerHeight + (editableCount * rowHeight) + buttonHeight + padding;
            Size = new Size(600, Math.Min(calculatedHeight, maxHeight));

            var mainPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                AutoSize = true,
                Padding = new Padding(10)
            };

            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180));
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            var scrollPanel = new Panel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true
            };

            int rowIndex = 0;
            foreach (var column in columnInfo)
            {
                // Skip autoincrement columns
                if (column.IsAutoIncrement)
                    continue;

                mainPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, rowHeight));

                // Label
                var labelText = column.Name;
                if (column.IsNotNull)
                    labelText += " *";
                if (column.IsPrimaryKey && !column.IsAutoIncrement)
                    labelText += " (PK)";

                var label = new Label
                {
                    Text = labelText + ":",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(3),
                    AutoSize = false
                };
                mainPanel.Controls.Add(label, 0, rowIndex);

                // Input control based on data type
                Control inputControl = CreateControlForType(column);
                columnControls[column.Name] = inputControl;
                mainPanel.Controls.Add(inputControl, 1, rowIndex);

                rowIndex++;
            }

            scrollPanel.Controls.Add(mainPanel);

            // Button panel
            var buttonPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Height = 50,
                Padding = new Padding(10)
            };

            var btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Width = 75,
                Height = 30
            };

            var btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Width = 75,
                Height = 30
            };
            btnOK.Click += BtnOK_Click;

            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnOK);

            Controls.Add(scrollPanel);
            Controls.Add(buttonPanel);

            AcceptButton = btnOK;
            CancelButton = btnCancel;
        }

        private Control CreateControlForType(TableColumnInfo column)
        {
            object? existingValue = null;
            if (existingRow != null && existingRow.Table.Columns.Contains(column.Name))
            {
                existingValue = existingRow[column.Name];
                if (existingValue == DBNull.Value)
                    existingValue = null;
            }

            // Create appropriate control based on data type
            if (column.DataType == typeof(bool))
            {
                var checkBox = new CheckBox
                {
                    Dock = DockStyle.Left,
                    Margin = new Padding(3),
                    Checked = existingValue != null && Convert.ToBoolean(existingValue)
                };
                return checkBox;
            }
            else if (column.DataType == typeof(long) || column.DataType == typeof(int))
            {
                var numericUpDown = new NumericUpDown
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3),
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    DecimalPlaces = 0,
                    ThousandsSeparator = true
                };

                if (existingValue != null)
                {
                    try
                    {
                        numericUpDown.Value = Convert.ToDecimal(existingValue);
                    }
                    catch { }
                }

                return numericUpDown;
            }
            else if (column.DataType == typeof(double) || column.DataType == typeof(decimal) || column.DataType == typeof(float))
            {
                var numericUpDown = new NumericUpDown
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3),
                    Minimum = decimal.MinValue,
                    Maximum = decimal.MaxValue,
                    DecimalPlaces = 4,
                    ThousandsSeparator = true
                };

                if (existingValue != null)
                {
                    try
                    {
                        numericUpDown.Value = Convert.ToDecimal(existingValue);
                    }
                    catch { }
                }

                return numericUpDown;
            }
            else if (column.DataType == typeof(DateTime))
            {
                var dateTimePicker = new DateTimePicker
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3),
                    Format = DateTimePickerFormat.Custom,
                    CustomFormat = "yyyy-MM-dd HH:mm:ss",
                    ShowCheckBox = !column.IsNotNull
                };

                if (existingValue != null)
                {
                    try
                    {
                        dateTimePicker.Value = Convert.ToDateTime(existingValue);
                        dateTimePicker.Checked = true;
                    }
                    catch { }
                }
                else
                {
                    dateTimePicker.Checked = false;
                }

                return dateTimePicker;
            }
            else if (column.DataType == typeof(byte[]))
            {
                // For BLOB, use a text box with hex input
                var textBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3)
                    // PlaceholderText is not supported in .NET Framework 4.8
                    // Placeholder: "Enter hex string (e.g., 48656C6C6F)"
                };

                if (existingValue is byte[] bytes)
                {
                    textBox.Text = BitConverter.ToString(bytes).Replace("-", "");
                }

                return textBox;
            }
            else
            {
                // Default to TextBox for strings and other types
                var textBox = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(3)
                    // PlaceholderText is not supported in .NET Framework 4.8
                    // Placeholder: column.IsNotNull ? "Required" : "Optional"
                };

                if (existingValue != null)
                {
                    textBox.Text = existingValue.ToString();
                }

                return textBox;
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            Values.Clear();

            foreach (var column in columnInfo)
            {
                // Skip autoincrement columns
                if (column.IsAutoIncrement)
                    continue;

                if (!columnControls.ContainsKey(column.Name))
                    continue;

                var control = columnControls[column.Name];
                object? value = null;

                try
                {
                    if (control is CheckBox checkBox)
                    {
                        value = checkBox.Checked;
                    }
                    else if (control is NumericUpDown numericUpDown)
                    {
                        value = numericUpDown.Value;

                        // Convert to appropriate type
                        if (column.DataType == typeof(long))
                            value = Convert.ToInt64(numericUpDown.Value);
                        else if (column.DataType == typeof(int))
                            value = Convert.ToInt32(numericUpDown.Value);
                        else if (column.DataType == typeof(double))
                            value = Convert.ToDouble(numericUpDown.Value);
                        else if (column.DataType == typeof(float))
                            value = Convert.ToSingle(numericUpDown.Value);
                    }
                    else if (control is DateTimePicker dateTimePicker)
                    {
                        if (dateTimePicker.ShowCheckBox && !dateTimePicker.Checked)
                        {
                            value = DBNull.Value;
                        }
                        else
                        {
                            value = dateTimePicker.Value;
                        }
                    }
                    else if (control is TextBox textBox)
                    {
                        if (string.IsNullOrWhiteSpace(textBox.Text))
                        {
                            if (column.IsNotNull)
                            {
                                MessageBox.Show($"Field '{column.Name}' is required and cannot be empty.",
                                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                DialogResult = DialogResult.None;
                                return;
                            }
                            value = DBNull.Value;
                        }
                        else
                        {
                            // Special handling for BLOB (byte array)
                            if (column.DataType == typeof(byte[]))
                            {
                                try
                                {
                                    // Convert hex string to byte array
                                    string hex = textBox.Text.Replace(" ", "").Replace("-", "");
                                    if (hex.Length % 2 != 0)
                                        throw new FormatException("Hex string must have even length");

                                    byte[] bytes = new byte[hex.Length / 2];
                                    for (int i = 0; i < bytes.Length; i++)
                                    {
                                        bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
                                    }
                                    value = bytes;
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show($"Invalid hex string for field '{column.Name}': {ex.Message}",
                                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    DialogResult = DialogResult.None;
                                    return;
                                }
                            }
                            else
                            {
                                value = textBox.Text;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error processing field '{column.Name}': {ex.Message}",
                        "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    DialogResult = DialogResult.None;
                    return;
                }

                Values[column.Name] = value;
            }
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            themeManager1 = new ZidUtilities.CommonCode.Win.Controls.ThemeManager(components);
            SuspendLayout();
            //
            // themeManager1
            //
            themeManager1.ParentForm = this;
            //
            // RowEditorDialog
            //
            ClientSize = new Size(600, 400);
            Name = "RowEditorDialog";
            Load += RowEditorDialog_Load;
            ResumeLayout(false);
        }

        private void RowEditorDialog_Load(object? sender, EventArgs e)
        {
            string savedTheme = Properties.Settings.Default.SelectedTheme;
            themeManager1.Theme = Enum.TryParse<ZidThemes>(savedTheme, out var theme) ? theme : ZidThemes.None;
            themeManager1.ApplyTheme();
        }
    }
}
