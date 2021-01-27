local ServiceManagerComp = Class.define("ServiceManagerComp",BaseModelComp)


function ServiceManagerComp:Load(MyModel,...)
    self:super(ServiceManagerComp,"Load",MyModel,...);
    self.Services = {}
    self:LoadEnd();
end

function ServiceManagerComp:RegisteredService(ComId,Service)
	if not self.Services[ComId] then
		self.Services[ComId] = {}
	end
	table.insert( self.Services[ComId], Service)
end

function ServiceManagerComp:UnRegisteredService(ComId,Service)
	if self.Services[ComId] then
		for i,v in ipairs(self.Services[ComId]) do
			if v == Service then
				table.remove(self.Services[ComId],i)
				return
			end
		end
	end
end

function ServiceManagerComp:DealMessage(ComId,MsgId,buffer)
	if self.Services[ComId] then
		for i,v in ipairs(self.Services[ComId]) do
			if v ~= nil then
				local ok = v:DealMessage(MsgId,buffer);
				if ok then return end
			end
		end
		error("消息处理组件没有注册ComId="..ComId.." MsgId = "..MsgId)
	else
		print("消息处理组件没有注册 ComId = "..ComId)
	end
end

return ServiceManagerComp;