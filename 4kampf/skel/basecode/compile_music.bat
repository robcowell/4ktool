@echo off
pushd %~dp0
set /p SYNTH=<.used_synth

echo active synth: %SYNTH%

pause

IF "%SYNTH%"=="oidos" (
	echo compiling oidos object files
	call "../oidos/easy_exe/build.bat"
)
IF "%SYNTH%"=="clinkster" (
	echo compiling clinkster object files
	call "../Clinkster/easy_exe/build.bat"
)
IF "%SYNTH%"=="vierklang" (
	echo does nothing atm, future: compile 4klang asm and inc files so they can be linked in the basecode
)
popd