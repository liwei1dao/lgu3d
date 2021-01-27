module ("LuaServiceModule", package.seeall);
require "LuaServiceModule.IServiceBase";

ModelControl = Class.new(require "LuaServiceModule.Module")

function New(_csobj)
	ModelControl:New(_csobj)
end

function Load(Ip,Prot,IsView)
	ModelControl:Load(Ip,Prot,IsView)
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

function Instance()
	return ModelControl
end

function RegisteredService(ComId,Service)
	ModelControl:RegisteredService(ComId,Service)
end
function UnRegisteredService(ComId,Service)
	ModelControl:UnRegisteredService(ComId,Service)
end
function Send(ComId,MsgId,Msg)
	ModelControl:Send(ComId,MsgId,Msg)
end
-------------------------------------------------------------------------------------------

--连接服务器
function Connect(Addr,Port,succFunc,failFunc)
	ModelControl:Connect(Addr,Port,succFunc,failFunc)
end

--链接成功回调
function ScoketConnectNote()
	ModelControl:ScoketConnectNote()
end


function Disconnect()
	ModelControl:Disconnect()
end

--断开连接回调
function ScoketDisconnectNote()
	ModelControl:ScoketDisconnectNote()
end

function DealMessage(ComId,MsgId,buffer)  
	ModelControl:DealMessage(ComId,MsgId,buffer)
end


