import os
import sys
import time
 
# 设置你本地的资源打包工具exe
resTool_exe = 'F:\CodeTest\ZTResourcesTools\ZTResourcesTools\\bin\Debug\ZTResourcesTools.exe'
# 设置你本地的资源路径
res_Path = 'E:\\ZTIosClient\\v1.0.2_MG_yonghengzhengtu\\Normal\\zt_android'
# 设置你本地的资源输出路径
resTarget_Path = 'E:\\ZTIosClient\\v1.0.2_MG_yonghengzhengtu\\Normal\\IosPackage'
# unity工程目录，当前脚本放在unity工程根目录中
root_path = os.getcwd()
project_path = root_path + "\ZTClient"

# 根目录svn更新
def svn_Update():
    path = 'svn update ' + root_path
    os.system(path)
    
# 调用资源打包工具
def call_ResTools():
    print("call_ResTools")
    os.system('start ' + resTool_exe + ' ' + res_Path + ' ' + resTarget_Path)
  
if __name__ == '__main__':
    svn_Update()
    call_ResTools()
    os.system('@Pause')
 
