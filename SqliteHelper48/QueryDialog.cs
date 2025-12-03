
using ICSharpCode.TextEditor;
using SqliteHelper48.Schema;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZidUtilities.CommonCode;
using ZidUtilities.CommonCode.ICSharpTextEditor;
using ZidUtilities.CommonCode.Win;
using ZidUtilities.CommonCode.Win.Forms;
using SchemaField = SqliteHelper48.Schema.Field;
using SchemaTable = SqliteHelper48.Schema.Table;
using SchemaView = SqliteHelper48.Schema.View;

namespace SqliteHelper48
{
    public partial class QueryDialog : Form
    {
        private readonly DatabaseManager databaseManager;

        public List<Schema.IDatabaseObject> DatabaseSchema { get; private set; } = new List<Schema.IDatabaseObject>();
        public List<Snippet> MySnippets { get; set; }

        //public QueryDialog()
        //{
        //    InitializeComponent();
        //}

        public QueryDialog(DatabaseManager databaseManager)
        {
            InitializeComponent();
            this.databaseManager = databaseManager;
        }

        private void QueryDialog_Load(object? sender, EventArgs e)
        {
            string savedTheme = Properties.Settings.Default.SelectedTheme;
            themeManager1.Theme = Enum.TryParse<ZidThemes>(savedTheme, out var theme) ? theme : ZidThemes.None;
            themeManager1.ApplyTheme();

            InitializeGrid();
            InitializeTextEditor();
            LoadDatabaseSchema();
            LoadSnippets();

            extendedEditor1.WhenKeyPress += OnEditorKeyPress;
        }

        private void LoadSnippets()
        {
            try
            {
                // Ensure list exists
                MySnippets = new List<Snippet>();

                // Obtain the ResourceSet for the current UI culture
                var resourceSet = Snippets.ResourceManager.GetResourceSet(System.Globalization.CultureInfo.CurrentUICulture, true, true);
                if (resourceSet == null)
                    return;

                // Iterate entries
                foreach (System.Collections.DictionaryEntry entry in resourceSet)
                {
                    try
                    {
                        if (!(entry.Key is string key))
                            continue;

                        string? script = null;
                        var value = entry.Value;

                        // If the resource is already a string, use it
                        if (value is string s)
                        {
                            script = s;
                        }
                        // If it's a byte array, assume UTF8 text
                        else if (value is byte[] bytes)
                        {
                            try
                            {
                                script = System.Text.Encoding.UTF8.GetString(bytes);
                            }
                            catch
                            {
                                // fallback: try default encoding
                                script = System.Text.Encoding.Default.GetString(bytes);
                            }
                        }
                        // If it's a stream (e.g., UnmanagedMemoryStream), read it
                        else if (value is System.IO.Stream stream)
                        {
                            try
                            {
                                // Ensure stream position is at start
                                if (stream.CanSeek)
                                    stream.Position = 0;
                                using (var reader = new System.IO.StreamReader(stream, System.Text.Encoding.UTF8, true, 1024, leaveOpen: true))
                                {
                                    script = reader.ReadToEnd();
                                }
                            }
                            catch
                            {
                                // ignore unreadable streams
                                script = null;
                            }
                        }
                        else
                        {
                            // Other resource types (images, icons, etc.) are skipped for text snippets
                            continue;
                        }

                        if (string.IsNullOrEmpty(script))
                            continue;

                        script = script
                            .Replace("type=\"Models.Snippets.Snippet\"", "type=\"SqliteHelper48.Snippet\"") // Normalize line endings
                            .Replace("assembly=\"Models,", "assembly=\"SqliteHelper48,");

                        MySnippets.Add((Snippet)script.DeserializeFromXmlString());
                    }
                    catch
                    {
                        // Skip problematic resource entry and continue
                        continue;
                    }
                }
            }
            catch
            {
                // Top-level safety: if anything goes wrong, ensure MySnippets is at least an empty list
                MySnippets = MySnippets ?? new List<Snippet>();
            }
        }

        #region Methods to handle autocomplete and intellisense
        /// <summary>
        /// Gets a value indicating whether IntelliSense is currently active.
        /// </summary>
        private bool IsIntellisenseOn { get { return ((AutocompleteDialog != null) && (!AutocompleteDialog.IsDisposed)); } }
        /// <summary>
        /// Represents the dialog window used for providing autocomplete suggestions.
        /// </summary>
        /// <remarks>This window is typically displayed to assist users in selecting from a list of
        /// suggestions based on their input. It is intended for internal use within the application.</remarks>
        private AutoCompleteWindow AutocompleteDialog;
        /// <summary>
        /// Represents the current filter string used to determine which items are included in a filtered view.
        /// </summary>
        /// <remarks>This field is intended for internal use to store the active filter criteria as a
        /// string. It is not exposed publicly and should not be accessed directly.</remarks>
        private string CurrentFilterString;
        /// <summary>
        /// Represents the starting offset for the auto-complete functionality.
        /// </summary>
        /// <remarks>This value is used to determine the position within a fullScript input where auto-complete
        /// suggestions begin.</remarks>
        private int AutoCompleteStartOffset;
        /// <summary>
        /// Indicates whether the auto-complete closure operation should be canceled.
        /// </summary>
        private bool CancelAutoCompleteClosure;
        /// <summary>
        /// Represents the last key pressed on the keyboard.
        /// </summary>
        /// <remarks>This field stores the most recent key press detected by the application. It is
        /// intended for internal use and may be updated dynamically during runtime.</remarks>
        private Keys LastKeyPressed;


