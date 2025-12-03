using ICSharpCode.TextEditor;
using ICSharpCode.TextEditor.Gui.CompletionWindow;
using SqliteHelper48.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqliteHelper48
{
    public enum FilteringType { Table, View, FieldItem, Any, None }
    public class CompletionDataProvider : ICompletionDataProvider
    {
        private ImageList imageList;
        List<IDatabaseObject> DbServerData;
        public FilteringType FilteringOption { get; set; }
        private string FObject, FChild;
        public string FilterString
        {
            get
            {
                if (FChild == null)
                    return FObject;
                else
                    return String.Format("{0}.{1}", FObject, FChild);

            }
        }
        public int FilteringLevel
        {
            get
            {
                if (FChild == null)
                    return 1;
                else
                    return 2;

            }
        }
        public List<IDatabaseObject> ComplementaryObjects { get; set; }


        public CompletionDataProvider(List<IDatabaseObject> DbServerData, ImageList imageList)
        {
            this.DbServerData = DbServerData;
            this.imageList = imageList;
            ComplementaryObjects = new List<IDatabaseObject>();
            FilteringOption = FilteringType.Any;
        }
        public CompletionDataProvider(List<IDatabaseObject> DbServerData, ImageList imageList, string FSchema, string FObject, string FChild)
        {
            this.DbServerData = DbServerData;
            this.imageList = imageList;
            this.FObject = FObject;
            this.FChild = FChild;
            ComplementaryObjects = new List<IDatabaseObject>();
            FilteringOption = FilteringType.Any;
        }
        public ImageList ImageList { get { return imageList; } }
        public string PreSelection { get { return null; } }
        public int DefaultIndex { get { return -1; } }
        private bool _IsEmpty;
        public bool IsEmpty { get { return _IsEmpty; } }
        public CompletionDataProviderKeyResult ProcessKey(char key)
        {
            if (char.IsLetterOrDigit(key) || key == '_')
            {
                return CompletionDataProviderKeyResult.NormalKey;
            }
            return CompletionDataProviderKeyResult.InsertionKey;
        }
        /// <summary>
        /// Called when entry should be inserted. Forward to the insertion action of the completion data.
        /// </summary>
        public bool InsertAction(ICompletionData data, TextArea textArea, int insertionOffset, char key)
        {
            textArea.Caret.Position = textArea.Document.OffsetToPosition(
                Math.Min(insertionOffset, textArea.Document.TextLength)
                );
            return data.InsertAction(textArea, key);
        }
        public ICompletionData[] GenerateCompletionData(string fileName, TextArea textArea, char charTyped)
        {
            //List<ISqlObject> BackItems = new List<ISqlObject>();
            List<ICompletionData> BackItems = new List<ICompletionData>();

            if (DbServerData == null)
                return BackItems.ToArray();


            switch (FilteringLevel)
            {
                case 1://only 1 item received for autocomplete(FObject)
                default:
                    if (ComplementaryObjects != null && ComplementaryObjects.Count >0)
                        BackItems.AddRange(ComplementaryObjects.Where(X => X.Name.StartsWith(FObject, StringComparison.CurrentCultureIgnoreCase)));
                    BackItems.AddRange(DbServerData.Where(X => X.Name.StartsWith(FObject, StringComparison.CurrentCultureIgnoreCase)));
                    break;
                case 2://two items received for autocomplete(FObject, FChild)
                    List<IDatabaseObject> Buffer = DbServerData.Where(X => X.Name.Equals(FObject, StringComparison.CurrentCultureIgnoreCase)).ToList();
                    if (ComplementaryObjects != null && ComplementaryObjects.Count > 0)
                        Buffer.AddRange(ComplementaryObjects.Where(X => X.Name.Equals(FObject, StringComparison.CurrentCultureIgnoreCase)));
                    foreach (IDatabaseObject Parent in Buffer)
                    {
                        BackItems.AddRange(Parent.Childs.Where(X => X.Name.StartsWith(FChild, StringComparison.CurrentCultureIgnoreCase)));
                    }
                    break;
            }

            //switch (FilteringOption)
            //{
            //    case FilteringType.Table:
            //        if(ComplementaryObjects != null)
            //            BackItems.AddRange(ComplementaryObjects);
            //        BackItems.AddRange(DbServerData.Where(X => X.ObjectType == DatabaseObjectType.Table && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
            //        break;
            //    case FilteringType.View:
            //        if (ComplementaryObjects != null)
            //            BackItems.AddRange(ComplementaryObjects);
            //        BackItems.AddRange(DbServerData.Where(X => X.ObjectType == DatabaseObjectType.View && X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
            //        break;
            //    case FilteringType.FieldItem:
            //        if (ComplementaryObjects != null)
            //            BackItems.AddRange(ComplementaryObjects);
            //        BackItems.AddRange(DbServerData.Where(X => X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
            //        break;
            //    case FilteringType.Any:
            //    default:
            //        if (ComplementaryObjects != null)
            //            BackItems.AddRange(ComplementaryObjects);
            //        BackItems.AddRange(DbServerData.Where(X => X.Name.StartsWith(FilterString, StringComparison.CurrentCultureIgnoreCase)));
            //        break;
            //}

            BackItems.Sort((X, Y) => String.Compare(X.Text, Y.Text));

            return BackItems.ToArray();

        }
    }
}
