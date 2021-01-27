BaseModelViewComp = Class.define("BaseModelViewComp",BaseModelComp);

function BaseModelViewComp:ctor(...)
	self.ViewName,self.UILevel,self.UIOption = select("1",...)
	if not self.UILevel then
		self.UILevel = ViewManagerModule.UILevel.LowUI;
	end
	if not self.UIOption then
		self.UIOption = ViewManagerModule.UIOption.Create;
	end

	self:super(BaseModelViewComp,"ctor",...);
end

function BaseModelViewComp:Load(_Model,...)
	self:super(BaseModelViewComp,"Load",_Model,...)
	if self.UIOption == ViewManagerModule.UIOption.Create then
		local asset = self.MyModel:LoadAsset("Prefab", self.ViewName)
		local UIobj = ViewManagerModule.Instance():CreateView(self,asset)
		self._UIGameObject = Class.new(LuaGameObject,UIobj)
	elseif self.UIOption == ViewManagerModule.UIOption.Find then
		local UIobj = ViewManagerModule.Instance():FindView(self,self.ViewName)
		if UIobj then
			self._UIGameObject = Class.new(LuaGameObject,UIobj)
		else
			Debug.LogError("查找界面失败:"..self.ViewName)
		end
	else
		local UIobj = ViewManagerModule.Instance():FindView(self,self.ViewName)
		if not UIobj then
			local asset = self.MyModel:LoadAsset("Prefab", self.ViewName)
			if asset then
				UIobj = ViewManagerModule.Instance():CreateView(self,asset)
			else
				error("加载UI界面失败:"..self.ViewName)
			end
		end
		if UIobj then
			UIobj.name = self.ViewName
			self._UIGameObject = Class.new(LuaGameObject,UIobj)
		end
	end
	if self._UIGameObject then
		ViewManagerModule.Instance():SetViewToTop(self);
	end
end

function BaseModelViewComp:Start(...)
	self:super(BaseModelViewComp,"Start",...)
end

function BaseModelViewComp:Close()
	ViewManagerModule.Instance():DeletView(self);
	self._UIGameObject:Destroy()
	self:super(BaseModelViewComp,"Close")
end

function BaseModelViewComp:getter_UIGameObject()
	return self._UIGameObject
end

function BaseModelViewComp:GetIndex()
	return self.Index;
end

function BaseModelViewComp:SetIndex(_Index)
	self.Index = _Index;
	self.Canvas.sortingOrder = _Index;
end

function BaseModelViewComp:GetLevel()
	return self.UILevel
end

function BaseModelViewComp:SetCanvas(_Canvas)
	self.Canvas = _Canvas;
end

function BaseModelViewComp:Show()
	ViewManagerModule.Instance():SetViewToTop(self);
	self._UIGameObject:Show()
end

function BaseModelViewComp:Hide()
	self._UIGameObject:Hide()
end

--查找子对象
function BaseModelViewComp:Find(target)
    return self._UIGameObject:Find(target)
end

--创建子对象
function BaseModelViewComp:CreateObj(BundlePath,ObjName,Parnt)
	local obj = self.MyModel:CreateObj(BundlePath,ObjName,Parnt)
	return Class.new(LuaGameObject,obj)
end

--查找子对象
function BaseModelViewComp:FindObj(target)
    return self._UIGameObject:FindObj(target)
end

--获取子对象的组件
function BaseModelViewComp:OnSubmit(target,CompName)
	return self._UIGameObject:OnSubmit(target,CompName)
end

--添加单击事件
function BaseModelViewComp:AddClick(target,func)
	self._UIGameObject:AddClick(target,func)
end

--添加事件
function BaseModelViewComp:AddEvent(target,eventtype,func)
	self._UIGameObject:AddEvent(target,eventtype,func)
end
