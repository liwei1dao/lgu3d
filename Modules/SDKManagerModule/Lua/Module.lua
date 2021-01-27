local Module = Class.define("SDKManagerModule",BaseModel)

function Module:Load(...)
    self.MessageFunc = {}
	self:super(Module,"Load",...);
end

function Module:RegisterMessageDeal(msgId,func)
    self.MessageFunc[msgId] = func
end

function Module:MessageReceive(msgId,data)
    if self.MessageFunc[msgId] then
        self.MessageFunc[msgId](data)
    end
end

--微信
function Module:GetWeChatComp() 
    return self.CSObj:GetWeChatComp()
end
function Module:InitWeChat(wxAppId,wxAppSecret)
    return self.CSObj:InitWeChat(wxAppId,wxAppSecret)
end

--百度
function Module:GetBaiDuComp() 
    return self.CSObj:GetBaiDuComp()
end
function Module:InitBaiDu()
    return self.CSObj:InitBaiDu()
end

--闲聊
function Module:GetXianLiaoComp()
    return self.CSObj:GetXianLiaoComp()
end
function Module:InitXianLiao(xlAppId,xlAppSecret)
    return self.CSObj:InitXianLiao(xlAppId,xlAppSecret)
end

--公共接口
function Module:GetPhoneComp()
    return self.CSObj:GetPhoneComp()
end

function Module:InitPhoneSys()
    return self.CSObj:InitPhoneSys()
end

return Module