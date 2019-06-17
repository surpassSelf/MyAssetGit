import os
import sys
import time
 
# 设置你本地的Unity安装目录
unity_exe = 'E:\Unity\Editor\Unity.exe'

# 设置你本地的资源打包工具exe
resTool_exe = 'F:\CodeTest\ZTResourcesTools\ZTResourcesTools\\bin\Debug\ZTResourcesTools.exe'
# 设置你本地的资源路径
res_Path = 'E:\\ZTIosClient\\v1.0.2_MG_yonghengzhengtu\\Normal\\zt_android'
# 设置你本地的资源输出路径
resTarget_Path = 'E:\\ZTIosClient\\v1.0.2_MG_yonghengzhengtu\\Normal\\IosPackage'

# unity工程目录，当前脚本放在unity工程根目录中
root_path = os.getcwd()
project_path = root_path + "\ZTClient"

# 要调用的函数，注意是类名加方法名
unity_Func = "DoAssetbundle.CreateAllAssetBundles"

# 日志
log_file = os.getcwd() + '\\unity_log.log'
 
# 杀掉unity进程
def kill_unity():
    os.system('taskkill /IM Unity.exe /F')

# 根目录svn更新
def svn_Update():
    path = 'svn update ' + root_path
    os.system(path)
 
# 调用unity中我们封装的静态函数
def call_unity_static_func(func):
    kill_unity()
    time.sleep(1)
    #clear_log()
    time.sleep(1)
    print('call unity static func: ' + func)
    os.system('start ' + unity_exe + ' -projectPath ' + project_path + ' -buildTarget IOS' +  ' -logFile ' + log_file + ' -executeMethod ' + func)
    
# 调用资源打包工具
def call_ResTools():
    print("call_ResTools")
    os.system('start ' + resTool_exe + ' ' + res_Path + ' ' + resTarget_Path)
 
# 实时监测unity的log, 参数target_log是我们要监测的目标log, 如果检测到了, 则跳出while循环    
def monitor_unity_log(target_log):
    pos = 0
    while True:
        if os.path.exists(log_file):
            fd = open(log_file, 'r')
            if 0 != pos:
                fd.seek(pos, 0)
            while True:
                line = fd.readline()
                pos = pos + len(line)
                if target_log in line:
                    print(u'监测到unity输出了目标log: ' + target_log)
                    fd.close()
                    return
                if line.strip():
                    print(line)
                else:
                    break
            fd.close()
 
if __name__ == '__main__':
    svn_Update()
    call_unity_static_func(unity_Func)
    monitor_unity_log('Build AssetBundle Success!!')
    print('done')
    call_ResTools()
    os.system('@Pause')
 
