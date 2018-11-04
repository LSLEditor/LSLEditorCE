REM cd "C:\Documents and Settings\Alphons\Mijn documenten\Visual Studio 2005\Projects\LSLEditor\bin\Debug"
cd "bin\Debug"

del d:\temp\LsLEditor.zip
del d:\temp\LsLEditor.exe.bz2
del d:\temp\LsLEditor.exe.gz

"c:\Program Files\7-Zip\7z" a -tzip d:\temp\LsLEditor.zip LsLEditor.exe
"c:\Program Files\7-Zip\7z" a -tbzip2 d:\temp\LsLEditor.exe.bz2 LsLEditor.exe
"c:\Program Files\7-Zip\7z" a -tgzip d:\temp\LsLEditor.exe.gz LsLEditor.exe

pause
