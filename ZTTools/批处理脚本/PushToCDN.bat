echo off
set ftpcmd=mycmd.txt
set fph="/data/ztsy/onine_cdn/IOS/0528shenhe/99999test"
set pth=Normal
set lph1=zt_android\iOS
set lph2=zt_android\Table
set lph3=IosPackage\20190528ResList\ResourceList.txt
set fip=111.111.111.111
set user=root
set pass=********

echo lcd %pth% >> %ftpcmd%
echo cd %fph% >> %ftpcmd%
echo put -r %lph1% >> %ftpcmd%
echo put -r %lph2% >> %ftpcmd%
echo put -r %lph3% >> %ftpcmd%
echo quit >> %ftpcmd%
psftp %fip% -l %user% -pw %pass% -b %ftpcmd%  -bc

del %ftpcmd%
pause

