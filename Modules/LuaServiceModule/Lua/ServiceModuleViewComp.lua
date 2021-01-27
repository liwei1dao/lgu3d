local ServiceModuleViewComp = Class.define("ServiceModuleViewComp",BaseModelViewComp);

function ServiceModuleViewComp:ctor(...)
	self:super(ServiceModuleViewComp,"ctor",...);
end

function ServiceModuleViewComp:Load(_Model,...)
	local Ip,Prot = select(1, ...)
	self.UILevel = ViewManagerModule.UILevel.NormalUI;
	self:super(ServiceModuleViewComp,"Load",_Model,...);
	self.IpInput = self:OnSubmit("Ip_Input","InputField")
	self.ProtInput = self:OnSubmit("Prot_Input","InputField")
	self:AddClick("Connet_Butt",function (go)
		local ip = self.IpInput.text
		local prot = self.ProtInput.text
		-- if not self.MyModel:Connect(ip,prot) then
		-- 	print("连接服务器失败")
		-- end
	end)
	self:AddClick("Off_Butt",function (go)
		
    end)

	self.IpInput.text = Ip
	self.ProtInput.text = Prot
	self:LoadEnd()
end

return ServiceModuleViewComp