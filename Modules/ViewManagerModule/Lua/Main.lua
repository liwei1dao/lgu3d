module ("ViewManagerModule", package.seeall);

UILevel =
{
    LowUI=1,                  --//一般用于app的业务逻辑界面
    NormalUI=2,               --//一般用于比业务逻辑界面高一级的弹出框类似界面
    HightUI=3,                --//一般用于类似与退出游街的弹窗界面
}

UIOption =
{
    Create=1,                  --//创建界面
    Find=2,                    --//寻找界面
    Auto=3,                    --//自动 先找 找不到就创建
}

ModelControl = Class.new(require "ViewManagerModule.Module")

function New(_csobj)
	ModelControl:New(_csobj)
end

function Instance()
	return ModelControl
end

function Load(...)
	ModelControl:Load(...)
end

function LoadEnd()
	return ModelControl:GetEnd()
end

function Start(...)
	ModelControl:Start(...)
end

function Close()
	ModelControl:Close()
end
