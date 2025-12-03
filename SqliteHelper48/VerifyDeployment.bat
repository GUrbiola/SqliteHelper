@echo off
echo ========================================
echo SqliteHelper48 Deployment Verification
echo ========================================
echo.

echo Checking deployment folder: %~dp0
echo.

echo === Required Files Check ===
echo.

if exist "%~dp0SqliteHelper48.exe" (
    echo [OK] SqliteHelper48.exe found
) else (
    echo [MISSING] SqliteHelper48.exe NOT FOUND!
)

if exist "%~dp0SqliteHelper48.exe.config" (
    echo [OK] SqliteHelper48.exe.config found
) else (
    echo [MISSING] SqliteHelper48.exe.config NOT FOUND!
    echo          ^(This file is CRITICAL for the app to run^)
)

echo.
echo === System.* Runtime DLLs Check ===
echo.

if exist "%~dp0System.ValueTuple.dll" (
    echo [OK] System.ValueTuple.dll found
    dir "%~dp0System.ValueTuple.dll" | find "25,232"
    if errorlevel 1 (
        echo     WARNING: File size doesn't match expected runtime DLL ^(should be 25,232 bytes^)
        echo     Current size:
        dir "%~dp0System.ValueTuple.dll" | find ".dll"
    ) else (
        echo     File size CORRECT ^(25,232 bytes - runtime assembly^)
    )
) else (
    echo [MISSING] System.ValueTuple.dll NOT FOUND!
)

if exist "%~dp0System.Buffers.dll" (
    echo [OK] System.Buffers.dll found
) else (
    echo [MISSING] System.Buffers.dll NOT FOUND!
)

if exist "%~dp0System.Memory.dll" (
    echo [OK] System.Memory.dll found
) else (
    echo [MISSING] System.Memory.dll NOT FOUND!
)

if exist "%~dp0System.Runtime.CompilerServices.Unsafe.dll" (
    echo [OK] System.Runtime.CompilerServices.Unsafe.dll found
) else (
    echo [MISSING] System.Runtime.CompilerServices.Unsafe.dll NOT FOUND!
)

if exist "%~dp0System.Numerics.Vectors.dll" (
    echo [OK] System.Numerics.Vectors.dll found
) else (
    echo [MISSING] System.Numerics.Vectors.dll NOT FOUND!
)

echo.
echo === Other Required DLLs ===
echo.

if exist "%~dp0SQLitePCLRaw.batteries_v2.dll" (
    echo [OK] SQLitePCLRaw.batteries_v2.dll found
) else (
    echo [MISSING] SQLitePCLRaw.batteries_v2.dll NOT FOUND!
)

if exist "%~dp0SQLitePCLRaw.core.dll" (
    echo [OK] SQLitePCLRaw.core.dll found
) else (
    echo [MISSING] SQLitePCLRaw.core.dll NOT FOUND!
)

if exist "%~dp0SQLitePCLRaw.provider.dynamic_cdecl.dll" (
    echo [OK] SQLitePCLRaw.provider.dynamic_cdecl.dll found
) else (
    echo [MISSING] SQLitePCLRaw.provider.dynamic_cdecl.dll NOT FOUND!
)

if exist "%~dp0Microsoft.Data.Sqlite.dll" (
    echo [OK] Microsoft.Data.Sqlite.dll found
) else (
    echo [MISSING] Microsoft.Data.Sqlite.dll NOT FOUND!
)

echo.
echo === Native SQLite Libraries ===
echo.

if exist "%~dp0runtimes\win-x64\native\e_sqlite3.dll" (
    echo [OK] runtimes\win-x64\native\e_sqlite3.dll found
) else (
    echo [MISSING] runtimes\win-x64\native\e_sqlite3.dll NOT FOUND!
)

if exist "%~dp0runtimes\win-x86\native\e_sqlite3.dll" (
    echo [OK] runtimes\win-x86\native\e_sqlite3.dll found
) else (
    echo [MISSING] runtimes\win-x86\native\e_sqlite3.dll NOT FOUND!
)

echo.
echo === Summary ===
echo.
echo All files checked. Review above for any [MISSING] files.
echo.
echo If System.ValueTuple.dll shows wrong size, your installer is
echo copying the REFERENCE assembly instead of the RUNTIME assembly!
echo.
echo Press any key to exit...
pause > nul
