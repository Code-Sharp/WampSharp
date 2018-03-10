@echo off 
setlocal enableDelayedExpansion 

set MYDIR=%1
for /F %%x in ('dir /B/D %MYDIR%\*.pdb') do (
  set FILENAME=%MYDIR%\%%x
  GitLink\build\GitLink.exe !FILENAME!
)