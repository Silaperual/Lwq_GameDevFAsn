UIRootView={}
local this=UIRootView
--定义变量
local transform



--说明:
--输入参数： obj 表示UI窗体对象。
function UIRootView.Awake(obj)
    --print("------- UIRootView.Awake  -----------");
    transform=obj.transform
    --初始化面板，查找“开始按钮”
    this.InitView()

end

--初始化面板，查找“开始按钮”
function UIRootView.InitView()
    print("UIRootView.InitView")
    --查找UI中按钮
    this.BtnStart=transform:Find("Panel/BtnStart")--返回transform
    this.BtnStart=this.BtnStart:GetComponent("UnityEngine.UI.Button") --返回Button类型
end

