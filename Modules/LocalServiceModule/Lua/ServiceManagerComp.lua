local ServiceManagerComp = Class.define("ServiceManagerComp",BaseModelComp)


function ServiceManagerComp:Load(MyModel,...)
    self:super(ServiceManagerComp,"Load",MyModel,...);
    self.Services = {}
    self:LoadEnd();
end

function ServiceManagerComp:RegisteredService(ComId,Service)
	if not self.Services[ComId] then
		self.Services[ComId] = Service
	end
end

function ServiceManagerComp:UnRegisteredService(ComId)
	if self.Services[ComId] then
		self.Services[ComId] = nil
	end
end

function ServiceManagerComp:DealMessage(ComId,MsgId,buffer)
	print("收到服务本地服务下发消息 ComId = "..ComId.." MsgId = "..MsgId.." Msg = "..buffer)
	if self.Services[ComId] then
		self.Services[ComId]:DealMessage(MsgId,buffer);
	else
		print("消息处理组件没有注册 ComId = "..ComId)
	end
end

return ServiceManagerComp;