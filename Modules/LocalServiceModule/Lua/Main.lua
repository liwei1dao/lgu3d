--本地模拟服务器模块
module ("LocalServiceModule", package.seeall);
require "LocalServiceModule.IServiceBase";
require "LocalServiceModule.ILocalServiceComp";

ModelControl = Class.new(require "LocalServiceModule.Module")

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

function Update(dealTime)
	ModelControl:Update(dealTime)
end

function Close()
	ModelControl:Close()
end


--客户端消息模块使用接口
function RegisteredService(cId,IServiceBase)
	ModelControl:GetComp("ServiceManagerComp"):RegisteredService(cId,IServiceBase)
end
function UnRegisteredService(cId)
	ModelControl:GetComp("ServiceManagerComp"):UnRegisteredService(cId)
end

function ReceiveMsg(ComId,MsgId,Msg)
	ModelControl:ReceiveMsg(ComId,MsgId,Msg)
end



--虚拟服务器使用接口
function RegisterLocalService(cId,LocalSC)
    ModelControl:GetComp("LocalServiceManagerComp"):RegisterLocalService(cId,LocalSC)
end

function UNRegisterLocalService(cId)
    ModelControl:GetComp("LocalServiceManagerComp"):UNRegisterLocalService(cId)
end


function ReplyMsgsMsg(ComId,MsgId,Msg)
	ModelControl:ReplyMsgsMsg(ComId,MsgId,Msg)
end