        private bool OnEditorKeyPress(Keys keyData)
        {
            bool NoEcho = true, Echo = false;
            int CurPos;
            string TxtBef, TxtAft, CurrentWord;
            Token CurrentToken, LastToken;

            // Echo == true, then NoEcho == false

            CurPos = extendedEditor1.Editor.CurrentOffset();
            TxtBef = extendedEditor1.Editor.Document.GetText(0, CurPos);
            TxtAft = extendedEditor1.Editor.Document.GetText(CurPos, extendedEditor1.Editor.Text.Length - CurPos);


            #region Autocomplete/intellisense code
            if (IsIntellisenseOn)
            {
                #region Code to handle the key pressed if the "intellisense" is active
                switch (keyData)
                {
                    case Keys.Back:
                        AutocompleteDialog.Close();
                        if (!String.IsNullOrEmpty(CurrentFilterString))
                        {
                            CurrentFilterString = CurrentFilterString.Remove(CurrentFilterString.Length - 1);
                            CancelAutoCompleteClosure = true;
                            ShowIntellisense(CurrentFilterString, GetTableAliases(extendedEditor1.Editor));
                        }
                        return NoEcho;
                    case Keys.Escape:
                        AutocompleteDialog.Close();
                        CurrentFilterString = "";
                        break;
                    case Keys.OemMinus:
                        CurrentFilterString += "-";
                        ShowIntellisense(CurrentFilterString, GetTableAliases(extendedEditor1.Editor));
                        return Echo;
                    case Keys.OemMinus | Keys.Shift:
                        CurrentFilterString += "_";
                        ShowIntellisense(CurrentFilterString, GetTableAliases(extendedEditor1.Editor));
                        return Echo;
                    case (Keys)65601:
                    case Keys.A:
                    case (Keys)65602:
                    case Keys.B:
                    case (Keys)65603:
                    case Keys.C:
                    case (Keys)65604:
                    case Keys.D:
                    case (Keys)65605:
                    case Keys.E:
                    case (Keys)65606:
                    case Keys.F:
                    case (Keys)65607:
                    case Keys.G:
                    case (Keys)65608:
                    case Keys.H:
                    case (Keys)65609:
                    case Keys.I:
                    case (Keys)65610:
                    case Keys.J:
                    case (Keys)65611:
                    case Keys.K:
                    case (Keys)65612:
                    case Keys.L:
                    case (Keys)65613:
                    case Keys.M:
                    case (Keys)65614:
                    case Keys.N:
                    case (Keys)65615:
                    case Keys.O:
                    case (Keys)65616:
                    case Keys.P:
                    case (Keys)65617:
                    case Keys.Q:
                    case (Keys)65618:
                    case Keys.R:
                    case (Keys)65619:
                    case Keys.S:
                    case (Keys)65620:
                    case Keys.T:
                    case (Keys)65621:
                    case Keys.U:
                    case (Keys)65622:
                    case Keys.V:
                    case (Keys)65623:
                    case Keys.W:
                    case (Keys)65624:
                    case Keys.X:
                    case (Keys)65625:
                    case Keys.Y:
                    case (Keys)65626:
                    case Keys.Z:
                    case Keys.NumPad0:
                    case Keys.NumPad1:
                    case Keys.NumPad2:
                    case Keys.NumPad3:
                    case Keys.NumPad4:
                    case Keys.NumPad5:
                    case Keys.NumPad6:
                    case Keys.NumPad7:
                    case Keys.NumPad8:
                    case Keys.NumPad9:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.RButton | System.Windows.Forms.Keys.MButton | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.Back | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.LButton | System.Windows.Forms.Keys.Back | System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                    case System.Windows.Forms.Keys.ShiftKey | System.Windows.Forms.Keys.Space:
                        CurrentFilterString += ((char)keyData).ToString();
                        ShowIntellisense(CurrentFilterString, GetTableAliases(extendedEditor1.Editor));
                        return Echo;
                    case Keys.Delete:
                    case Keys.Left:
                    case Keys.Right:
                        return NoEcho;
                    default:
                        if (IsIntellisenseOn)
                            return Echo;
                        else
                            return NoEcho;
                }
                #endregion
            }
            else
            {
                LastKeyPressed = keyData;

                #region Code to handle the key pressed if the intellisense is inactive
                switch (keyData)
                {
                    case Keys.Control | Keys.OemPeriod://ctrl + . , Show autocomplete
                    case Keys.Space | Keys.Control://ctrl + space, Show autocomplete
                        CurrentToken = TxtBef.GetLastToken();
                        if (CurrentToken.Type == TokenType.EMPTYSPACE || CurrentToken.Type == TokenType.COMMA)
                        {
                            CurrentWord = "";
                        }
                        else
                        {
                            CurrentWord = CurrentToken.Text;
                        }
                        CurrentFilterString = CurrentWord;
                        AutoCompleteStartOffset = CurPos - CurrentWord.Length;
                        ShowIntellisense(CurrentWord, GetTableAliases(extendedEditor1.Editor));
                        return NoEcho;
                    case Keys.Space://if the fullScript before is an @ or an #, show posible vars or temp tables
                        LastToken = TxtBef.GetLastToken();
                        if (LastToken.Text.Equals("from", StringComparison.CurrentCultureIgnoreCase))
                        {
                            extendedEditor1.Editor.InsertString(" ");
                            CurrentFilterString = "";
                            AutoCompleteStartOffset = CurPos + 1;
                            ShowIntellisense("", GetTableAliases(extendedEditor1.Editor));
                            return NoEcho;
                        }
                        else if (LastToken.Text.Equals("join", StringComparison.CurrentCultureIgnoreCase))
                        {
                            extendedEditor1.Editor.InsertString(" ");
                            CurrentFilterString = "";
                            AutoCompleteStartOffset = CurPos + 1;
                            ShowIntellisense("", GetTableAliases(extendedEditor1.Editor));
                            return NoEcho;
                        }
                        break;
                    case Keys.OemPeriod:
                    case Keys.Decimal://show Sql Object childs... if any
                        extendedEditor1.Editor.InsertString(".");
                        CurPos = extendedEditor1.Editor.CurrentOffset();
                        TxtBef = extendedEditor1.Editor.Document.GetText(0, CurPos);
                        TxtAft = extendedEditor1.Editor.Document.GetText(CurPos, extendedEditor1.Editor.Text.Length - CurPos);
                        CurrentToken = TxtBef.GetLastToken();
                        if (CurrentToken.Type == TokenType.EMPTYSPACE || CurrentToken.Type == TokenType.COMMA)
                        {
                            CurrentWord = "";
                        }
                        else
                        {
                            CurrentWord = CurrentToken.Text;
                        }
                        CurrentFilterString = CurrentWord;
                        AutoCompleteStartOffset = CurPos - CurrentWord.Length;
                        ShowIntellisense(CurrentWord, GetTableAliases(extendedEditor1.Editor), "Field");
                        return NoEcho;
                    case Keys.S | Keys.Control | Keys.Shift://ctrl + shift + s , show list of snippets
                        
                        Dictionary<string, object> SnippetsDict = new Dictionary<string, object>();

                        foreach (Snippet sn in MySnippets)
                        {
                            if(!SnippetsDict.ContainsKey(sn.Name))
                                SnippetsDict.Add(sn.Name, sn.Shortcut);
                        }

                        SingleSelectionDialog dialog = new SingleSelectionDialog();
                        dialog.DialogTitle = "Snippets";
                        dialog.Message = "Please select one snippet:";
                        dialog.Style = DialogStyle.Information;
                        dialog.Required = true;
                        dialog.DialogImage = Properties.Resources.Piece48;
                        dialog.SetDataSource(SnippetsDict);

                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            extendedEditor1.Editor.InsertString(dialog.SelectedValue.ToString());
                            return NoEcho;
                        }

                        return Echo;
                    case Keys.Tab://check if it is a request for a snippet, a variable or a temp table
                        LastToken = TxtBef.GetLastToken();
                        //check for snippet
                        Snippet snip = MySnippets.FirstOrDefault(X => X.Shortcut.Equals(LastToken.Text, StringComparison.CurrentCultureIgnoreCase));
                        if (snip != null)
                        {
                            int Offset = extendedEditor1.Editor.CurrentOffset();
                            TokenList tmp = TxtBef.GetTokens();
                            extendedEditor1.Editor.SetSelectionByOffset(Offset - LastToken.Text.Length, Offset);
                            string indentation = null;
                            if (tmp.TokenCount > 1)
                            {
                                indentation = tmp[tmp.TokenCount - 2].Text;
                                int lastChar = indentation.Length - 1;
                                int chars = 0;
                                while (lastChar >= 0 && indentation[lastChar] != '\n' && indentation[lastChar] != '\r')
                                {
                                    chars++;
                                    lastChar--;
                                }
                                indentation = indentation.Substring(indentation.Length - chars);
                            }

                            InsertSnippet(snip, LastToken, indentation);
                            return NoEcho;
                        }
                        return Echo;
                        
                    default:
                        //this.Text = keyData.ToString();
                        return Echo;
                }
                #endregion
            }
            #endregion

