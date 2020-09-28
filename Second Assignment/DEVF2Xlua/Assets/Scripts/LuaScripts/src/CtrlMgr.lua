--引入控制层管理器脚本
require("UIRootCtrl")
require("TaskCtrl")

CtrlMgr={}
local this=CtrlMgr
--定义一个控制器列表（缓存所有项目中用到的所有控制层lua脚本）
local ctrlList={}

--lua 控制器初始化(缓存所有项目中控制层lua脚本)
function CtrlMgr.Init()
    ctrlList[CtrlName.UIRootCtrl]=UIRootCtrl.GetInstance() --得到脚本的实例
    ctrlList[CtrlName.TaskCtrl]=TaskCtrl.GetInstance()
end

--获取控制器lua脚本
function CtrlMgr.GetCtrlInstance(ctrlName)
    return ctrlList[ctrlName]
end

--获取控制器lua脚本实例，且调用StartProcess函数。
function CtrlMgr.StartProcess(ctrlName)
    local ctrlObj=CtrlMgr.GetCtrlInstance(ctrlName)
    if(ctrlObj~=nil) then
        ctrlObj.StartProcess()
    end
end


















