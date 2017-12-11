@echo off

rem AjaxMinifier.exe ‚ÌƒpƒX
if "%ProgramFiles(x86)%XXX"=="XXX" (
set AjaxMinifier="%ProgramFiles%\Microsoft\Microsoft Ajax Minifier\AjaxMinifier.exe"
) else (
set AjaxMinifier="%ProgramFiles(x86)%\Microsoft\Microsoft Ajax Minifier\AjaxMinifier.exe"
)

%AjaxMinifier% -minify:false ..\KLibraryJS\Resources\Scripts\KLibrary-vsdoc.js -o ..\KLibraryJS\Resources\Scripts\KLibrary.min.js

pause
