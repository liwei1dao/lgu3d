module ("SDKManagerModule", package.seeall);
ModelControl = Class.new(require "SDKManagerModule.Module")

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

function MessageReceive(msgId,data)
    ModelControl:MessageReceive(msgId,data)
end