            return Echo;
        }

        #region Snippet inserting and processing methods
        private void InsertSnippet(Snippet snip, Token lastToken, string indentation)
        {
            int curOffset = extendedEditor1.Editor.CurrentOffset();
            if (snip == null || string.IsNullOrEmpty(snip.Shortcut) || string.IsNullOrEmpty(snip.Script))
                return;

            string result = string.Empty;
            if (NewProcessSnippet(snip, indentation, out result))
            {
                snip.CursorOffset = result.IndexOf("{cursor}", StringComparison.OrdinalIgnoreCase);
                result = result.Replace("{cursor}", "");
                extendedEditor1.Editor.InsertString(result);
                extendedEditor1.Editor.ActiveTextAreaControl.Caret.Position = extendedEditor1.Editor.Document.OffsetToPosition(curOffset - lastToken.Text.Length + snip.CursorOffset);
            }
        }
        private bool NewProcessSnippet(Snippet sn, string indentation, out string results)
        {
            Dictionary<string, string> Constants = new Dictionary<string, string>();
            Constants.Add("#Today#", DateTime.Now.ToString("yyyy-MM-dd"));
            Constants.Add("#TodayNow#", DateTime.Now.ToString("yyyy-MM-dd hh:mm tt"));
            Constants.Add("#Now#", DateTime.Now.ToString("hh:mm tt"));
            Constants.Add("#Year#", DateTime.Now.Year.ToString());
            Constants.Add("#Month#", DateTime.Now.Month.ToString("00"));
            Constants.Add("#Day#", DateTime.Now.Day.ToString("00"));
            Constants.Add("#User#", Environment.UserName);
            Constants.Add("#PC#", Environment.MachineName);

            Constants.Add("#Selection Path#", databaseManager.DatabasePath);
            Constants.Add("#Server#", "");
            Constants.Add("#DataBase#", "");
            Constants.Add("#Connection#", "");
            Constants.Add("#Is Favorite#", "");
            Constants.Add("#Comments#", "");

            foreach (var constant in Constants)
            {
                if(sn.Script.Contains(constant.Key))
                    sn.Script = sn.Script.Replace(constant.Key, constant.Value);
            }
            SnippetTokenList tks = SnippetTokenList.LoadFromString(sn.Script);
            List<IDatabaseObject> selectedObjects = new List<IDatabaseObject>();

            foreach (SnippetToken st in tks.GetByType(SnippetTokenType.OBJECT))
            {
                string title = string.Empty, description = string.Empty;
                List<IDatabaseObject> values = new List<IDatabaseObject>();
                SnippetObject obj = st.AsSnippetObject();

                switch (obj.SqlObjectType)
                {
                    case SnippetObjectType.Any:
                        title = "Any SQL Object";
                        description = "Select any SQL object from the database";
                        values = DatabaseSchema.ToList();
                        break;
                    case SnippetObjectType.Table:
                        title = "Tables";
                        description = "Select a table from the database";
                        values = DatabaseSchema.Where(x => x.ObjectType == DatabaseObjectType.Table).ToList();
                        break;
                    case SnippetObjectType.View:
                        title = "Views";
                        description = "Select a view from the database";
                        values = DatabaseSchema.Where(x => x.ObjectType == DatabaseObjectType.View).ToList();
                        break;
                    case SnippetObjectType.ObjectWithFields:
                        title = "Objects with Fields";
                        description = "Select an object with fields from the database (Table, View)";
                        values = DatabaseSchema.Where(x => x.ObjectType == DatabaseObjectType.Table || x.ObjectType == DatabaseObjectType.View).ToList();
                        break;
                }

                SingleSelectionDialog dialog = new SingleSelectionDialog();
                dialog.DialogTitle = "Db Objects";
                dialog.Message = "Please select one table/view";
                dialog.Style = DialogStyle.Information;
                dialog.Required = true;
                dialog.DialogImage = Properties.Resources.Piece48;
                DataTable source = values.ConvertToDataTable("DbObjects");
                dialog.SetDataSource(source, "Name", "Name");


                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    IDatabaseObject iObj = DatabaseSchema.Where(x => x.Name.Equals(dialog.SelectedValue.ToString(), StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    iObj.NameOnSnippet = st.Text.Split(':')[1].Trim('}');
                    iObj.UsesAlias = obj.UseAlias;

                    if (obj.UseAlias)
                        st.ProcessedText = $"{iObj.Name} AS {CalculateTableAlias(iObj.Name)}";
                    else
                        st.ProcessedText = $"{iObj.Name}";

                    selectedObjects.Add(iObj);
                }
                else
                {
                    results = string.Empty;
                    return false;
                }

            }

            //resolve cycles, if any    
            int cycleNumber = 0;
            if (tks.List.Any(x => x.Type == SnippetTokenType.CYCLESTART))
            {
                SnippetTokenList resolvedCycles = new SnippetTokenList();
                for (int i = 0; i < tks.TokenCount; i++)
                {
                    SnippetToken st = tks[i];
                    if (st.Type == SnippetTokenType.CYCLESTART)
                    {
                        List<SnippetToken> tokensInCycle = new List<SnippetToken>();
                        tokensInCycle.Add(st);
                        cycleNumber++;

                        int endIndex = i + 1;
                        while (endIndex < tks.TokenCount && tks[endIndex].Type != SnippetTokenType.CYCLEEND)
                        {
                            tokensInCycle.Add(tks[endIndex]);
                            endIndex++;
                        }

                        if (tks[endIndex].Type == SnippetTokenType.CYCLEEND)
                        {
                            tokensInCycle.Add(tks[endIndex]);
                            //if the cycle end is found, then we can resolve the cycle
                            string strResolved = ResolveCycle(sn.Script, tokensInCycle, selectedObjects, cycleNumber, out bool isCanceled);

                            if (isCanceled)
                            {
                                results = string.Empty;
                                return false; // Cycle resolution was canceled
                            }

                            resolvedCycles.AddToken(new SnippetToken(SnippetTokenType.WORD, strResolved));
                            i = endIndex; // Move the index to the end of the cycle
                        }
                        else
                        {
                            //if the cycle end is not found, then we have an unmatched cycle start
                            results = string.Empty;
                            return false; // Unmatched cycle start
                        }
                    }
                    else
                    {
                        resolvedCycles.AddToken(st);
                    }
                }

                tks = resolvedCycles; // Replace the original tokens with the resolved ones
            }

            #region Generate the final script from the tokens
            string back = "";
            for (int i = 0; i < tks.TokenCount; i++)
            {
                if (tks[i].ProcessedText.IsEmpty())
                    back += tks[i].Text;
                else
                    back += tks[i].ProcessedText;
            }
            results = back;
            #endregion

            #region If the script is marked as indented, is processed here, to maintain original indentation 
            if (sn.Indented && indentation != null && indentation.Length > 0)
            {
                StringBuilder buffer = new StringBuilder();
                string[] lines = results.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                for (int i = 0; i < lines.Length; i++)
                {
                    if (i == 0)
                        buffer.AppendLine(lines[i]);
                    else if (i == lines.Length - 1)
                        buffer.Append($"{indentation}{lines[i]}");
                    else
                        buffer.AppendLine($"{indentation}{lines[i]}");
                }
                results = buffer.ToString();
            }
            #endregion

            return true;
        }

        private string CalculateTableAlias(string ReferenceTableName)
        {
            string proposedAlias = "";
            for (int i = 0; i < ReferenceTableName.Length; i++)
            {
                if (i > 0 && ReferenceTableName[i - 1] == '_')
                {
                    if (proposedAlias.Length == 0)
                        proposedAlias = ReferenceTableName[0].ToString().ToUpper();
                    proposedAlias += ReferenceTableName[i].ToString().ToUpper();
                }
                else if (char.IsUpper(ReferenceTableName[i]) || char.IsDigit(ReferenceTableName[i]) || ReferenceTableName[i] == '_')
                    proposedAlias += ReferenceTableName[i];
            }

            if (proposedAlias.Length == 0)
                proposedAlias = ReferenceTableName.Substring(0, 1).ToUpper();

            return proposedAlias.Length > 30 ? proposedAlias.Substring(0, 30) : proposedAlias;
        }

        private string ResolveCycle(string script, List<SnippetToken> tks, List<IDatabaseObject> snippetObjects, int cycleIndex, out bool isCanceled)
        {
            string separator = "";
            List<IDatabaseObject> parents = new List<IDatabaseObject>();
            Dictionary<IDatabaseObject, string> aliases = new Dictionary<IDatabaseObject, string>();

            for (int i = 0; i < tks.Count; i++)
            {
                if (tks[i].Type == SnippetTokenType.SEPARATORCONTENT)
                {
                    separator = tks[i].Text;
                    break;//ONLY ONE SEPARATOR PER CYCLE IS ALLOWED
                }
            }

            string template = string.Empty;
            string results = string.Empty;
            for (int i = 0; i < tks.Count; i++)
            {
                if (tks[i].Type == SnippetTokenType.CYCLESTART || tks[i].Type == SnippetTokenType.CYCLEEND
                    || tks[i].Type == SnippetTokenType.SEPARATORSTART || tks[i].Type == SnippetTokenType.SEPARATOREND)
                    continue; // Skip

                if (tks[i].Type == SnippetTokenType.SEPARATORCONTENT)
                {
                    template += "{SEPARATOR}"; // Add space as is
                    continue;
                }

                template += tks[i].Text;
            }


            for (int i = 0; i < tks.Count; i++)
            {
                if (tks[i].Type == SnippetTokenType.CHILD || tks[i].Type == SnippetTokenType.CHILDTYPE)
                {
                    SnippetToken childToken = tks[i];
                    string[] parts = childToken.Text.Split(new char[] { '.' });
                    string parentName = parts.Length > 0 ? parts[0].Trim('{', '}') : string.Empty;
                    parentName = parentName.Split(':')[1];
                    string childType = parts.Length > 1 ? parts[1].Trim('{', '}') : string.Empty;

                    IDatabaseObject parent = snippetObjects.FirstOrDefault(x => x.NameOnSnippet.Equals(parentName, StringComparison.CurrentCultureIgnoreCase));
                    if (parent == null)
                    {
                        //if the ParentObject is not found, then we have an unmatched cycle start
                        isCanceled = true;
                        return string.Empty; // Unmatched cycle start
                    }

                    if (!parents.Contains(parent))
                    {
                        if (parent.UsesAlias && !aliases.ContainsKey(parent))
                            aliases.Add(parent, CalculateTableAlias(parent.Name));

                        parents.Add(parent);
                    }
                }
            }

            foreach (IDatabaseObject parent in parents)
            {
                MultiSelectionDialog dialog = new MultiSelectionDialog();
                dialog.DialogTitle = "Select Fields/Parameters";
                dialog.Message = $"Select Childs for: {parent.Name}";
                dialog.Style = DialogStyle.Information;
                dialog.Required = true;
                dialog.DialogImage = Properties.Resources.Field48;
                DataTable source = parent.Childs.ConvertToDataTable("DbFields");
                dialog.SetDataSource(source, "Name", "Name");

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    List<Field> selectedChildren = new List<Field>();
                    foreach (string selected in dialog.SelectedValues)
                    {
                        Field f = parent.Childs.FirstOrDefault(x => x.Name.Equals(selected, StringComparison.CurrentCultureIgnoreCase));
                        if (f != null)
                            selectedChildren.Add(f);
                    }

                    for (int childIndex = 0; childIndex < selectedChildren.Count; childIndex++)
                    {
                        Field curChild = selectedChildren[childIndex];
                        string childType = curChild.FieldType.ToString();
                        string childProcessed = "";
                        for (int i = 0; i < tks.Count; i++)
                        {
                            if (tks[i].Type == SnippetTokenType.CYCLESTART || tks[i].Type == SnippetTokenType.CYCLEEND
                                || tks[i].Type == SnippetTokenType.SEPARATORSTART || tks[i].Type == SnippetTokenType.SEPARATOREND)
                                continue; // Skip

                            if (tks[i].Type == SnippetTokenType.SEPARATORCONTENT)
                            {
                                if (childIndex < selectedChildren.Count - 1)
                                    childProcessed += separator; // Add space as is
                            }
                            else if (tks[i].Type == SnippetTokenType.CHILD)
                            {
                                if (!aliases.ContainsKey(parent))
                                    childProcessed += curChild.Name; // Add space as is
                                else
                                    childProcessed += $"{aliases[parent]}.{curChild.Name}"; // Add space as is
                            }
                            else if (tks[i].Type == SnippetTokenType.CHILDTYPE)
                            {
                                childProcessed += childType;
                            }
                            else if (tks[i].Type == SnippetTokenType.SPACE || tks[i].Type == SnippetTokenType.WORD)
                            {
                                childProcessed += tks[i].Text;
                            }

                        }

                        results += childProcessed;
                    }
                }
                else
                {
                    isCanceled = true;
                    return string.Empty; // Cycle resolution was canceled
                }
            }


            isCanceled = false;
            return results;//empty string, to be replaced with the actual cycle resolution logic
        }
        #endregion


