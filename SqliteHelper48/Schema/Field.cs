using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace SqliteHelper48.Schema
{
    public class Field : ICompletionData
    {
        public string Name { get; set; } = string.Empty;
        public SqliteFieldType FieldType { get; set; }
        public bool IsNullable { get; set; }
        public bool Increment { get; set; }
        public string? DefaultValue { get; set; }
        public bool IsKey { get; set; }


        public string Description { get { return String.Format("Table field: {0}", Name); } }
        public int ImageIndex { get { return 5; } }
        public bool InsertAction(ICSharpCode.TextEditor.TextArea textArea, char ch)
        {
            textArea.InsertString(String.Format("{0}", Name));
            return false;
        }
        public double Priority { get { return 2.0; } }
        public string Text
        {
            get { return Name; }
            set { Name = value; }
        }
    }
}
