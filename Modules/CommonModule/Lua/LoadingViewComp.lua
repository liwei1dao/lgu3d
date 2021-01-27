local LoadingViewComp = Class.define("LoadingViewComp",BaseModelViewComp)

function LoadingViewComp:ctor(...)
    self:super(LoadingViewComp,"ctor",...);
end

function LoadingViewComp:Load(MyModel,...)
	self:super(LoadingViewComp,"Load",MyModel,...);
	self.Progress = self:OnSubmit("Progress","Image")
	self.Describe = self:OnSubmit("Describe","Text")
	self:LoadEnd()
end

--更新进度
function  LoadingViewComp:UpDateProgress(Describe,progress)
	self.Progress.fillAmount = progress
	self.Describe = Describe..tostring(progress*100).."%"
end

--更新下载进度
function  LoadingViewComp:UpDateDownloadProgress(taskgroup)
	self.Progress.fillAmount = taskgroup.Progress
	self.Describe = "下载进度:"..tostring(taskgroup.Progress*100).."%"
end

--加载错误
function LoadingViewComp:OnError(TitleStr,func)
    self.MyModel:GetComp("MessageBox"):ShowBoxOk(TitleStr,func)
end

return LoadingViewComp