        private List<IDatabaseObject> GetTableAliases(TextEditorControl editor)
        {
            List<IDatabaseObject> Back = new List<IDatabaseObject>();
            TokenList Tokens = editor.Text.GetTokens();
            List<int> TokensToDelete = new List<int>();

            for (int i = 0; i < Tokens.TokenCount; i++)
            {
                Token CurToken = Tokens.GetToken(i);
                #region check for aliases, type: TableTypeObject Alias
                if ((i + 2) < Tokens.TokenCount)
                {
                    Token ProposedAlias = Tokens[i + 2];
                    if (Tokens[i + 1].Type == TokenType.EMPTYSPACE && ProposedAlias.Type == TokenType.WORD)
                    {
                        IDatabaseObject Aliased = IsTableTypeObject(CurToken.Text);
                        if (Aliased != null)//if it is a table, view or a table value function
                        {
                            if (Back.Any(X => X.Name.Equals(ProposedAlias.Text)))
                            {
                                Back.Remove(Back.First(X => X.Name.Equals(ProposedAlias.Text)));
                            }

                            Back.Add
                            (
                                new Alias()
                                {
                                    AliasedObject = Aliased.Name,
                                    AliasText = ProposedAlias.Text,
                                    Id = "Alias",
                                    Name = ProposedAlias.Text,
                                    ObjectType = Aliased.ObjectType,
                                    Script = Aliased.Script,
                                    Childs = Aliased.Childs
                                }
                            );
                        }
                    }
                }
                #endregion
                #region check for aliases, type: TableTypeObject AS Alias
                if ((i + 4) < Tokens.TokenCount)
                {
                    Token ProposedAlias = Tokens[i + 4];
                    if (Tokens[i + 1].Type == TokenType.EMPTYSPACE && Tokens[i + 2].Text.Equals("AS", StringComparison.CurrentCultureIgnoreCase) && Tokens[i + 3].Type == TokenType.EMPTYSPACE && ProposedAlias.Type == TokenType.WORD)
                    {
                        IDatabaseObject Aliased = IsTableTypeObject(CurToken.Text);
                        if (Aliased != null)
                        {
                            if (Back.Any(X => X.Name.Equals(ProposedAlias.Text)))
                            {
                                Back.Remove(Back.First(X => X.Name.Equals(ProposedAlias.Text)));
                            }

                            Back.Add
                            (
                                new Alias()
                                {
                                    AliasedObject = Aliased.Name,
                                    AliasText = ProposedAlias.Text,
                                    Id = "Alias",
                                    Name = ProposedAlias.Text,
                                    ObjectType = Aliased.ObjectType,
                                    Script = Aliased.Script,
                                    Childs = Aliased.Childs
                                }
                            );
                        }
                    }
                }
                #endregion
            }
            return Back;
        }

