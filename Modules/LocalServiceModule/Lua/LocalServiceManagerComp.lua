local LocalServiceManagerComp = Class.define("LocalServiceManagerComp",BaseModelComp)


function LocalServiceManagerComp:Load(MyModel,...)
    self:super(LocalServiceManagerComp,"Load",MyModel,...);
    self.LocalServices = {}
    self:LoadEnd();
end

function LocalServiceManagerComp:RegisterLocalService(cId,LocalSC)
    self.LocalServices[cId] = LocalSC
end

function LocalServiceManagerComp:UNRegisterLocalService(cId)
    self.LocalServices[cId] = nil
end

--接收本地消息分发到本地模拟服务器组件中
function LocalServiceManagerComp:OnMessage(cId,mId,msg)
    print("LocalService ComId = "..cId.." MsgId = "..mId.." Msg = "..msg)
    if self.LocalServices[cId] then
        self.LocalServices[cId]:OnMessage(mId,msg)
    else
        print("没有注册本地服务器【"..cId.."】")
    end
end

return LocalServiceManagerComp;