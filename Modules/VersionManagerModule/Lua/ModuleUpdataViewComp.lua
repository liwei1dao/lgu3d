local ModuleUpdataViewComp = Class.define("ModuleUpdataViewComp",BaseModelViewComp);

function ModuleUpdataViewComp:ctor(...)
    self:super(ModuleUpdataViewComp,"ctor",...);
end

function ModuleUpdataViewComp:Load(MyModel,...)
	self:super(ModuleUpdataViewComp,"Load",MyModel,...);
	self.Progress = self:OnSubmit("Progress","Slider")
	self.Describe = self:OnSubmit("Progress/Describe","Text")
	self:LoadEnd()
end

function ModuleUpdataViewComp:UpdataView(TitleStr,DescribeStr,Progress)
	self.Progress.value = Progress
	self.Describe.text = TitleStr..DescribeStr
end

function ModuleUpdataViewComp:Error(TitleStr)
	print("版本管理错误! "..TitleStr)
end

return ModuleUpdataViewComp