        private IDatabaseObject IsTableTypeObject(string text)
        {
            IDatabaseObject obj = DatabaseSchema.FirstOrDefault(X => X.Name.Equals(text, StringComparison.CurrentCultureIgnoreCase));
            return obj;
        }

        /// <summary>
        /// Displays an IntelliSense autocomplete dialog based on the provided filter string and complementary objects.
        /// </summary>
        /// <remarks>The method dynamically determines the context of the filter string and provides
        /// appropriate suggestions based on the number of components in the string. If IntelliSense is currently
        /// active, it will be closed before displaying the new autocomplete dialog.</remarks>
        /// <param name="FilterString">The string used to filter the autocomplete suggestions. This can represent schema names, database objects,
        /// or temporary objects.</param>
        /// <param name="ComplementaryObjects">A list of complementary SQL objects to include in the autocomplete suggestions.</param>
        /// <param name="Filter">Specifies the filtering type to apply to the autocomplete suggestions. The default is <see
        /// cref="FilteringType.Any"/>.</param>
        void ShowIntellisense(string FilterString, List<IDatabaseObject> ComplementaryObjects, string IsField = "")
        {
            bool QualifierBehind = false;
            int AutoCompleteLength = 0;
            List<string> Data;

            if (IsIntellisenseOn)
            {
                AutocompleteDialog.Close();
            }
            Data = FilterString.Split('.').ToList();

            CompletionDataProvider completionDataProvider;
            switch (Data.Count)
            {
                case 1://filter string 1 word, so is a dbobject
                    completionDataProvider = new CompletionDataProvider(DatabaseSchema, PopIList, null, Data[0], null);
                    AutoCompleteLength = Data[0].Length;
                    break;
                case 2://filter string 2 word, must be an object, child
                    completionDataProvider = new CompletionDataProvider(DatabaseSchema, PopIList, null, Data[0], Data[1]);
                    AutoCompleteLength = Data[1].Length;
                    QualifierBehind = true;
                    break;
                default://no filter string, show empty
                case 0://no filter string, show empty
                    completionDataProvider = new CompletionDataProvider(null, PopIList, null, "", null);
                    AutoCompleteLength = 0;
                    break;
            }
            completionDataProvider.FilteringOption = FilteringType.Any;
            completionDataProvider.ComplementaryObjects = ComplementaryObjects;
            //CompletionDataProvider completionDataProvider = new CompletionDataProvider(DataProvider, PopIList, FilterString);
            //SQLSCCProvider(CurChilds, Vars, DataProvider, PopIList, FilterString, CurrentFilter, FireAt);
            AutocompleteDialog = AutoCompleteWindow.ShowCompletionWindow(
                         this,
                         extendedEditor1.Editor,
                         "sql",
                         completionDataProvider,
                         ' ',
                         AutoCompleteStartOffset + (FilterString.Length - AutoCompleteLength),//AutoCompleteStartOffset,
                         AutoCompleteStartOffset + FilterString.Length,//FilterString.Length
                         QualifierBehind
                         );

            if (AutocompleteDialog != null)
            {
                AutocompleteDialog.Closing += ISense_Closing;
                AutocompleteDialog.Closed += CodeCompletionWindowClosed;
            }
        }
        /// <summary>
        /// Handles the event triggered when the code completion window is closed.
        /// </summary>
        /// <remarks>This method is intended to perform cleanup or other necessary actions when the code
        /// completion window is closed. Ensure that any resources associated with the code completion window are
        /// properly disposed of after this method is invoked.</remarks>
        /// <param name="sender">The source of the event, typically the code completion window.</param>
        /// <param name="e">An <see cref="EventArgs"/> instance containing the event data.</param>
        void CodeCompletionWindowClosed(object sender, EventArgs e)
        {
            //AutocompleteDialog.Closed -= CodeCompletionWindowClosed;
            //AutocompleteDialog.Closing -= ISense_Closing;
            //AutocompleteDialog.Dispose();
            //AutocompleteDialog = null;
        }
        /// <summary>
        /// Handles the closing event for an object implementing the ISense interface.
        /// </summary>
        /// <remarks>This method is invoked during the closing process and provides an opportunity to
        /// cancel the operation by setting <see cref="CancelEventArgs.Cancel"/> to <see langword="true"/>.</remarks>
        /// <param name="sender">The source of the event, typically the object being closed.</param>
        /// <param name="e">An instance of <see cref="CancelEventArgs"/> that allows the closing operation to be canceled.</param>
        void ISense_Closing(object sender, CancelEventArgs e)
        {
            //if (CancelAutoCompleteClosure)
            //{
            //    CancelAutoCompleteClosure = false;
            //    e.Cancel = true;
            //}
        }
        #region Snippet inserting and processing methods
        //private void InsertSnippet(Snippet snip, Token lastToken, string indentation)
        //{
        //    int curOffset = QueryCtrl.CurrentOffset();
        //    if (snip == null || string.IsNullOrEmpty(snip.Shortcut) || string.IsNullOrEmpty(snip.Script))
        //        return;
        //    //string selection = null;

        //    //if(snip.IsSurround)
        //    //    selection = QueryCtrl.ActiveTextAreaControl.SelectionManager.SelectedText;

