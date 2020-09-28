--引入控制层管理器脚本
require("CtrlMgr")

ProjectInit={}
local this=ProjectInit

function ProjectInit.Init()
    --导入引入项目中所有的视图层脚本
    this.ImportAllViews()
    --lua控制器初始化
    CtrlMgr.Init()
    --加载UI‘根窗体’控制脚本
    CtrlMgr.StartProcess(CtrlName.UIRootCtrl)
end

--导入引入项目中所有的视图层脚本
function ProjectInit.ImportAllViews()
    for i = 1, #ViewNames do
        require(tostring(ViewNames[i]))
    end
end
















