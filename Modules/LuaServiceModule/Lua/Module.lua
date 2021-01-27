local Module = Class.define("LuaServiceModule",BaseModel)

function Module:Load(...)
	local Ip,Prot,_,IsView = select(1, ...)
	self.ServicesComp = self:AddComp("ServiceManagerComp",require "LuaServiceModule.ServiceManagerComp")
	-- self.MessageComp = self:AddComp("SytemMessageComp",require "LuaServiceModule.SytemMessageComp")
	if IsView then
		self.CSModelObj:LoadResourceComp()
		self.ServiceModuleViewComp = self:AddComp("ServiceModuleViewComp",require "LuaServiceModule.ServiceModuleViewComp","ServiceModuleView")
	end
	self:super(Module,"Load",...);
end


function Module:Connect(Addr,Port,succFunc,failFunc)
	self.succFunc = succFunc
	self.failFunc = failFunc
	self.CSObj:Connect(Addr,Port)
end

function Module:ScoketConnectNote()
	
	if self.succFunc then
		self.succFunc()
	end
end

function Module:Disconnect()
	self.CSObj:Disconnect()
end

function Module:ScoketDisconnectNote()
	if self.failFunc then
		self.failFunc()
	end
end


function Module:RegisteredService(ComId,Service)
	self.ServicesComp:RegisteredService(ComId,Service);
end

function Module:UnRegisteredService(ComId,Service)
	self.ServicesComp:UnRegisteredService(ComId,Service);
end

--处理消息
function Module:DealMessage(ComId,MsgId,buffer)
	self.ServicesComp:DealMessage(ComId,MsgId,buffer)
end

function Module:Send(ComId,MsgId,Msg)
	self.CSObj:SendMessage(ComId,MsgId,Msg)
end

return Module