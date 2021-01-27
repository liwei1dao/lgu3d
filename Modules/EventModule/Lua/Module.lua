local Module = Class.define("EventModule",BaseModel)

function Module:New(_csobj)
    self:super(Module,"New", _csobj);
    self.EventSystem={}
end

function Module:Load(...)
	self:super(Module,"Load",...);
end

--添加事件 eventType-->事件类型  func-->方法
function Module:AddListener(eventType,func)
    if(eventType==nil or func==nil)then
        print('在EventSystem.AddListener中eventType或func为空')
        return
    end
    if(self.EventSystem[eventType]==nil)then
        local a={}
        table.insert(a,func)
        self.EventSystem[eventType]=a
    else
        table.insert(self.EventSystem[eventType],func)
    end
end

--移除事件
function Module:RemoveListener(eventType,func)
    if(eventType==nil or func==nil)then
        print('在EventSystem.RemoveListener中eventType或func为空')
        return
    end
    local a=self.EventSystem[eventType]
    if(a~=nil)then
        for k,v in pairs(a) do
            if(v==func)then
                a[k]=nil
            end
        end
    end
end

--派发事件
function Module:TriggerEvent(eventType,...)
    if(eventType~=nil)then
        local a=self.EventSystem[eventType]
        if(a~=nil)then
            for k,v in pairs(a) do
                v(...)
            end
        end
    end
end

return Module