        //    string result = string.Empty;
        //    //if (ProcessScript(snip, indentation, out result))
        //    if (NewProcessSnippet(snip, indentation, out result))
        //    {
        //        snip.CursorOffset = result.IndexOf("{cursor}", StringComparison.OrdinalIgnoreCase);
        //        result = result.Replace("{cursor}", "");
        //        QueryCtrl.InsertString(result);
        //        QueryCtrl.ActiveTextAreaControl.Caret.Position = QueryCtrl.Document.OffsetToPosition(curOffset - lastToken.Text.Length + snip.CursorOffset);
        //    }

        //    //int curOffset = QueryCtrl.CurrentOffset();
        //    //QueryCtrl.ActiveTextAreaControl.Caret.Position = QueryCtrl.Document.OffsetToPosition(curOffset + snip.CursorOffset);
        //    //if (snip.ProcessScript())
        //    //{//if the snippet is processed, then we must process it before inserting it
        //    //    //process the script, replacing the variables with the values
        //    //    QueryCtrl.InsertString(snip.ProcessedScript);
        //    //    QueryCtrl.ActiveTextAreaControl.Caret.Position = QueryCtrl.Document.OffsetToPosition(curOffset + snip.CursorOffset);
        //    //}

        //}
        //private bool NewProcessSnippet(Snippet sn, string indentation, out string results)
        //{
        //    Dictionary<string, string> Constants = new Dictionary<string, string>();
        //    Constants.Add("#Today#", DateTime.Now.ToString("yyyy-MM-dd"));
        //    Constants.Add("#TodayNow#", DateTime.Now.ToString("yyyy-MM-dd hh:mm tt"));
        //    Constants.Add("#Now#", DateTime.Now.ToString("hh:mm tt"));
        //    Constants.Add("#Year#", DateTime.Now.Year.ToString());
        //    Constants.Add("#Month#", DateTime.Now.Month.ToString("00"));
        //    Constants.Add("#Day#", DateTime.Now.Day.ToString("00"));
        //    Constants.Add("#User#", Environment.UserName);
        //    Constants.Add("#PC#", Environment.MachineName);

        //    Constants.Add("#Selection Path#", CalculateConectionPath(ConxNode));
        //    Constants.Add("#Server#", CalculateServer(ConxNode.Myself));
        //    Constants.Add("#DataBase#", CalculateDataBase(ConxNode.Myself));
        //    Constants.Add("#Connection#", ConxNode.Myself.Name);
        //    Constants.Add("#Is Favorite#", ConxNode.Myself.IsFavorite ? "Yes" : "No");
        //    Constants.Add("#Comments#", ConxNode.Myself.Comments);

        //    SnippetTokenList tks = sn.Script.ProcessSnippetConstants(Constants).GetSnippetTokens();
        //    List<ISqlObject> selectedObjects = new List<ISqlObject>();

        //    foreach (SnippetToken st in tks.GetByType(SnippetTokenType.OBJECT))
        //    {
        //        string title = string.Empty, description = string.Empty;
        //        List<ISqlObject> values = new List<ISqlObject>();
        //        SnippetObject obj = st.AsSnippetObject();

        //        switch (obj.SqlObjectType)
        //        {
        //            case SnippetObjectType.Any:
        //                title = "Any SQL Object";
        //                description = "Select any SQL object from the database";
        //                values = DataProvider.DbObjects.ToList();
        //                break;
        //            case SnippetObjectType.Table:
        //                title = "Tables";
        //                description = "Select a table from the database";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.Table).ToList();
        //                break;
        //            case SnippetObjectType.View:
        //                title = "Views";
        //                description = "Select a view from the database";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.View).ToList();
        //                break;
        //            case SnippetObjectType.StoredProcedure:
        //                title = "Stored Procedures";
        //                description = "Select a stored procedure from the database";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.Procedure).ToList();
        //                break;
        //            case SnippetObjectType.ScalarFunction:
        //                title = "Scalar Functions";
        //                description = "Select a scalar function from the database";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.ScalarFunction).ToList();
        //                break;
        //            case SnippetObjectType.TableFunction:
        //                title = "Table Functions";
        //                description = "Select a table function from the database";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.TableFunction).ToList();
        //                break;
        //            case SnippetObjectType.ObjectWithFields:
        //                title = "Objects with Fields";
        //                description = "Select an object with fields from the database (Table, View, Table Function)";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.Table || x.Kind == ObjectType.View || x.Kind == ObjectType.TableFunction).ToList();
        //                break;
        //            case SnippetObjectType.ObjectWithParameters:
        //                title = "Objects with Parameters";
        //                description = "Select an object with parameters from the database (Procedure, Scalar Function, Table Function)";
        //                values = DataProvider.DbObjects.Where(x => x.Kind == ObjectType.Procedure || x.Kind == ObjectType.ScalarFunction || x.Kind == ObjectType.TableFunction).ToList();
        //                break;
        //        }
        //        SqlObjectSelector selForm = new SqlObjectSelector(title, description, values);
        //        selForm.ToReplaceText = st.Text;
        //        selForm.RawScript = sn.Script;

        //        if (selForm.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //        {
        //            ISqlObject iObj = selForm.SelectedObject;
        //            iObj.NameOnSnippet = st.Text.Split(':')[1].Trim('}');
        //            iObj.UsesAlias = obj.UseAlias;

        //            if (obj.UseAlias)
        //                st.ProcessedText = $"{iObj.Name} AS {iObj.Name.CalculateTableAlias()}";
        //            else
        //                st.ProcessedText = $"{iObj.Name}";

        //            selectedObjects.Add(iObj);
        //        }
        //        else
        //        {
        //            results = string.Empty;
        //            return false;
        //        }
        //    }

        //    //resolve cycles, if any    
        //    int cycleNumber = 0;
        //    if (tks.List.Any(x => x.Type == SnippetTokenType.CYCLESTART))
        //    {
        //        SnippetTokenList resolvedCycles = new SnippetTokenList();
        //        for (int i = 0; i < tks.TokenCount; i++)
        //        {
        //            SnippetToken st = tks[i];
        //            if (st.Type == SnippetTokenType.CYCLESTART)
        //            {
        //                List<SnippetToken> tokensInCycle = new List<SnippetToken>();
        //                tokensInCycle.Add(st);
        //                cycleNumber++;

        //                int endIndex = i + 1;
        //                while (endIndex < tks.TokenCount && tks[endIndex].Type != SnippetTokenType.CYCLEEND)
        //                {
        //                    tokensInCycle.Add(tks[endIndex]);
        //                    endIndex++;
        //                }

        //                if (tks[endIndex].Type == SnippetTokenType.CYCLEEND)
        //                {
        //                    tokensInCycle.Add(tks[endIndex]);
        //                    //if the cycle end is found, then we can resolve the cycle
        //                    string strResolved = ResolveCycle(sn.Script, tokensInCycle, selectedObjects, cycleNumber, out bool isCanceled);

        //                    if (isCanceled)
        //                    {
        //                        results = string.Empty;
        //                        return false; // Cycle resolution was canceled
        //                    }

