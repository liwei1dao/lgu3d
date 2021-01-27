local WaitingViewComp = Class.define("WaitingViewComp",BaseModelViewComp)

function WaitingViewComp:ctor(...)
	self:super(WaitingViewComp,"ctor",...);
end


function WaitingViewComp:Load(_Model,...)
    self:super(WaitingViewComp,"Load",_Model,...);
    self:Hide()
    self:LoadEnd()
end

function WaitingViewComp:ShowLoading(msg)
    self:Show()
end

return WaitingViewComp