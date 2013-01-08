@echo off
echo Copy Config/Files Begin...
copy /Y ..\Config\* ..\Debug\Login\Config\*
echo Copy Finish
PAUSE
GOTO END
:END