        //                    resolvedCycles.AddToken(new SnippetToken(SnippetTokenType.WORD, strResolved));
        //                    i = endIndex; // Move the index to the end of the cycle
        //                }
        //                else
        //                {
        //                    //if the cycle end is not found, then we have an unmatched cycle start
        //                    results = string.Empty;
        //                    return false; // Unmatched cycle start
        //                }
        //            }
        //            else
        //            {
        //                resolvedCycles.AddToken(st);
        //            }
        //        }

        //        tks = resolvedCycles; // Replace the original tokens with the resolved ones
        //    }

        //    #region Generate the final script from the tokens
        //    string back = "";
        //    for (int i = 0; i < tks.TokenCount; i++)
        //    {
        //        if (tks[i].ProcessedText.IsEmpty())
        //            back += tks[i].Text;
        //        else
        //            back += tks[i].ProcessedText;
        //    }
        //    results = back;
        //    #endregion

        //    #region If the script is marked as indented, is processed here, to maintain original indentation 
        //    if (sn.Indented && indentation != null && indentation.Length > 0)
        //    {
        //        StringBuilder buffer = new StringBuilder();
        //        string[] lines = results.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        //        for (int i = 0; i < lines.Length; i++)
        //        {
        //            if (i == 0)
        //                buffer.AppendLine(lines[i]);
        //            else if (i == lines.Length - 1)
        //                buffer.Append($"{indentation}{lines[i]}");
        //            else
        //                buffer.AppendLine($"{indentation}{lines[i]}");
        //        }
        //        results = buffer.ToString();
        //    }
        //    #endregion

        //    return true;
        //}
        //private string ResolveCycle(string script, List<SnippetToken> tks, List<ISqlObject> snippetObjects, int cycleIndex, out bool isCanceled)
        //{
        //    string separator = "";
        //    List<ISqlObject> parents = new List<ISqlObject>();
        //    Dictionary<ISqlObject, string> aliases = new Dictionary<ISqlObject, string>();

        //    for (int i = 0; i < tks.Count; i++)
        //    {
        //        if (tks[i].Type == SnippetTokenType.SEPARATORCONTENT)
        //        {
        //            separator = tks[i].Text;
        //            break;//ONLY ONE SEPARATOR PER CYCLE IS ALLOWED
        //        }
        //    }

        //    string template = string.Empty;
        //    string results = string.Empty;
        //    for (int i = 0; i < tks.Count; i++)
        //    {
        //        if (tks[i].Type == SnippetTokenType.CYCLESTART || tks[i].Type == SnippetTokenType.CYCLEEND
        //            || tks[i].Type == SnippetTokenType.SEPARATORSTART || tks[i].Type == SnippetTokenType.SEPARATOREND)
        //            continue; // Skip

        //        if (tks[i].Type == SnippetTokenType.SEPARATORCONTENT)
        //        {
        //            template += "{SEPARATOR}"; // Add space as is
        //            continue;
        //        }

        //        template += tks[i].Text;
        //    }


        //    for (int i = 0; i < tks.Count; i++)
        //    {
        //        if (tks[i].Type == SnippetTokenType.CHILD || tks[i].Type == SnippetTokenType.CHILDTYPE)
        //        {
        //            SnippetToken childToken = tks[i];
        //            string[] parts = childToken.Text.Split(new char[] { '.' });
        //            string parentName = parts.Length > 0 ? parts[0].Trim('{', '}') : string.Empty;
        //            parentName = parentName.Split(':')[1];
        //            string childType = parts.Length > 1 ? parts[1].Trim('{', '}') : string.Empty;

        //            ISqlObject parent = snippetObjects.FirstOrDefault(x => x.NameOnSnippet.Equals(parentName, StringComparison.CurrentCultureIgnoreCase));
        //            if (parent == null)
        //            {
        //                //if the ParentObject is not found, then we have an unmatched cycle start
        //                isCanceled = true;
        //                return string.Empty; // Unmatched cycle start
        //            }

        //            if (!parents.Contains(parent))
        //            {
        //                if (parent.UsesAlias && !aliases.ContainsKey(parent))
        //                    aliases.Add(parent, parent.Name.CalculateTableAlias());

        //                parents.Add(parent);
        //            }
        //        }
        //    }

        //    foreach (ISqlObject parent in parents)
        //    {
        //        SqlChildSelector selForm = new SqlChildSelector(parent, "Select Fields/Parameters", $"Select Childs for: {parent.Schema}.{parent.Name}");
        //        /*
        //        selForm.ToReplaceText = "{ObjectType:ObjectName.Child}";
        //        int tmpIndex = script.IndexOf($":{parent.NameOnSnippet}.Child}}", StringComparison.CurrentCultureIgnoreCase);
        //        int startTmp;
        //        if (tmpIndex >= 0)
        //        {
        //            startTmp = tmpIndex - 1;
        //            while (startTmp >= 0 && script[startTmp] != '{')
        //                startTmp--;

        //            selForm.ToReplaceText = script.Substring(startTmp, tmpIndex - startTmp + 1) + $"{parent.NameOnSnippet}.Child}}";
        //        }
        //        else
        //        {
        //            tmpIndex = script.IndexOf($":{parent.NameOnSnippet}.ChildType}}", StringComparison.CurrentCultureIgnoreCase);
        //            if (tmpIndex >= 0)
        //            {
        //                startTmp = tmpIndex - 1;
        //                while (startTmp >= 0 && script[startTmp] != '{')
        //                    startTmp--;

        //                selForm.ToReplaceText = script.Substring(startTmp, tmpIndex - startTmp + 1) + $"{parent.NameOnSnippet}.ChildType}}";
        //            }
        //            else
        //            {
        //                selForm.ToReplaceText = "";
        //            }
        //        }
        //        */
        //        selForm.RawScript = script;
        //        selForm.CycleNumber = cycleIndex;

        //        if (selForm.ShowDialog() == DialogResult.OK)
        //        {
        //            List<ISqlChild> selectedChildren = selForm.SelectedChilds;
        //            for (int childIndex = 0; childIndex < selectedChildren.Count; childIndex++)
        //            {
        //                ISqlChild curChild = selectedChildren[childIndex];
        //                string childType = "";

        //                if (curChild.Type.InStringList(false, "varchar", "nvarchar", "char", "nchar", "binary", "varbinary"))
        //                {
        //                    if (curChild.Precision == -1)
        //                        childType += $"{curChild.Type}(MAX)";
        //                    else
        //                        childType += $"{curChild.Type}({curChild.Precision})";
        //                }
        //                else
        //                {
        //                    childType += curChild.Type;
        //                }

        //                string childProcessed = "";
        //                for (int i = 0; i < tks.Count; i++)
        //                {
        //                    if (tks[i].Type == SnippetTokenType.CYCLESTART || tks[i].Type == SnippetTokenType.CYCLEEND
        //                        || tks[i].Type == SnippetTokenType.SEPARATORSTART || tks[i].Type == SnippetTokenType.SEPARATOREND)
        //                        continue; // Skip

