cd "C:\Documents and Settings\Alphons\Mijn documenten\Visual Studio 2005\Projects\LSLEditor\bin\Debug"

copy LsLEditor.exe LsLEditor-beta.exe

del d:\temp\LsLEditor-beta.zip
del d:\temp\LsLEditor-beta.exe.bz2
del d:\temp\LsLEditor-beta.exe.gz

"c:\Program Files\7-Zip\7z" a -tzip d:\temp\LsLEditor-beta.zip LsLEditor-beta.exe
"c:\Program Files\7-Zip\7z" a -tbzip2 d:\temp\LsLEditor-beta.exe.bz2 LsLEditor-beta.exe
"c:\Program Files\7-Zip\7z" a -tgzip d:\temp\LsLEditor-beta.exe.gz LsLEditor-beta.exe

pause
