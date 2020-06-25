wpeinit
echo off
color 0c
cls
echo ---------Vitejte v automaticke instalaci ucebny (AutoIU)---------
echo                 --Vytvoril Dominik Hejl--
echo:
echo:
echo ---kontrola sitove karty---
echo:
echo ----------------------------------------------------------
ipconfig
echo ----------------------------------------------------------
echo:
echo ---pripojeni slozky s instalaci windows---
echo:
echo ----------------------------------------------------------
net use a: \\10.0.2.15\windows /user: ""
echo ----------------------------------------------------------
echo:
echo ---spousteni instalace---
"a:\setup.exe" /Unattend:autounattend.xml
pause
