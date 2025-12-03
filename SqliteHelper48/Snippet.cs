using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZidUtilities.CommonCode;

namespace SqliteHelper48
{
    public class Snippet
    {
        public string Name { get; set; }
        public string Shortcut { get; set; }
        public string Description { get; set; }
        private string _Script;
        public string Script
        {
            get
            {
                return _Script;
            }
            set
            {
                _Script = value;
                if (value != null)
                    CursorOffset = _Script.IndexOf("{cursor}", StringComparison.OrdinalIgnoreCase);
                else
                    CursorOffset = -1;
            }
        }
        public bool Indented { get; set; }
        public int CursorOffset { get; set; }
        //public bool IsInsert { get; set; }
        //public bool IsSurround { get { return !IsInsert; } set { IsInsert = !value; }  }

        public Snippet()
        {
            Name = string.Empty;
            Shortcut = string.Empty;
            Description = string.Empty;
            Script = string.Empty;
            Indented = false;
            //IsInsert = true;
            CursorOffset = 0;
        }
        public Snippet(string name, string shortcut, string description, string script, bool indented = false)
        {
            Name = name;
            Shortcut = shortcut;
            Description = description;
            Script = script;
            Indented = indented;
            //IsInsert = true;
            CursorOffset = 0;
        }


    }
    public enum SnippetObjectType
    {
        Any,
        Table,
        View,
        StoredProcedure,
        ScalarFunction,
        TableFunction,
        ObjectWithFields, // Table, View, Table Function
        ObjectWithParameters // Procedure, Scalar Function, Table Function
    }

    public class SnippetObject
    {
        public int StartOffset { get; set; }
        public string ReplaceString { get; set; }
        public string Name { get; set; }
        public bool UseAlias { get; set; }
        public SnippetObjectType SqlObjectType { get; set; }
        public SnippetObject()
        {
            StartOffset = 0;
            ReplaceString = string.Empty;
            Name = string.Empty;
            SqlObjectType = SnippetObjectType.Any;
        }
        public SnippetObject(SnippetObjectType sqlObjectType, string name)
        {
            Name = name;
            SqlObjectType = sqlObjectType;
        }
    }
    public class SnippetCycle
    {
        public SnippetObject Parent { get; set; }
        public string RawText { get; set; }
        public string InnerText { get; set; }
    }

    public class SnippetTokenList
    {
        public List<SnippetToken> List { get; set; }
        public List<int> StartOffsets { get; set; }
        public List<int> EndOffsets { get; set; }
        public List<int> TokenLengths { get; set; }
        public int TokenCount { get { return List.Count; } }

        public SnippetTokenList()
        {
            List = new List<SnippetToken>();
            StartOffsets = new List<int>();
            EndOffsets = new List<int>();
            TokenLengths = new List<int>();
        }

