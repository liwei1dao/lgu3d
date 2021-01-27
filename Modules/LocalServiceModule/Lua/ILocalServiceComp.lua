ILocalServiceComp = Class.define("ILocalServiceComp",BaseModelComp);

function ILocalServiceComp:Load(_Model,...)
	self:super(ILocalServiceComp,"Load",_Model,...);
	self.ComId = 0							--需要重写
	self.Route = {}					
	self:LoadEnd()
end

function ILocalServiceComp:Start(...)
	self:super(ILocalServiceComp,"Start",...);
	LocalServiceModule.RegisterLocalService(self.ComId,self)
end


function ILocalServiceComp:Close()
    LocalServiceModule.UNRegisterLocalService(self.ComId);
    self:super(ILocalServiceComp,"Close");
end

--注册消息处理
function ILocalServiceComp:RegisterMsgDealF(mId,func)
    self.Route[mId] = func
end

--回应消息
function ILocalServiceComp:RespondMsg(mId,msg)
	if AppConfig.ProtoType == lgu3d.ProtoType.Json then
		local jsonStr = Json.encode(msg);
		LocalServiceModule.ReplyMsgsMsg(self.ComId,mId,jsonStr)
	else
		LuaServiceModule.ReplyMsgsMsg(self.ComId,mId,msg)
	end
end
--消息分发处理
function ILocalServiceComp:OnMessage(mId,buffer)
	local msg = Json.decode(buffer);
    if self.Route[mId] then
        self.Route[mId](self,msg)
    else
        print("本地服务【"..self.ComId.."】未注册服务【"..mId.."】")
    end
end
