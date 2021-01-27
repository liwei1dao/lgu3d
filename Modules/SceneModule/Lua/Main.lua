module ("SceneModule", package.seeall);
require "SceneModule.ISceneLoadCompBase";

ModelControl = Class.new(require "SceneModule.Module")

function New(_csobj)
	ModelControl:New(_csobj)
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

--------------------------------------------对外接口-------------------------------------
function ChangeScene(SceneLoadComp)
	print("跳转场景")
	ModelControl:ChangeScene(SceneLoadComp)
end