        public void AddToken(SnippetToken Current)
        {
            if (Current == null || Current.IsTextEmpty)
                return;

            if (List.Count == 0)
            {
                StartOffsets.Add(0);
            }
            else
            {
                StartOffsets.Add(StartOffsets.Last() + TokenLengths.Last());
            }
            EndOffsets.Add(StartOffsets.Last() + Current.Text.Length - 1);
            TokenLengths.Add(Current.Text.Length);
            List.Add(Current);
        }
        public int GetStartOf(SnippetToken token)
        {
            int index = List.IndexOf(token);
            return GetStartOf(index);
        }
        public int GetStartOf(int tokenIndex)
        {
            if (tokenIndex < List.Count)
                return StartOffsets[tokenIndex];
            return -1;
        }
        public int GetEndOf(SnippetToken token)
        {
            int index = List.IndexOf(token);
            return GetEndOf(index);
        }
        public int GetEndOf(int tokenIndex)
        {
            if (tokenIndex < List.Count)
                return EndOffsets[tokenIndex];
            return -1;
        }
        public int GetLengthOf(SnippetToken token)
        {
            int index = List.IndexOf(token);
            return GetLengthOf(index);
        }
        public int GetLengthOf(int tokenIndex)
        {
            if (tokenIndex < List.Count)
                return TokenLengths[tokenIndex];
            return -1;
        }
        public void Clean()
        {
            List.Clear();
            StartOffsets.Clear();
            EndOffsets.Clear();
            TokenLengths.Clear();
        }
        public SnippetToken GetToken(int tokenIndex)
        {
            return tokenIndex < List.Count ? List[tokenIndex] : null;
        }
        public void RemoveTokenAt(int tokenIndex)
        {
            if (tokenIndex < List.Count)
            {
                List.RemoveAt(tokenIndex);
                StartOffsets.RemoveAt(tokenIndex);
                EndOffsets.RemoveAt(tokenIndex);
                TokenLengths.RemoveAt(tokenIndex);
                if (List.Count > 0)
                {
                    for (int ind = tokenIndex; ind < List.Count; ind++)
                    {
                        if (tokenIndex == 0 && ind == 0)
                        {
                            StartOffsets[0] = 0;
                            EndOffsets[0] = StartOffsets[0] + List[0].Text.Length - 1;
                            TokenLengths[0] = EndOffsets[0] - StartOffsets[0] + 1;
                        }
                        else
                        {
                            StartOffsets[ind] = StartOffsets[ind - 1] + TokenLengths[ind - 1];
                            EndOffsets[ind] = StartOffsets[ind] + List[ind].Text.Length - 1;
                            TokenLengths[ind] = EndOffsets[ind] - StartOffsets[ind] + 1;
                        }
                    }
                }
            }
        }
        public SnippetToken this[int index]
        {
            get { return GetToken(index); }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (SnippetToken t in List)
            {
                sb.Append(t.Text);
            }
            return sb.ToString();
        }
        public SnippetToken GetTokenAtOffset(int offset, out int index)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (StartOffsets[i] <= offset && offset <= EndOffsets[i])
                {
                    index = i;
                    return GetToken(i);
                }
            }
            index = -1;
            return null;
        }
        public SnippetToken GetTokenAtOffset(int offset)
        {
            for (int i = 0; i < List.Count; i++)
            {
                if (StartOffsets[i] <= offset && offset <= EndOffsets[i])
                {
                    return GetToken(i);
                }
            }
            return null;
        }
        public List<SnippetToken> GetByType(SnippetTokenType tokenType)
        {
            return List.Where(X => X.Type == tokenType).ToList();
        }


        public void AddTokenAt(int i, SnippetToken token)
        {
            SnippetTokenList buff = new SnippetTokenList();
            for (int j = 0; j < this.TokenCount; j++)
            {
                if (j == i)
                    buff.AddToken(token);
                buff.AddToken(this[j]);
            }

            this.List = buff.List;
            this.StartOffsets = buff.StartOffsets;
            this.EndOffsets = buff.EndOffsets;
            this.TokenLengths = buff.TokenLengths;
        }

        public static SnippetTokenList LoadFromString(string Text, bool keepOriginal = false)
        {
            SnippetTokenList Back = new SnippetTokenList();
            int StringLength = String.IsNullOrEmpty(Text) ? 0 : Text.Length;
            SnippetToken Current = null;
            List<char> whiteSpaces = new List<char>() { ' ', '\t', '\r', '\n' };


            for (int index = 0; index < StringLength; index++)
            {
                char CurChar = Text[index];
                if (Current != null)
                {
                    switch (Current.Type)
                    {
                        case SnippetTokenType.SPACE:
                            if (whiteSpaces.Contains(CurChar))
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            else
                            {
                                Back.AddToken(Current);
                                index--;
                                Current = null;
                            }
                            break;
                        case SnippetTokenType.WORD:
                            if (whiteSpaces.Contains(CurChar) || CurChar == '{')
                            {
                                Back.AddToken(Current);
                                index--;
                                Current = null;
                            }
                            else
                            {
                                Current.Text = Current.Text.AppendChar(CurChar);
                            }
                            break;
                        case SnippetTokenType.OBJECT:
                        case SnippetTokenType.CHILD:
                        case SnippetTokenType.SEPARATORSTART:
                        case SnippetTokenType.SEPARATOREND:
                        case SnippetTokenType.SEPARATORCONTENT:
                        case SnippetTokenType.CURSOR:
                        case SnippetTokenType.CYCLESTART:
                        case SnippetTokenType.CYCLEEND:
                        case SnippetTokenType.UNKNOWN:
                        default://this must never happen, only words and spaces are expected in this switch
                            break;
                    }
                }
                else if (Current == null)
                {
                    #region no previous token, must check if create a new one, or let the current stay null and add a new instance of token
                    if (whiteSpaces.Contains(CurChar))
                    {
                        Current = new SnippetToken(SnippetTokenType.SPACE, CurChar.ToString());
                    }
                    else if (CurChar == '{')
                    {//might be OBJECT, CHILD, SEPARATORSTART, SEPARATOREND, CURSOR, CYCLESTART,CYCLEEND
                        string content = "";
                        int indexNext = index + 1;
                        while (indexNext < StringLength && Text[indexNext] != '}')
                        {
                            content += Text[indexNext];
                            indexNext++;
                        }

                        if (Text[indexNext] == '}')
                        {
                            if (content.Contains(":") && !content.Contains("."))
                            {
                                if (content.EndsWith("*"))
                                {
                                    Current = new SnippetToken(SnippetTokenType.OBJECT, $"{CurChar}{content.Trim('*')}{Text[indexNext]}");
                                    Current.UsesAlias = true;
                                }
                                else
                                {
                                    Current = new SnippetToken(SnippetTokenType.OBJECT, $"{CurChar}{content}{Text[indexNext]}");
                                }

                            }
                            else if (content.Contains(":") && content.Contains("."))
                            {
                                if (content.Contains("CHILDTYPE", StringComparison.CurrentCultureIgnoreCase))
                                    Current = new SnippetToken(SnippetTokenType.CHILDTYPE, $"{CurChar}{content}{Text[indexNext]}");
                                else
                                    Current = new SnippetToken(SnippetTokenType.CHILD, $"{CurChar}{content}{Text[indexNext]}");
                            }
                            else if (content.Equals("separator", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Current = new SnippetToken(SnippetTokenType.SEPARATORSTART, $"{CurChar}{content}{Text[indexNext]}");
                            }
                            else if (content.Equals("/separator", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Current = new SnippetToken(SnippetTokenType.SEPARATOREND, $"{CurChar}{content}{Text[indexNext]}");
                            }
                            else if (content.Equals("cursor", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Current = new SnippetToken(SnippetTokenType.CURSOR, $"{CurChar}{content}{Text[indexNext]}");
                            }
                            else if (content.Equals("cycle", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Current = new SnippetToken(SnippetTokenType.CYCLESTART, $"{CurChar}{content}{Text[indexNext]}");
                            }
                            else if (content.Equals("/cycle", StringComparison.CurrentCultureIgnoreCase))
                            {
                                Current = new SnippetToken(SnippetTokenType.CYCLEEND, $"{CurChar}{content}{Text[indexNext]}");
                            }
                            else
                            {
                                Current = new SnippetToken(SnippetTokenType.UNKNOWN, $"{CurChar}{content}{Text[indexNext]}");
                            }

                            Back.AddToken(Current);//Add to the list
                            index = indexNext;//skip this token
                            Current = null;//reset current token
                        }

                    }
                    else
                    {
                        Current = new SnippetToken(SnippetTokenType.WORD, CurChar.ToString());
                    }
                    #endregion
                }
            }
            if (Current != null)
                Back.AddToken(Current);

            //finally cycle through the tokens and replace the tokens between SEPARATORSTART and SEPARATOREND with a single token of type SEPARATORCONTENT
            if (!keepOriginal && Back.List.Count > 0)
            {
                if (Back.List.Any(x => x.Type == SnippetTokenType.SEPARATORSTART))
                {
                    SnippetTokenList buffer = new SnippetTokenList();
                    for (int i = 0; i < Back.List.Count; i++)
                    {
                        if (Back.List[i].Type == SnippetTokenType.SEPARATORSTART)
                        {
                            int sepEnd = i + 1;
                            while (sepEnd < Back.List.Count && Back.List[sepEnd].Type != SnippetTokenType.SEPARATOREND)
                                sepEnd++;

                            if (Back.List[sepEnd].Type == SnippetTokenType.SEPARATOREND)
                            {
                                int ctnIndex = i + 1;
                                string strBuff = "";
                                while (ctnIndex < sepEnd)
                                {
                                    strBuff += Back.List[ctnIndex].Text;
                                    ctnIndex++;
                                }

                                buffer.AddToken(new SnippetToken(SnippetTokenType.SEPARATORCONTENT, strBuff));
                                i = sepEnd;//skip the SEPARATOREND token
                            }
                            else
                            {//if we reach here, it means that there is no SEPARATOREND, so we just add the SEPARATORSTART and continue
                                buffer.AddToken(Back.List[i]);
                            }
                        }
                        else
                        {
                            buffer.AddToken(Back.List[i]);//just add it to the list
                        }
                    }
                    return buffer;
                }
            }

            return Back;
        }
    }

    public class SnippetToken
    {
        public SnippetTokenType Type { get; set; }
        public string Text { get; set; }
        public string ProcessedText { get; set; }
        public bool UsesAlias { get; set; }
        public SnippetToken() { UsesAlias = false; }
        public SnippetToken(SnippetTokenType Type, string Text)
        {
            this.Type = Type;
            this.Text = Text;
            UsesAlias = false;
        }
        public bool IsTextEmpty { get { return String.IsNullOrEmpty(Text); } }
        public SnippetObject AsSnippetObject()
        {
            SnippetObject back = null;
            if (this.Type == SnippetTokenType.OBJECT)
            {
                string[] data = this.Text.Trim('{').Trim('}').Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                string tp = data[0].Trim().ToUpper();
                string name = data.Length > 1 ? data[1].Trim() : string.Empty;

                switch (tp.ToUpper())
                {
                    case "TABLE":
                        back = new SnippetObject(SnippetObjectType.Table, name);
                        break;
                    case "VIEW":
                        back = new SnippetObject(SnippetObjectType.View, name);
                        break;
                    case "STOREDPROCEDURE":
                        back = new SnippetObject(SnippetObjectType.StoredProcedure, name);
                        break;
                    case "SCALARFUNCTION":
                        back = new SnippetObject(SnippetObjectType.ScalarFunction, name);
                        break;
                    case "TABLEFUNCTION":
                        back = new SnippetObject(SnippetObjectType.TableFunction, name);
                        break;
                    case "OBJECTWITHFIELDS":
                        back = new SnippetObject(SnippetObjectType.ObjectWithFields, name);
                        break;
                    case "OBJECTWITHPARAMETERS":
                        back = new SnippetObject(SnippetObjectType.ObjectWithParameters, name);
                        break;
                    case "ANY":
                    default:
                        back = new SnippetObject(SnippetObjectType.Any, name);
                        break;
                }

                if (back != null)
                    back.UseAlias = this.UsesAlias;

                return back;
            }

            return null;
        }
    }

    public enum SnippetTokenType
    {
        SPACE,
        OBJECT,
        CHILD,
        CHILDTYPE,
        SEPARATORSTART,
        SEPARATOREND,
        SEPARATORCONTENT,
        CURSOR,
        CYCLESTART,
        CYCLEEND,
        WORD,
        UNKNOWN
    };

}
