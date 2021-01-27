IServiceBase = Class.define("IServiceBase",BaseModelComp)

function IServiceBase:Load(_Model,...)
	self:super(IServiceBase,"Load",_Model,...);
	self.ComId = 0							--需要重写
	self.MessageDeals = {}					--消息处理
	self.MessageResDeals = {}				--消息回应处理
	self:LoadEnd()
end

function IServiceBase:Start(...)
	self:super(IServiceBase,"Start",...);
	LocalServiceModule.RegisteredService(self.ComId,self)
end

function IServiceBase:Close()
	self:super(IServiceBase,"Close");
	LocalServiceModule.UnRegisteredService(self.ComId)
end

function IServiceBase:RegisterMDeal(_MsgId,_Pd,_DealFunc)
	self.MessageDeals[_MsgId] = {
		MsgId = _MsgId,
		Pd = _Pd,
		DealFunc = _DealFunc
	}
end

function IServiceBase:DealMessage(MsgId,buffer)
	if self.MessageResDeals[MsgId] then
		if AppConfig.ProtoType == lgu3d.ProtoType.Json then
			self.MessageResDeals[MsgId].Pd = Json.decode(buffer)
		else 
			self.MessageResDeals[MsgId].Pd:ParseFromString(buffer)
		end
		self.MessageResDeals[MsgId].Resfunc(self.MessageResDeals[MsgId].Pd)
		self.MessageResDeals[MsgId] = nil
		return
	end

	if self.MessageDeals[MsgId] then
		if AppConfig.ProtoType == lgu3d.ProtoType.Json then
			self.MessageDeals[MsgId].Pd = Json.decode(buffer)
		else
			self.MessageDeals[MsgId].Pd:ParseFromString(buffer)
		end
		self.MessageDeals[MsgId].DealFunc(self,self.MessageDeals[MsgId].Pd)
	else
		error("DealMessage Error MsgId "..MsgId.." 没有注册处理函数")
	end
end

function IServiceBase:Send(MsgId,Msg)
	if AppConfig.ProtoType == lgu3d.ProtoType.Json then
		local jsonStr = Json.encode(Msg);
		LocalServiceModule.ReceiveMsg(self.ComId,MsgId,jsonStr)
	else
		LuaServiceModule.ReceiveMsg(self.ComId,MsgId,Msg)
	end
	
end

function IServiceBase:SendRes(MsgId,Msg,_ResMsgId,_Resfunc)
	self.MessageResDeals[_ResMsgId] = {
		Resfunc = _Resfunc
	}
	self:Send(MsgId,Msg)
end
