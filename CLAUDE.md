# SqliteHelper - Project Memory

## Project Overview

**SqliteHelper** is a comprehensive Windows Forms desktop application for both designing and managing SQLite databases. It combines visual diagram-based database design with full database management capabilities including querying, table creation, and data editing.

**Technology Stack:**
- .NET 8.0 Windows Forms
- SourceGear.sqlite3 3.50.4.5
- ZidUtilities.CommonCode packages (custom utilities)
- ZidUtilities.CommonCode.DataAccess.Sqlite 1.0.0
- ICSharpCode.TextEditor (for SQL syntax highlighting)
- ClosedXML.Excel (for data export)
- C# with nullable reference types enabled

**Project File Format:** `.shlp` (SQLite Helper Project)

## Key Components

### Core Files
- `MainForm.cs` - Main application window with dual modes: project design & database management
- `DiagramDesignerForm.cs` - Visual database diagram designer
- `DatabaseProject.cs` - Project data model
- `TableControl.cs` - Visual representation of tables in diagram
- `CommentControl.cs` - Comments/annotations in diagrams

### Database Management
- `DatabaseManager.cs` - Core database connection and lifecycle management
- `SqlGenerator.cs` - Generates SQL scripts from diagram
- `SqliteDatabaseExporter.cs` - Export functionality
- `TableColumnInfo.cs` - Table metadata and schema information

### Dialogs & Editors
- `ProjectNameDialog.cs` - New project creation
- `TableEditorDialog.cs` - Table structure editing
- `RelationshipDialog.cs` - Define table relationships
- `ManageRelationshipsDialog.cs` - Manage existing relationships
- `QueryDialog.cs` - SQL query editor with syntax highlighting and export
- `CreateTableDialog.cs` - Interactive table creation with SQL preview
- `RowEditorDialog.cs` - Edit table row data

### Utilities
- `RecentProjects.cs` - Recent projects tracking
- `RecentDatabases.cs` - Recent databases tracking

## Features

1. **Project Management (Design Mode)**
   - Create/Open/Save `.shlp` project files
   - Recent projects list with quick access
   - Project naming and organization

2. **Visual Database Design**
   - Diagram-based table designer
   - Visual relationship management
   - Comment/annotation support
   - SQL script generation from diagrams

3. **Database Management (Runtime Mode)**
   - Open and browse existing SQLite databases
   - Tree view navigation of database structure
   - SQL query editor with syntax highlighting
   - Create tables interactively
   - Edit table data
   - Export query results to Excel
   - Recent databases tracking

4. **Dual-Mode Operation**
   - Design Mode: Create database schemas visually
   - Management Mode: Work with actual SQLite databases
   - Generate SQLite files from design projects

## Architecture Notes

- Windows Forms application with visual designer pattern
- Project-based workflow (similar to ERD tools)
- Separation between design (diagram) and generation (SQL output)

## Development Environment

- Visual Studio 2022 (17.14+)
- .NET 8.0 SDK
- Windows platform required

## Git Repository

- **Current Branch:** master
- **Remote:** origin/master
- **Last Commits:**
  - Add project files
  - Add .gitattributes, .gitignore, and README.md

## Recent Conversation Context

### 2025-12-01 (Session 1)

**User Request:** Retrieve previous prompts and save conversations

**Context:**
- User wants to ensure conversations are preserved
- Explained Claude Code conversation retention (5 years w/ opt-in, 30 days without)
- No built-in conversation export feature exists
- CLAUDE.md is for project memory (key decisions/context), not full transcripts
- Will update this file with important decisions and context going forward

**Action:** Created initial CLAUDE.md file

### 2025-12-01 (Session 1 - Update)

**User Action:** Restored old files with enhanced functionality

**Changes Detected:**
- MainForm.cs: Massive expansion (+799 lines) - now supports dual modes (design + database management)
- Added DatabaseManager.cs for connection management
- Added QueryDialog.cs with SQL editor and Excel export
- Added CreateTableDialog.cs for interactive table creation
- Added RowEditorDialog.cs for data editing
- Added RecentDatabases.cs for recent database tracking
- Dependencies changed:
  - Removed: Microsoft.Data.Sqlite 10.0.0
  - Added: SourceGear.sqlite3 3.50.4.5
  - Added: ZidUtilities.CommonCode.DataAccess.Sqlite 1.0.0
- SqliteConnector.cs excluded from build (using ZidUtilities version)

**Impact:** Application evolved from pure design tool to full-featured database designer + manager

### 2025-12-01 (Session 1 - Bug Investigation & Fix)

**User Issue:** objectInfoTextBox not showing information when clicking on tree view nodes

**Investigation:**
- Verified event handler `databaseTreeView_AfterSelect` is properly wired up in Designer (line 342 of MainForm.Designer.cs)
- Verified node tags are set correctly in `LoadDatabaseStructure()` method (format: "table:tableName", "view:viewName", "index:indexName")
- Verified DatabaseManager methods exist: `GetTableSchema()`, `GetViewSchema()`, `GetIndexSchema()`
- Added missing package: `System.Data.SQLite.Core 1.0.119` (was referenced in code but not in project file)
- User testing revealed: "connection not open" error

**Root Cause:**
The `TestConnection()` method in `OpenDatabase()` was closing the connection after testing, leaving all subsequent database operations with a closed connection. The schema methods were failing with "connection not open" errors.

**Solution Implemented:**
1. Added connection state check after `TestConnection()` in `OpenDatabase()` method (DatabaseManager.cs:40-44)
2. Created `EnsureConnectionOpen()` helper method to verify and open connection before operations (DatabaseManager.cs:15-21)
3. Added `EnsureConnectionOpen()` calls to all database methods:
   - `GetTableNames()`, `GetViewNames()`, `GetIndexNames()`
   - `GetTableColumnInfo()`, `GetTableSchema()`, `GetViewSchema()`, `GetIndexSchema()`
   - `GetTableData()`, `ExecuteNonQuery()`

**Changes Made:**
- Added `System.Data.SQLite.Core 1.0.119` NuGet package
- Modified DatabaseManager.cs: Added connection state management
- Temporarily added/removed diagnostic MessageBoxes for troubleshooting

**Status:** ✅ Fixed - Connection now stays open and schema information displays correctly

---

## Notes for Future Sessions

This file captures important project context, architectural decisions, and key information that should persist across Claude Code sessions. It will be updated with:

- New feature decisions and implementations
- Architectural changes
- Important bugs and fixes
- Design patterns used
- Dependencies added/removed
- Key learnings and gotchas

This is **not** a verbatim transcript but a curated memory of what matters for development.
