IServiceBase = Class.define("IServiceBase",BaseModelComp)
local pb = require "pb"
function IServiceBase:Load(_Model,...)
	self:super(IServiceBase,"Load",_Model,...);
	local comId,pbname = select(1, ...)
	self.ComId = comId						--需要重写
	if AppConfig.ProtoType == lgu3d.ProtoType.ProtoBuff then
		assert(protoc:loadfile(self.MyModel.className.."."..pbname))
	end
	self.pbMap = {}							--pb 消息命令对应表
	self.MessageDeals = {}					--消息处理
	self.MessageResDeals = {}				--消息回应处理
	self:LoadEnd()
end

function IServiceBase:Start(...)
	self:super(IServiceBase,"Start",...);
	LuaServiceModule.RegisteredService(self.ComId,self)
end

function IServiceBase:Close()
	LuaServiceModule.UnRegisteredService(self.ComId,self)
	self:super(IServiceBase,"Close");
end

--加载pb消息对应表
function IServiceBase:LoadPbMap(map)
	if map then 
		for k,v in pairs(map) do
			self.pbMap[k] = v
		end
	end
end

function IServiceBase:RegisterMDeal(_MsgId,_DealFunc)
	self.MessageDeals[_MsgId] = {
		MsgId = _MsgId,
		DealFunc = _DealFunc
	}
end

function IServiceBase:DealMessage(MsgId,buffer)
	local msg = nil
	if self.MessageResDeals[MsgId] then
		if AppConfig.ProtoType == lgu3d.ProtoType.Json then
			msg = Json.decode(buffer)
			if self.ComId ~= 0 then
				print("收到消息 ComId = "..self.ComId.." MsgId = "..MsgId.." Msg = "..tostring(msg))
			end
		else 
			if not self.pbMap[MsgId] then
				error("没有对应的pb Map:"..MsgId)
				return false
			end
			msg = assert(pb.decode(self.pbMap[MsgId], buffer))
			if self.ComId ~= 0 then
				print("收到消息 ComId = "..self.ComId.." MsgId = "..MsgId.." Msg = "..serpent.block(msg))
			end
		end
		self.MessageResDeals[MsgId].Resfunc(self,msg)
		self.MessageResDeals[MsgId] = nil
		return true
	end

	if self.MessageDeals[MsgId] then
		if AppConfig.ProtoType == lgu3d.ProtoType.Json then
			msg = Json.decode(buffer)
			if self.ComId ~= 0 then
				print("收到消息 ComId = "..self.ComId.." MsgId = "..MsgId.." Msg = "..tostring(msg))
			end
		else
			if not self.pbMap[MsgId] then
				error("没有对应的pb Map:"..MsgId)
				return false
			end
			msg = assert(pb.decode(self.pbMap[MsgId], buffer))
			if self.ComId ~= 0 then
				print("收到消息 ComId = "..self.ComId.." MsgId = "..MsgId.." Msg = "..serpent.block(msg))
			end
		end
		self.MessageDeals[MsgId].DealFunc(self,msg)
		return true
	end
	return false
end


function IServiceBase:Send(MsgId,Msg)
	local msg
	if AppConfig.ProtoType == lgu3d.ProtoType.Json then
		msg = Json.encode(Msg)
		if self.ComId ~= 0 then
			print("发送消息 ComId = "..self.ComId.." MsgId = "..MsgId.." Msg = "..tostring(msg))
		end
	else
		if not self.pbMap[MsgId] then
			error("没有对应的pb Map:"..MsgId)
			return
		end

		msg = assert(pb.encode(self.pbMap[MsgId], Msg)) --Msg:SerializeToString()
		if self.ComId ~= 0 then
			print("发送消息 ComId = "..self.ComId.." MsgId = "..MsgId.." Msg = "..pb.tohex(msg))
		end
	end
	LuaServiceModule.Send(self.ComId,MsgId,msg)
end

function IServiceBase:SendRes(MsgId,Msg,_ResMsgId,_Resfunc)
	self.MessageResDeals[_ResMsgId] = {
		Resfunc = _Resfunc
	}
	self:Send(MsgId,Msg)
end