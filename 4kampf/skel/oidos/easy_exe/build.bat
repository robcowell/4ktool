pushd %~dp0

set OBJ_DIR=..\..\basecode\music\
set PLAYER=..\..\basecode\music\player\
set TOOLS=..\..\basecode\tools\
set OUT_DIR=..\..\
SET TMP=temp\

if not exist %TMP% mkdir %TMP%
del /q %TMP%\*
del music.exe
del music_wav.exe

if not exist %OBJ_DIR% mkdir %OBJ_DIR%
del /q %OBJ_DIR%\*

if not exist %PLAYER% mkdir %PLAYER%
del /q %PLAYER%\*


..\convert\OidosConvert.exe ..\..\oidos.xrns %TMP%\music.asm

copy ..\player\oidos.asm %TMP%
copy ..\player\random.asm %TMP%
copy ..\player\oidos.inc %TMP%
copy ..\player\play.asm %TMP%
copy music.txt %TMP%
copy wav_filename.txt %TMP%

%TOOLS%\nasmw -f win32 -I %TMP%\ %TMP%\oidos.asm -o %OBJ_DIR%\music.obj
%TOOLS%\nasmw -f win32 %TMP%\random.asm -o %OBJ_DIR%\..\random.obj
%TOOLS%\nasmw -f win32 -I %TMP%\ %TMP%\play.asm -o %PLAYER%\play.obj
%TOOLS%\nasmw -f win32 -I %TMP%\ -dWRITE_WAV %TMP%\play.asm -o %PLAYER%\play_wav.obj

%TOOLS%\crinkler20\crinkler^
	%OBJ_DIR%\music.obj^
	%OBJ_DIR%\..\random.obj^
	%PLAYER%\play.obj^
	/OUT:%OUT_DIR%\music.exe^
	/ENTRY:main %TOOLS%\kernel32.lib %TOOLS%\user32.lib %TOOLS%\winmm.lib %TOOLS%\msvcrt_old.lib^
	@crinkler_options.txt

%TOOLS%\crinkler20\crinkler^
	%OBJ_DIR%\music.obj^
	%OBJ_DIR%\..\random.obj^
	%PLAYER%\play_wav.obj^
	/OUT:%OUT_DIR%\music_wav.exe^
	/ENTRY:main %TOOLS%\kernel32.lib %TOOLS%\user32.lib %TOOLS%\winmm.lib %TOOLS%\msvcrt_old.lib^
	@crinkler_options.txt

popd
