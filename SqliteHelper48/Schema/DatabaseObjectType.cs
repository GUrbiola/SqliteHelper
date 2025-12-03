using System;

namespace SqliteHelper48.Schema
{
    [Flags]
    public enum DatabaseObjectType
    {
        None = 0,
        Table = 1,
        View = 2
    }
}
