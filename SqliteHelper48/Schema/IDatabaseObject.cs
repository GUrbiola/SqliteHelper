using ICSharpCode.TextEditor.Gui.CompletionWindow;
using System.Collections.Generic;

namespace SqliteHelper48.Schema
{
    public interface IDatabaseObject : ICompletionData
    {
        string Id { get; set; }
        string Name { get; set; }
        DatabaseObjectType ObjectType { get; set; }
        string Script { get; set; }
        List<Field> Childs { get; set; }
        string NameOnSnippet { get; set; }
        bool UsesAlias { get; set; }

    }
}
