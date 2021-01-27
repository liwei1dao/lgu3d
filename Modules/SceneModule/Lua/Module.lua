local Module = Class.define("SceneModule",BaseModel)
function SceneModule:New(_csobj)
	self:super(Module,"New", _csobj);
	self.LoadingSteps = 100;                                           --加载进度平率
end

function Module:Load(...)
	self.CSModelObj:LoadResourceComp()
	self.ViewComp = self:AddComp("SwitchSceneViewComp",require "SceneModule.SwitchSceneViewComp","SwitchSceneView",3)
	self.ChangeSceneComp = self:AddComp("SceneChedulerComp",require "SceneModule.SceneChedulerComp")
	self:super(Module,"Load",...);
end

function Module:getter_LoadingSteps()
	return self.LoadingSteps
end

function Module:ChangeScene(SceneLoadComp)
    self.ChangeSceneComp:ChangeScene(SceneLoadComp);
end

function Module:StartLoadChanage()
	self.ViewComp:StartLoadChanage();
end 

function Module:UpdataProgress(Progress,Describe)
	self.ViewComp:UpdataProgress(Progress,Describe);
end

function Module:EndLoadChanage()
	self.ViewComp:EndLoadChanage();
end

return Module
