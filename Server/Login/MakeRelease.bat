@echo off
IF EXIST ..\Exe_Config GOTO COPYCONFIG
echo PATH ..\Exe_Config Not Found
GOTO END
:COPYCONFIG
echo Copy Config/Files Begin...
IF EXIST ..\Exe\Release\Login\Config GOTO COPYF
mkdir ..\Exe\Release\Login\Config
:COPYF
copy /Y ..\Exe_Config\* ..\Exe\Release\Login\Config\*
echo Copy Finish
PAUSE
GOTO END
:END