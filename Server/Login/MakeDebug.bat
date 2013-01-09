@echo off
IF EXIST ..\Exe_Config GOTO COPYCONFIG
echo PATH ..\Exe_Config Not Found
GOTO END
:COPYCONFIG
echo Copy Config/Files Begin...
IF EXIST ..\Exe\Debug\Login\Config GOTO COPYF
mkdir ..\Exe\Debug\Login\Config
:COPYF
copy /Y ..\Exe_Config\* ..\Exe\Debug\Login\Config\*
echo Copy Finish
PAUSE
GOTO END
:END