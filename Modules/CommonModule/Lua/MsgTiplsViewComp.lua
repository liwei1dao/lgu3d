local MsgTiplsViewComp = Class.define("MsgTiplsViewComp",BaseModelViewComp)

function MsgTiplsViewComp:ctor(...)
	self:super(MsgTiplsViewComp,"ctor",...);
end


function MsgTiplsViewComp:Load(_Model,...)
    self:super(MsgTiplsViewComp,"Load",_Model,...);
    self:Hide()
    self:LoadEnd()
end

function MsgTiplsViewComp:ShowTipls(msg,time)
    
end


return MsgTiplsViewComp