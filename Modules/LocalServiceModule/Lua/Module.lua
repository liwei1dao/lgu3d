local Module = Class.define("LocalServiceModule",BaseModel)
function Module:New(_csobj)
	self:super(Module,"New", _csobj);
end

function Module:Load(...)
    self.TempReceiveMsgs = {}
    self.ReceiveMsgs = {}
    self.TempReplyMsgs = {}
    self.ReplyMsgs = {}
    self.ServiceManagerComp = self:AddComp("ServiceManagerComp",require "LocalServiceModule.ServiceManagerComp")
    self.LocalServiceManagerComp = self:AddComp("LocalServiceManagerComp",require "LocalServiceModule.LocalServiceManagerComp")
	self:super(Module,"Load",...)
end

function Module:ReceiveMsg(cId,mId,msg)
   table.insert(self.TempReceiveMsgs,{ComId=cId,MsgId=mId,Msg=msg})  
end

function Module:ReplyMsgsMsg(cId,mId,msg)
    table.insert(self.TempReplyMsgs,{ComId=cId,MsgId=mId,Msg=msg})  
end

function Module:Update()
    for i,v in ipairs(self.TempReceiveMsgs) do
        table.insert(self.ReceiveMsgs,v) 
    end
    for i,v in ipairs(self.TempReplyMsgs) do
        table.insert(self.ReplyMsgs,v) 
    end
    self.TempReceiveMsgs = {}
    self.TempReplyMsgs = {}
    for i,v in ipairs(self.ReceiveMsgs) do
        self.LocalServiceManagerComp:OnMessage(v.ComId,v.MsgId,v.Msg)
    end
    for i,v in ipairs(self.ReplyMsgs) do
        self.ServiceManagerComp:DealMessage(v.ComId,v.MsgId,v.Msg)
    end
    self.ReceiveMsgs = {}
    self.ReplyMsgs = {}
 
end

return Module