        //                    if (tks[i].Type == SnippetTokenType.SEPARATORCONTENT)
        //                    {
        //                        if (childIndex < selectedChildren.Count - 1)
        //                            childProcessed += separator; // Add space as is
        //                    }
        //                    else if (tks[i].Type == SnippetTokenType.CHILD)
        //                    {
        //                        if (!aliases.ContainsKey(parent))
        //                            childProcessed += curChild.Name; // Add space as is
        //                        else
        //                            childProcessed += $"{aliases[parent]}.{curChild.Name}"; // Add space as is
        //                    }
        //                    else if (tks[i].Type == SnippetTokenType.CHILDTYPE)
        //                    {
        //                        childProcessed += childType;
        //                    }
        //                    else if (tks[i].Type == SnippetTokenType.SPACE || tks[i].Type == SnippetTokenType.WORD)
        //                    {
        //                        childProcessed += tks[i].Text;
        //                    }

        //                }

        //                results += childProcessed;
        //            }
        //        }
        //        else
        //        {
        //            isCanceled = true;
        //            return string.Empty; // Cycle resolution was canceled
        //        }
        //    }


        //    isCanceled = false;
        //    return results;//empty string, to be replaced with the actual cycle resolution logic
        //}
        #endregion
        #endregion


        private void InitializeGrid()
        {
            zidGrid1.Plugins.Add(new ZidUtilities.CommonCode.Win.Controls.Grid.Plugins.DataExportPlugin());
            zidGrid1.Plugins.Add(new ZidUtilities.CommonCode.Win.Controls.Grid.Plugins.ColumnVisibilityPlugin());
            zidGrid1.Plugins.Add(new ZidUtilities.CommonCode.Win.Controls.Grid.Plugins.CopySpecialPlugin());
            zidGrid1.Plugins.Add(new ZidUtilities.CommonCode.Win.Controls.Grid.Plugins.FreezeColumnsPlugin());
            zidGrid1.Plugins.Add(new ZidUtilities.CommonCode.Win.Controls.Grid.Plugins.QuickFilterPlugin());

            foreach (var option in zidGrid1.GetDefaultMenuOptions())
                zidGrid1.CustomMenuItems.Add(option);

        }

        private void InitializeTextEditor()
        {
            extendedEditor1.Txt01Helper.Text = databaseManager.GetConnection().DataSource;
            extendedEditor1.Syntax = ZidUtilities.CommonCode.ICSharpTextEditor.SyntaxHighlighting.TransactSQL;
            extendedEditor1.Editor.Refresh();
        }

        private void extendedEditor1_OnRun(string selectedText, ZidUtilities.CommonCode.ICSharpTextEditor.ToolbarOption btnClicked)
        {
            string errorText = String.Empty;
            DataSet? results = databaseManager.ExecuteQuery(selectedText, out errorText);

            if (results != null && errorText.IsEmpty())
            {
                if(results.Tables.Count == 0)
                {
                    MessageBox.Show(this, "Query executed successfully. No results to display.", "Query Executed", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    zidGrid1.DataSource = results.Tables[0];
                    splitContainer1.Panel2Collapsed = false;
                }
            }
            else
            {
                MessageBox.Show(this, errorText, "Query Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void extendedEditor1_OnKill(string selectedText, ZidUtilities.CommonCode.ICSharpTextEditor.ToolbarOption btnClicked)
        {

        }

        private void extendedEditor1_OnStop(string selectedText, ZidUtilities.CommonCode.ICSharpTextEditor.ToolbarOption btnClicked)
        {

        }

        public void LoadDatabaseSchema()
        {
            DatabaseSchema.Clear();

            // Get connection for direct PRAGMA queries
            var connection = databaseManager.GetConnection();
            if (connection == null || connection.State != ConnectionState.Open)
                return;

            // Load all tables
            var tableNames = databaseManager.GetTableNames();
            foreach (var tableName in tableNames)
            {
                var table = new SchemaTable
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = tableName,
                    ObjectType = Schema.DatabaseObjectType.Table,
                    Script = databaseManager.GetTableSql(tableName),
                    Childs = new List<SchemaField>()
                };

                // Get fields for this table using PRAGMA table_info
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA table_info('{tableName}')";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var columnName = reader.GetString(1);
                            var sqlType = reader.GetString(2).ToUpper();
                            var notNull = reader.GetInt32(3) == 1;
                            var defaultValue = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString();
                            var isPrimaryKey = reader.GetInt32(5) == 1;

                            // Check if autoincrement by querying sqlite_sequence or checking if it's INTEGER PRIMARY KEY
                            bool isAutoIncrement = false;
                            if (isPrimaryKey && sqlType.Contains("INT"))
                            {
                                // Check if table has AUTOINCREMENT in its definition
                                var tableSql = table.Script.ToUpper();
                                if (tableSql.Contains("AUTOINCREMENT"))
                                {
                                    isAutoIncrement = true;
                                }
                            }

                            // Map SQLite type to SqliteFieldType enum
                            var fieldType = MapSqliteType(sqlType);

                            var field = new SchemaField
                            {
                                Name = columnName,
                                FieldType = fieldType,
                                IsNullable = !notNull,
                                Increment = isAutoIncrement,
                                DefaultValue = defaultValue,
                                IsKey = isPrimaryKey
                            };

                            table.Childs.Add(field);
                        }
                    }
                }

                DatabaseSchema.Add(table);
            }

            // Load all views
            var viewNames = databaseManager.GetViewNames();
            foreach (var viewName in viewNames)
            {
                var view = new SchemaView
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = viewName,
                    ObjectType = Schema.DatabaseObjectType.View,
                    Script = databaseManager.GetViewSql(viewName),
                    Childs = new List<SchemaField>()
                };

                // Get fields for this view using PRAGMA table_info (works for views too)
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"PRAGMA table_info('{viewName}')";
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var columnName = reader.GetString(1);
                            var sqlType = reader.GetString(2).ToUpper();
                            var notNull = reader.GetInt32(3) == 1;
                            var defaultValue = reader.IsDBNull(4) ? null : reader.GetValue(4).ToString();
                            var isPrimaryKey = reader.GetInt32(5) == 1;

                            // Views don't have autoincrement
                            var fieldType = MapSqliteType(sqlType);

                            var field = new SchemaField
                            {
                                Name = columnName,
                                FieldType = fieldType,
                                IsNullable = !notNull,
                                Increment = false,
                                DefaultValue = defaultValue,
                                IsKey = isPrimaryKey
                            };

                            view.Childs.Add(field);
                        }
                    }
                }

                DatabaseSchema.Add(view);
            }
        }

        private Schema.SqliteFieldType MapSqliteType(string sqlType)
        {
            sqlType = sqlType.ToUpper();

            if (sqlType.Contains("INT"))
                return Schema.SqliteFieldType.INTEGER;
            else if (sqlType.Contains("CHAR") || sqlType.Contains("CLOB") || sqlType.Contains("TEXT"))
                return Schema.SqliteFieldType.TEXT;
            else if (sqlType.Contains("BLOB"))
                return Schema.SqliteFieldType.BLOB;
            else if (sqlType.Contains("REAL") || sqlType.Contains("FLOA") || sqlType.Contains("DOUB"))
                return Schema.SqliteFieldType.REAL;
            else if (sqlType.Contains("NUMERIC") || sqlType.Contains("DECIMAL"))
                return Schema.SqliteFieldType.NUMERIC;
            else
                return Schema.SqliteFieldType.TEXT; // Default to TEXT for unknown types
        }
    }
}
