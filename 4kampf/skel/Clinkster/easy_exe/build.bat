pushd %~dp0

set OBJ_DIR=..\..\basecode\music\
set PLAYER=..\..\basecode\music\player\
set TOOLS=..\..\basecode\tools\
set OUT_DIR=..\..\
SET TMP=temp\

if not exist %TMP% mkdir %TMP%
del /q %TMP%\*
del music_wav.exe


if not exist %OBJ_DIR% mkdir %OBJ_DIR%
del /q %OBJ_DIR%\*

if not exist %PLAYER% mkdir %PLAYER%
del /q %PLAYER%\*

tools\RenoiseConvert.exe ..\..\clinkster.xrns %TMP%\music.asm

%TOOLS%\nasmw -f win32 -I%TMP% src\clinkster.asm -o %OBJ_DIR%\clinkster.obj
%TOOLS%\nasmw -f win32 -dWRITE_WAV src\play.asm -o %PLAYER%\play.obj
%TOOLS%\crinkler20\crinkler %OBJ_DIR%\clinkster.obj^
	%PLAYER%\play.obj^
	/OUT:%OUT_DIR%\music_wav.exe^ /ENTRY:main^
	%TOOLS%\kernel32.lib %TOOLS%\user32.lib %TOOLS%\winmm.lib %TOOLS%\msvcrt_old.lib^
	@crinkler_options.txt

popd