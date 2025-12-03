using SqliteHelper48.Schema;
using System;
using System.Collections.Generic;

namespace SqliteHelper48.Schema
{
    internal class Alias : IDatabaseObject
    {
        public string AliasedObject { get; set; } = string.Empty;
        public string AliasText { get; set; } = string.Empty;
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public DatabaseObjectType ObjectType { get; set; } = DatabaseObjectType.Table;
        public string Script { get; set; } = string.Empty;
        public List<Field> Childs { get; set; } = new List<Field>();

        public string Description
        {
            get { return String.Format("Alias: {0}", Name); }
        }
        public int ImageIndex
        {
            get { return 8; }
        }
        public bool InsertAction(ICSharpCode.TextEditor.TextArea textArea, char ch)
        {
            textArea.InsertString(String.Format("{0}", Name));
            return false;
        }
        public double Priority
        {
            get { return 1.0; }
        }
        public string Text
        {
            get { return Name; }
            set { Name = value; }
        }

        public string NameOnSnippet { get; set; } = string.Empty;
        public bool UsesAlias { get; set; } = false;
    }
}