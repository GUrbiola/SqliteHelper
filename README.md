# SqliteHelper48

**A powerful, visual SQLite database designer and manager for Windows**

SqliteHelper48 is a comprehensive desktop application that lets you design SQLite databases visually using diagrams, generate the actual database files, and then manage them with a full-featured database manager—all in one tool.

---

## 📋 Table of Contents

- [What Does This App Do?](#what-does-this-app-do)
- [Key Features](#key-features)
- [Installation](#installation)
- [Complete User Guide](#complete-user-guide)
  - [Part 1: Designing Your Database](#part-1-designing-your-database)
  - [Part 2: Managing Your Database](#part-2-managing-your-database)
  - [Part 3: Advanced Query Editor](#part-3-advanced-query-editor)
- [Understanding Visual Relationships](#understanding-visual-relationships)
- [Feature Reference](#feature-reference)
- [Technical Details](#technical-details)
- [FAQ](#faq)
- [Troubleshooting](#troubleshooting)

---

## What Does This App Do?

SqliteHelper48 is designed to make working with SQLite databases easier through **two complementary modes**:

### 🎨 **Design Mode** - Visual Database Designer
Create database schemas without writing SQL. Draw tables on a canvas, define columns with point-and-click, and add visual relationship lines to document how tables connect. When you're ready, generate the SQL script and create your database file with one click.

### 🗄️ **Management Mode** - Database Manager
Once your database exists, switch to management mode to browse tables, edit data, create new tables, add views, and run queries with an advanced SQL editor that includes autocomplete, syntax highlighting, and snippets.

**Think of it as:** A visual ERD (Entity Relationship Diagram) tool combined with a database administration tool, specifically built for SQLite.

---

## Key Features

### Visual Database Designer
- ✅ **Drag-and-drop table designer** - Create tables visually on a canvas
- ✅ **Column editor** - Define columns, data types, primary keys, and constraints through dialogs
- ✅ **Relationship lines** - Draw lines between tables to show how they relate (for documentation)
- ✅ **Comments and annotations** - Add text boxes to document your design
- ✅ **SQL generation** - Automatically creates CREATE TABLE scripts from your diagram
- ✅ **Project files (.shlp)** - Save and reopen your designs

### Database Management
- ✅ **Database browser** - Tree view showing all tables, views, and indexes
- ✅ **Schema viewer** - Click any table to see its structure and column details
- ✅ **Data grid** - View and browse table data in a spreadsheet-like interface
- ✅ **Interactive table creation** - Create new tables with live SQL preview
- ✅ **Row editor** - Add and edit individual rows in any table
- ✅ **View management** - Create and manage database views

### Professional Query Editor
- ✅ **Syntax highlighting** - SQL keywords, strings, and comments are color-coded
- ✅ **Autocomplete** - IntelliSense-style completion for table names, columns, and SQL keywords
- ✅ **Code snippets** - Type shortcuts like `sel` + Tab to insert `SELECT * FROM table`
- ✅ **Export to Excel** - One-click export of query results to .xlsx files
- ✅ **Multiple queries** - Execute multiple SQL statements in one go
- ✅ **Results grid** - View query results in a sortable data grid

### Project Management
- ✅ **Recent projects list** - Quick access to your last 10 design projects
- ✅ **Recent databases list** - Quick access to your last 10 opened databases
- ✅ **Dual mode switching** - Seamlessly switch between design and management modes

---

## Installation

### System Requirements
- **Operating System:** Windows 7 or later (Windows 10/11 recommended)
- **.NET Framework:** 4.8 or higher
- **Disk Space:** ~50 MB
- **Memory:** 512 MB RAM minimum, 1 GB recommended

### Installation Steps
1. Download the latest release (SqliteHelper48.zip)
2. Extract all files to a folder of your choice (e.g., `C:\Program Files\SqliteHelper48`)
3. Run `SqliteHelper48.exe`
4. No installation or administrator rights required

---

## Complete User Guide

### Part 1: Designing Your Database

#### Step 1: Create a New Project

1. **Launch SqliteHelper48** by double-clicking the executable
2. You'll see the main window with a toolbar and empty workspace
3. **Click File → New Project** or press `Ctrl+N`
4. Enter a name for your project (e.g., "CustomerDatabase")
5. Click OK

**What you'll see:** A blank white canvas (the diagram designer) with a toolbar at the top.

---

#### Step 2: Add Tables to Your Diagram

1. **Right-click on the blank canvas**
2. Select **"Add Table"** from the context menu
3. A new table box appears on the canvas with a default name like "Table1"

**What a table looks like:** A rectangular box with:
- Title bar (table name)
- Column list area (initially empty)
- Resize handles on the corners

---

#### Step 3: Define Table Structure

1. **Double-click the table** you just created
2. The **Table Editor Dialog** opens with these tabs:
   - **General:** Set table name (e.g., "Customers")
   - **Columns:** Add and configure columns

3. **Click "Add Column"** to add a new column
4. For each column, specify:
   - **Column Name** (e.g., "CustomerID", "FirstName", "Email")
   - **Data Type** (TEXT, INTEGER, REAL, BLOB, NUMERIC)
   - **Primary Key** (check the box for ID columns)
   - **Not Null** (require a value)
   - **Unique** (no duplicates allowed)
   - **Default Value** (optional)

**Example - Creating a Customers table:**
```
CustomerID    INTEGER    [✓] Primary Key  [✓] Not Null
FirstName     TEXT       [ ] Primary Key  [✓] Not Null
LastName      TEXT       [ ] Primary Key  [✓] Not Null
Email         TEXT       [ ] Primary Key  [ ] Not Null  [✓] Unique
CreatedDate   TEXT       [ ] Primary Key  [ ] Not Null  Default: CURRENT_TIMESTAMP
```

5. **Click OK** to save the table definition
6. The table box now shows all your columns

---

#### Step 4: Add More Tables

Repeat steps 2-3 to create additional tables. For example, create:
- **Orders** table (OrderID, CustomerID, OrderDate, TotalAmount)
- **Products** table (ProductID, ProductName, Price, StockQuantity)
- **OrderItems** table (OrderItemID, OrderID, ProductID, Quantity)

**Tip:** You can **drag tables around** the canvas to organize them visually.

---

#### Step 5: Draw Visual Relationships

1. **Click the "Add Relationship" button** in the toolbar (or right-click and select "Add Relationship")
2. **Click on the first table** (e.g., Customers)
3. **Click on the second table** (e.g., Orders)
4. The **Relationship Dialog** appears:
   - **From Table:** Customers
   - **To Table:** Orders
   - **Relationship Type:** One-to-Many (or other types)
   - **From Column:** CustomerID
   - **To Column:** CustomerID

5. **Click OK** to create the relationship
6. A **line with an arrow** appears connecting the two tables

**Repeat for all relationships:**
- Customers → Orders (One customer can have many orders)
- Orders → OrderItems (One order can have many items)
- Products → OrderItems (One product can appear in many order items)

---

#### Step 6: Add Comments (Optional)

1. **Right-click on the canvas**
2. Select **"Add Comment"**
3. A text box appears where you can type notes like:
   - "Customer table stores basic contact info"
   - "Orders are linked to customers via CustomerID"
   - "TODO: Add indexes on foreign keys"

---

#### Step 7: Save Your Project

1. **Click File → Save Project** or press `Ctrl+S`
2. Choose a location and filename (e.g., "CustomerDatabase.shlp")
3. Click Save

**What's saved:** All tables, columns, relationships, comments, and their positions on the canvas.

---

#### Step 8: Generate SQL Script

1. **Click the "Generate SQL" button** in the toolbar
2. A window appears showing the complete SQL script:

```sql
CREATE TABLE Customers (
    CustomerID INTEGER PRIMARY KEY NOT NULL,
    FirstName TEXT NOT NULL,
    LastName TEXT NOT NULL,
    Email TEXT UNIQUE,
    CreatedDate TEXT DEFAULT CURRENT_TIMESTAMP
);

CREATE TABLE Orders (
    OrderID INTEGER PRIMARY KEY NOT NULL,
    CustomerID INTEGER NOT NULL,
    OrderDate TEXT NOT NULL,
    TotalAmount REAL
);

-- ... more tables ...
```

3. **Review the SQL** to ensure it matches your design
4. You can **copy this script** to use elsewhere, or proceed to create the database

---

#### Step 9: Create the Database File

1. **Click "Create Database" button** in the toolbar
2. Choose a save location and filename (e.g., "CustomerDatabase.db")
3. Click Save
4. SqliteHelper48 executes all CREATE TABLE statements
5. **Success!** You now have a working SQLite database file

---

### Part 2: Managing Your Database

Once your database is created, you can manage it using the built-in database manager.

#### Step 1: Open a Database

1. **Click File → Open Database** or press `Ctrl+O`
2. Browse to your .db file (e.g., "CustomerDatabase.db")
3. Click Open

**What you'll see:** The main window switches to **Management Mode** with:
- **Left Panel:** Tree view showing Tables, Views, and Indexes
- **Right Panel (top):** Object info text box showing schema details
- **Right Panel (bottom):** Data grid showing table contents

---

#### Step 2: Browse Database Structure

**Tree View Navigation:**
```
📂 CustomerDatabase.db
  ├── 📁 Tables
  │   ├── 📋 Customers
  │   ├── 📋 Orders
  │   ├── 📋 Products
  │   └── 📋 OrderItems
  ├── 📁 Views
  └── 📁 Indexes
```

**Click on any table** (e.g., Customers) and you'll see:
- **Top panel:** Schema information (column names, types, constraints)
- **Bottom panel:** All rows from that table in a grid

**Example output in info panel:**
```
Table: Customers
Columns:
  CustomerID (INTEGER) - PRIMARY KEY
  FirstName (TEXT) - NOT NULL
  LastName (TEXT) - NOT NULL
  Email (TEXT) - UNIQUE
  CreatedDate (TEXT)
```

---

#### Step 3: View Table Data

When you click a table in the tree view, the **data grid** shows all rows:

```
| CustomerID | FirstName | LastName  | Email                | CreatedDate         |
|------------|-----------|-----------|----------------------|---------------------|
| 1          | John      | Smith     | john@example.com     | 2024-01-15 10:30:00 |
| 2          | Jane      | Doe       | jane@example.com     | 2024-01-16 14:22:00 |
| 3          | Bob       | Johnson   | bob@example.com      | 2024-01-17 09:15:00 |
```

**You can:**
- Scroll through all rows
- Sort by any column (click column header)
- Resize columns by dragging headers

---

#### Step 4: Add New Data

1. **Click the "Edit Data" button** in the toolbar (or right-click table → Edit Data)
2. The **Row Editor Dialog** opens
3. **Click "Add New Row"**
4. Fill in values for each column:
   - CustomerID: 4
   - FirstName: Alice
   - LastName: Williams
   - Email: alice@example.com
5. **Click Save**
6. The new row appears in the data grid

---

#### Step 5: Edit Existing Data

1. **Open the Row Editor** (Edit Data button)
2. **Select a row** from the list
3. **Modify any field** (e.g., change email address)
4. **Click Save**
5. Changes are immediately written to the database

---

#### Step 6: Create New Tables

Even after the database is created, you can add more tables:

1. **Click "Create Table" button** in the toolbar
2. The **Create Table Dialog** opens with:
   - **Table Name** field
   - **Column Editor** (add/remove columns)
   - **SQL Preview** pane (live preview of CREATE TABLE statement)

3. **Enter table name** (e.g., "Suppliers")
4. **Add columns:**
   - SupplierID (INTEGER, Primary Key)
   - CompanyName (TEXT, Not Null)
   - ContactEmail (TEXT)

5. **Watch the SQL preview update** in real-time:
```sql
CREATE TABLE Suppliers (
    SupplierID INTEGER PRIMARY KEY,
    CompanyName TEXT NOT NULL,
    ContactEmail TEXT
);
```

6. **Click "Create"** to execute the statement
7. The new table appears in the tree view

---

#### Step 7: Manage Views

**Creating a View:**
1. **Click "Query" button** to open the query editor
2. Write a CREATE VIEW statement:
```sql
CREATE VIEW CustomerOrderSummary AS
SELECT
    c.CustomerID,
    c.FirstName || ' ' || c.LastName AS FullName,
    COUNT(o.OrderID) AS TotalOrders,
    SUM(o.TotalAmount) AS TotalSpent
FROM Customers c
LEFT JOIN Orders o ON c.CustomerID = o.CustomerID
GROUP BY c.CustomerID;
```
3. **Click Execute** (F5)
4. The view is created and appears under "Views" in the tree view

**Viewing the Data:**
1. **Click the view** in the tree view
2. The data grid shows the view's result set (just like a table)

---

### Part 3: Advanced Query Editor

The built-in query editor is where SqliteHelper48 really shines.

#### Opening the Query Editor

1. **Click the "Query" button** in the toolbar
2. Or press `Ctrl+Q`
3. The **Query Dialog** opens with:
   - **Top panel:** SQL editor with syntax highlighting
   - **Bottom panel:** Results grid
   - **Toolbar:** Execute, Export, Save buttons

---

#### Feature 1: Syntax Highlighting

As you type SQL, different elements are automatically color-coded:

```sql
SELECT CustomerID, FirstName, LastName    ← Keywords in blue
FROM Customers                            ← Table names in different color
WHERE Email LIKE '%@example.com'          ← Strings in red
AND CreatedDate > '2024-01-01'           ← Operators highlighted
-- This is a comment                     ← Comments in green
```

**This makes it easier to:**
- Spot typos and syntax errors
- Read complex queries
- Distinguish between keywords and identifiers

---

#### Feature 2: Autocomplete

**How it works:**
1. Start typing a query: `SELECT * FROM Cust`
2. **Press Ctrl+Space** (or wait a moment)
3. An **autocomplete popup** appears showing:
   ```
   Customers
   CustomerOrderSummary
   ```
4. **Use arrow keys** to select "Customers"
5. **Press Enter** to insert it

**What gets autocompleted:**
- ✅ **Table names** - All tables in the database
- ✅ **View names** - All views
- ✅ **Column names** - All columns from referenced tables
- ✅ **SQL keywords** - SELECT, FROM, WHERE, JOIN, etc.
- ✅ **Functions** - COUNT, SUM, AVG, MAX, MIN, etc.

**Example workflow:**
```
Type: SEL [Ctrl+Space]
   → Shows: SELECT

Type: SELECT * FR [Ctrl+Space]
   → Shows: FROM

Type: SELECT * FROM Ord [Ctrl+Space]
   → Shows: Orders, OrderItems
```

---

#### Feature 3: Code Snippets

**Snippets are pre-built SQL templates** that you can insert by typing a shortcut and pressing Tab.

**Built-in Snippets:**

| Shortcut | Press Tab | Result |
|----------|-----------|--------|
| `sel` | Tab | `SELECT * FROM tableName WHERE condition` |
| `ins` | Tab | `INSERT INTO tableName (column1, column2) VALUES (value1, value2)` |
| `upd` | Tab | `UPDATE tableName SET column1 = value1 WHERE condition` |
| `del` | Tab | `DELETE FROM tableName WHERE condition` |
| `join` | Tab | `SELECT * FROM table1 INNER JOIN table2 ON table1.id = table2.id` |
| `left` | Tab | `SELECT * FROM table1 LEFT JOIN table2 ON table1.id = table2.id` |
| `create` | Tab | `CREATE TABLE tableName (id INTEGER PRIMARY KEY, ...)` |
| `view` | Tab | `CREATE VIEW viewName AS SELECT ...` |
| `idx` | Tab | `CREATE INDEX indexName ON tableName (column)` |

**Example Usage:**

1. **Type:** `sel` (just those 3 letters)
2. **Press Tab** (not Enter!)
3. **The editor inserts:**
```sql
SELECT * FROM tableName WHERE condition
```
4. **Your cursor** is positioned at "tableName" (ready to replace)
5. **Type the table name** and continue editing

**Real-world example:**
```
1. Type: sel [Tab]
   → SELECT * FROM tableName WHERE condition

2. Replace "tableName" with "Customers"
   → SELECT * FROM Customers WHERE condition

3. Replace "condition" with "CreatedDate > '2024-01-01'"
   → SELECT * FROM Customers WHERE CreatedDate > '2024-01-01'

4. Press F5 to execute
   → Results appear below
```

**This saves you from typing repetitive SQL structures over and over.**

---

#### Feature 4: Executing Queries

**Single Query:**
1. Write a query:
```sql
SELECT FirstName, LastName, Email
FROM Customers
WHERE LastName LIKE 'S%'
ORDER BY FirstName;
```
2. **Press F5** or click **Execute**
3. Results appear in the grid below:

```
| FirstName | LastName | Email              |
|-----------|-----------|--------------------|
| John      | Smith    | john@example.com   |
| Sarah     | Stevens  | sarah@example.com  |
```

**Multiple Queries:**
You can run multiple statements at once (separated by semicolons):

```sql
SELECT COUNT(*) AS TotalCustomers FROM Customers;
SELECT COUNT(*) AS TotalOrders FROM Orders;
SELECT SUM(TotalAmount) AS Revenue FROM Orders;
```

All three results are displayed sequentially.

---

#### Feature 5: Export to Excel

After executing a query:

1. **Click the "Export to Excel" button** in the toolbar
2. Choose a save location (e.g., "CustomerReport.xlsx")
3. Click Save
4. **An Excel file is created** with:
   - First row: Column headers
   - Remaining rows: All query results
   - Automatic column sizing

**Use cases:**
- Share query results with non-technical users
- Create reports for management
- Import data into other tools (Excel, Power BI, etc.)
- Backup specific data subsets

---

#### Feature 6: Save and Load Queries

**Save a Query:**
1. Write a complex query you'll use again
2. Click **File → Save Query** or press `Ctrl+S`
3. Save as a .sql file (e.g., "MonthlyReport.sql")

**Load a Query:**
1. Click **File → Open Query**
2. Select your saved .sql file
3. The query loads into the editor

**Benefit:** Build a library of commonly-used queries for different reports and analyses.

---

## Understanding Visual Relationships

### ⚠️ Important: Relationships Are Visual Only

When you draw relationship lines in the diagram designer, they serve as **documentation and visualization tools**. They help you and your team understand the database structure, especially when dealing with large databases with many tables.

**What relationship lines DO:**
- ✅ Show up in the diagram for easy visualization
- ✅ Help you understand how tables connect conceptually
- ✅ Are saved in the .shlp project file
- ✅ Make diagrams easier to read and share with others
- ✅ Document the intended relationships between tables

**What relationship lines DO NOT do:**
- ❌ Create FOREIGN KEY constraints in the database
- ❌ Enforce referential integrity
- ❌ Prevent deletion of related records
- ❌ Automatically create indexes
- ❌ Affect how queries or joins work

### Why This Design Decision?

SQLite supports foreign keys, but they're:
- Optional (must be enabled with `PRAGMA foreign_keys = ON`)
- Can complicate data import/export
- May not be needed for all use cases
- Can be added manually if desired

**SqliteHelper48 leaves this choice to you.** You can:
1. Use relationships purely for documentation (most common)
2. Manually add FOREIGN KEY constraints in the table editor if needed
3. Add them via SQL queries in the query editor

### How to Add Real Foreign Keys (If Needed)

If you want actual foreign key constraints:

**Option 1: In the Table Editor**
When defining columns, you can add a foreign key constraint manually in the SQL that gets generated.

**Option 2: Via Query Editor**
```sql
CREATE TABLE Orders (
    OrderID INTEGER PRIMARY KEY,
    CustomerID INTEGER NOT NULL,
    OrderDate TEXT,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
```

**Option 3: Alter Existing Tables**
SQLite doesn't support ALTER TABLE for adding foreign keys, so you'd need to:
1. Create a new table with the constraint
2. Copy data from the old table
3. Drop the old table
4. Rename the new table

---

## Feature Reference

### Main Window (Design Mode)

| Button | Function | Shortcut |
|--------|----------|----------|
| New Project | Create a new database design | Ctrl+N |
| Open Project | Open an existing .shlp project | Ctrl+O |
| Save Project | Save current design | Ctrl+S |
| Add Table | Add a table to the diagram | Right-click |
| Add Relationship | Draw a line between tables | - |
| Add Comment | Add a text annotation | Right-click |
| Generate SQL | Preview CREATE TABLE scripts | - |
| Create Database | Generate the actual .db file | - |

### Main Window (Management Mode)

| Button | Function | Shortcut |
|--------|----------|----------|
| Open Database | Open a SQLite .db file | Ctrl+O |
| Query | Open the query editor | Ctrl+Q |
| Create Table | Add a new table to the database | - |
| Edit Data | Open row editor for selected table | - |
| Refresh | Reload database structure | F5 |

### Query Editor

| Button | Function | Shortcut |
|--------|----------|----------|
| Execute | Run the SQL query | F5 |
| Export to Excel | Save results as .xlsx | - |
| Save Query | Save SQL to a file | Ctrl+S |
| Open Query | Load SQL from a file | Ctrl+O |
| Clear | Clear the editor | - |
| Format | Auto-format SQL (if available) | - |

### Keyboard Shortcuts

#### Global
- `Ctrl+N` - New project
- `Ctrl+O` - Open project/database/query
- `Ctrl+S` - Save project/query
- `Ctrl+Q` - Open query editor
- `F5` - Refresh / Execute

#### Query Editor
- `Ctrl+Space` - Trigger autocomplete
- `Tab` - Expand snippet (after typing shortcut)
- `F5` - Execute query
- `Ctrl+E` - Execute query (alternative)
- `Ctrl+A` - Select all
- `Ctrl+C` - Copy
- `Ctrl+V` - Paste
- `Ctrl+Z` - Undo
- `Ctrl+Y` - Redo

---

## Technical Details

### Technology Stack

- **.NET Framework 4.8** - Application framework
- **Windows Forms** - UI framework
- **SourceGear.sqlite3 3.50.4.5** - SQLite database engine (native)
- **System.Data.SQLite.Core 1.0.119** - SQLite ADO.NET provider
- **ICSharpCode.TextEditor** - Syntax highlighting component
- **ClosedXML.Excel 0.95.4** - Excel export library
- **ZidUtilities.CommonCode** - Custom utility libraries
- **ZidUtilities.CommonCode.DataAccess.Sqlite 1.0.0** - SQLite data access layer

### File Formats

**Project Files (.shlp)**
- XML-based format storing database design
- Contains table definitions, relationships, positions
- Can be version-controlled (text-based)

**Database Files (.db, .sqlite, .sqlite3)**
- Standard SQLite 3 database format
- Compatible with all SQLite tools
- Can be opened in SQLiteStudio, DB Browser for SQLite, etc.

**Query Files (.sql)**
- Plain text SQL scripts
- Standard SQL syntax
- Can be edited in any text editor

### Supported SQLite Features

**Data Types:**
- INTEGER - Signed integer (1, 2, 3, 4, 6, or 8 bytes)
- REAL - Floating point (8-byte IEEE)
- TEXT - Text string (UTF-8, UTF-16BE, or UTF-16LE)
- BLOB - Binary data (stored exactly as input)
- NUMERIC - Affinity (stores as INTEGER, REAL, or TEXT)

**Constraints:**
- PRIMARY KEY
- NOT NULL
- UNIQUE
- CHECK
- DEFAULT
- FOREIGN KEY (must add manually)

**SQL Support:**
- SELECT, INSERT, UPDATE, DELETE
- CREATE TABLE, DROP TABLE, ALTER TABLE
- CREATE VIEW, DROP VIEW
- CREATE INDEX, DROP INDEX
- Transactions (BEGIN, COMMIT, ROLLBACK)
- JOINs (INNER, LEFT, RIGHT, CROSS)
- Subqueries and CTEs (WITH clause)
- Aggregate functions (COUNT, SUM, AVG, MIN, MAX)
- Window functions (supported in SQLite 3.25+)

### Limitations

- **Windows Only** - Requires Windows 7 or later
- **SQLite Only** - Does not support other databases (MySQL, PostgreSQL, etc.)
- **No Multi-User** - SQLite is file-based, not designed for concurrent writes
- **No Stored Procedures** - SQLite doesn't support stored procedures
- **No Triggers in Designer** - Triggers must be created via SQL
- **No Auto-Generated Foreign Keys** - Must add manually if needed

---

## FAQ

### Q: Can I open databases created by other SQLite tools?
**A:** Yes! SqliteHelper48 can open any standard SQLite 3 database file (.db, .sqlite, .sqlite3). If you created a database using DB Browser for SQLite, SQLiteStudio, or any other tool, you can open and manage it in SqliteHelper48.

### Q: Can other tools open databases I create?
**A:** Absolutely. SqliteHelper48 creates standard SQLite database files. They can be opened in:
- DB Browser for SQLite
- SQLiteStudio
- DataGrip
- Python (sqlite3 module)
- Any programming language with SQLite support

### Q: Do I need to install SQLite separately?
**A:** No. SQLite is embedded in the application. Everything you need is included.

### Q: Can I use this for large databases?
**A:** SqliteHelper48 works with databases of any size that SQLite supports (up to 281 terabytes theoretically). However, very large result sets (millions of rows) may take time to load in the data grid. Use WHERE clauses to limit results.

### Q: Are the relationship lines in diagrams enforced?
**A:** No. See the [Understanding Visual Relationships](#understanding-visual-relationships) section. They're for documentation only and don't create foreign key constraints.

### Q: How do I add real foreign key constraints?
**A:** You have two options:
1. Add them manually in the CREATE TABLE SQL (in the table editor or query editor)
2. Use the query editor to execute CREATE TABLE with FOREIGN KEY clauses

Example:
```sql
CREATE TABLE Orders (
    OrderID INTEGER PRIMARY KEY,
    CustomerID INTEGER,
    FOREIGN KEY (CustomerID) REFERENCES Customers(CustomerID)
);
```

### Q: Can I export data to formats other than Excel?
**A:** Currently only Excel (.xlsx) export is built-in. However, you can:
- Copy data from the results grid and paste into any application
- Use the query editor to create INSERT statements
- Export to Excel, then convert using Excel

### Q: Can I import data from Excel or CSV?
**A:** Not directly in the current version. You can:
1. Use the query editor to write INSERT statements
2. Use external tools like DB Browser for SQLite for bulk imports
3. Use the row editor for small amounts of data

### Q: How do I create indexes?
**A:** Use the query editor:
```sql
CREATE INDEX idx_customer_email ON Customers(Email);
CREATE INDEX idx_order_date ON Orders(OrderDate);
```

### Q: Can I create triggers?
**A:** Yes, via the query editor:
```sql
CREATE TRIGGER update_timestamp
AFTER UPDATE ON Customers
BEGIN
    UPDATE Customers SET ModifiedDate = CURRENT_TIMESTAMP
    WHERE CustomerID = NEW.CustomerID;
END;
```

### Q: How do I backup my database?
**A:** SQLite databases are single files. Simply copy the .db file to a backup location. You can also:
1. Use Windows File Explorer to copy the file
2. Use the "Generate SQL" feature to export the schema
3. Use the query editor with `.backup` command (if supported)

### Q: What's the difference between a project (.shlp) and a database (.db)?
**A:**
- **.shlp** = Design project (diagram, visual relationships, not a working database)
- **.db** = Actual SQLite database (created from the design, contains real data)

Think of .shlp as "blueprints" and .db as the "finished building."

### Q: Can I edit the diagram after creating the database?
**A:** Yes, but changes to the .shlp project won't automatically update the .db file. You'd need to:
1. Regenerate the database (loses data), or
2. Use ALTER TABLE statements in the query editor to modify the existing database

### Q: Where are recent projects and databases stored?
**A:** In configuration files in your user profile directory. You can clear them from the File menu.

---

## Troubleshooting

### Issue: "Database is locked" error

**Cause:** Another application has the database file open.

**Solution:**
1. Close any other SQLite tools (DB Browser, SQLiteStudio, etc.)
2. Make sure no programs are querying the database
3. Restart SqliteHelper48
4. If problem persists, restart Windows

---

### Issue: Autocomplete not showing in query editor

**Solutions:**
- Press `Ctrl+Space` manually to trigger it
- Make sure you've opened a database (autocomplete needs table names)
- Try typing a few more characters
- Check that the database connection is active

---

### Issue: Snippet not expanding when I press Tab

**Solutions:**
- Make sure you typed the exact shortcut (e.g., `sel`, not `SELECT`)
- Press Tab immediately after typing the shortcut (before typing a space)
- Check that snippets are enabled in settings (if option exists)
- Try typing the shortcut again

---

### Issue: Can't see table data after clicking in tree view

**Cause:** Database connection may be closed.

**Solution:**
1. Close and reopen the database
2. Check the object info panel for any error messages
3. Verify the database file exists and isn't corrupted
4. Try opening the database in another SQLite tool to verify it's valid

---

### Issue: Query results not exporting to Excel

**Solutions:**
- Make sure you executed the query first (results must be visible)
- Check that you have write permissions to the save location
- Try saving to a different location (e.g., Desktop)
- Make sure Excel isn't already open with a file of the same name

---

### Issue: Relationship lines not appearing in diagram

**Solutions:**
- Make sure you completed the relationship dialog (clicked OK)
- Check that both tables still exist in the diagram
- Try zooming out (line might be outside visible area)
- Save and reopen the project

---

### Issue: Generated SQL doesn't match my design

**Solutions:**
- Double-check table definitions in the table editor
- Make sure you saved changes to all tables
- Try closing and reopening the project
- Check for any error messages when generating SQL

---

### Issue: Application won't start

**Solutions:**
1. Verify .NET Framework 4.8 is installed:
   - Open Control Panel → Programs → Programs and Features
   - Look for "Microsoft .NET Framework 4.8"
   - If missing, download from Microsoft's website

2. Check system requirements:
   - Windows 7 or later
   - 512 MB RAM minimum
   - Administrator rights (if running from Program Files)

3. Try running as administrator:
   - Right-click SqliteHelper48.exe
   - Select "Run as administrator"

4. Check Event Viewer for error details:
   - Press Win+X → Event Viewer
   - Navigate to Windows Logs → Application
   - Look for errors from SqliteHelper48

---

## Use Cases

### 1. Rapid Prototyping
Design a database schema visually, generate it, and start testing queries—all in under 10 minutes.

### 2. Learning SQL and Databases
Students can see the relationship between visual design and SQL, experiment with queries, and learn by doing.

### 3. Small Business Applications
Create and manage databases for inventory, customers, orders, etc. without needing a full database server.

### 4. Data Analysis
Open existing SQLite databases, run queries, and export results to Excel for further analysis.

### 5. Documentation
Create visual diagrams of existing databases to document structure for teams and stakeholders.

### 6. Database Migration Planning
Design the target schema visually before performing data migrations.

### 7. Report Generation
Write queries for common reports, save them as .sql files, and reuse them to generate Excel reports.

---

## Contributing

This is a personal project by Gonzalo. If you encounter bugs or have feature suggestions, feel free to reach out.

---

## License

[Specify your license here - e.g., MIT, GPL, proprietary, etc.]

---

## Acknowledgments

- **SQLite** - Public domain database engine by D. Richard Hipp and contributors
- **ICSharpCode.TextEditor** - Open source text editor component
- **ClosedXML** - .NET library for creating Excel files without Microsoft Excel
- **SourceGear** - SQLite wrapper for .NET

---

## Version History

### Version 1.0 (Current)
- Initial release
- Visual database designer with diagram support
- Full database management capabilities
- Query editor with autocomplete, syntax highlighting, and snippets
- Excel export functionality
- Project file support (.shlp)
- Recent projects and databases tracking

---

**Built with ❤️ using .NET Framework 4.8 and Windows Forms**

**Platform:** Windows 7/8/10/11
**Framework:** .NET Framework 4.8
**Database:** SQLite 3

For questions, issues, or feedback, please contact the developer.
