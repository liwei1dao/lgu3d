local Module = Class.define("CommonModule",BaseModel)

function Module:New(_csobj)
	self:super(Module,"New", _csobj);
end

function Module:Load(...)
    self.CSModelObj:LoadResourceComp()
    self:super(Module,"Load", ...);
    self:AddComp("MessageBox",require "CommonModule.MsgBoxViewComp","MessageBox",ViewManagerModule.UILevel.HightUI,ViewManagerModule.UIOption.Auto)
    self:AddComp("Waiting",require "CommonModule.WaitingViewComp","WaitingView",ViewManagerModule.UILevel.HightUI,ViewManagerModule.UIOption.Auto)
    self:AddComp("MsgTipls",require "CommonModule.MsgTiplsViewComp","MessageTipls",ViewManagerModule.UILevel.HightUI,ViewManagerModule.UIOption.Auto)
    self:AddComp("Loading",require "CommonModule.LoadingViewComp","LoadingView",ViewManagerModule.UILevel.HightUI,ViewManagerModule.UIOption.Auto)
    self:LoadEnd()
end

return Module