local MsgBoxViewComp = Class.define("MsgBoxViewComp",BaseModelViewComp)

function MsgBoxViewComp:ctor(...)
	self:super(MsgBoxViewComp,"ctor",...);
end

function MsgBoxViewComp:Load(_Model,...)
    self:super(MsgBoxViewComp,"Load",_Model,...);
    self.ConfirmFun = nil
    self.CancelFun = nil
    self:AddClick("CloseButt",function (go)
        self:Hide()
    end)
    self.Msg = self:OnSubmit("Msg","Text")
    self.ConfirmButt = self:Find("Butts/ConfirmButt")
    self.CancelButt = self:Find("Butts/CancelButt")
    self:AddClick("Butts/ConfirmButt",function (go)
        self:Hide()
        if self.ConfirmFun then
            self.ConfirmFun()
        end
    end)
    self:AddClick("Butts/CancelButt",function (go)
        self:Hide()
        if self.CancelFun then
            self.CancelFun()
        end
    end)
    self:Hide()
    self:LoadEnd()
end

function MsgBoxViewComp:ShowBox(_Msg,_ConfirmFun,_CancelFun)
    self.ConfirmFun = _ConfirmFun;
    self.CancelFun = _CancelFun;
    self.Msg.text = _Msg
    self:Show()
end

function MsgBoxViewComp:ShowBoxOk(_Msg,_ConfirmFun)
    self.ConfirmFun = _ConfirmFun;
    self.Msg.text = _Msg
    self.CancelButt:Hide()
    self:Show()
end

return MsgBoxViewComp