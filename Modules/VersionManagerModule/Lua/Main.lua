module ("VersionManagerModule", package.seeall);

--版本检测返回类型
VersionCheckReturnType =
{
	UpdateCompleted = 0,			--更新了新的资源
	VersionConsistency = 1,			--版本一致 无语更新
	EVersionLow	=2,					--服务器上版本过低 无语更新
	BigVersionLow = 3,				--大版本过低 需要下载新的app
	CheckVersionError = 4,			--监测版本错误
}

ModelControl = Class.new(require "VersionManagerModule.Module")

function New(_csobj)
	ModelControl:New(_csobj)
end

function Instance()
	return ModelControl
end

function Load(...)
	ModelControl:Load(...)
end

function LoadEnd()
	return ModelControl:GetEnd()
end

function Start(...)
	ModelControl:Start(...)
end

function Close()
	ModelControl:Close()
end