local Module = Class.define("VersionManagerModule",BaseModel);

function Module:New(_csobj)
	require "VersionManagerModule.DownloadTask"
	self:super(Module,"New", _csobj);
end

function Module:Load(...)
	self.ResServiceUrl = select(1, ...)
	print("启动版本管理服务:"..self.ResServiceUrl)
	self.CSModelObj:LoadResourceComp()
	self.CurrPlatform = "Windows" 
	if AppConfig.TargetPlatform == lgu3d.AppPlatform.Windows then
		self.CurrPlatform = "Windows" 
	elseif AppConfig.TargetPlatform == lgu3d.AppPlatform.Android then
		self.CurrPlatform = "Android" 
	else
		self.CurrPlatform = "IOS" 
	end
	self.LocalCheckeComp = self:AddComp("LocalVersionCheckeComp",require "VersionManagerModule.LocalVersionCheckeComp");
	self.VersionContrastComp = self:AddComp("VersionContrastComp",require "VersionManagerModule.VersionContrastComp");
	self.VersionDownlooadComp = self:AddComp("VersionDownlooadComp",require "VersionManagerModule.VersionDownlooadComp");
	self:super(Module,"Load", ...);
end

--启动app版本校验
function Module:StartAppVersionCheck(LoadingView,CompleteFun)
    self.LocalCheckeComp:CheckeLocalVersion(LoadingView,function(LocalVersion,LocalAssetInfo)
		local Url = self.ResServiceUrl ..'/'..self.CurrPlatform .. "/VersionInfo.json";
		WebServiceModule.Instance():Get(Url,function(www)
			if not www.isHttpError and not www.isNetworkError then
				local data = www.downloadHandler.text
				Debug.Log("版本信息："..data)
				local ServiceVersion = Json.decode(data)
				if ServiceVersion.ProVersion <= LocalVersion.ProVersion then
					if ServiceVersion.ResVersion >= LocalVersion.ResVersion then
						local AssetUrl = self.ResServiceUrl..'/'..self.CurrPlatform .. "/AssetInfo.json";
						self.VersionContrastComp:ContrastVersion(AssetUrl,LocalAssetInfo,function(sassetinfos,upres)
							Debug.Log("检查更新结果"..LuaTableTools.TableToStr(upres))
							if upres and #upres > 0 then
								local tsdks = {}
								for i,v in ipairs(upres) do
									local DownloadTask = Class.new(DownloadTask,
									v.Id,
									self.ResServiceUrl..'/'..self.CurrPlatform .. "/" .. v.Id,
									AppConfig.AppAssetBundleTemp .. "/" .. v.Id,
									v.Size)
									table.insert(tsdks,DownloadTask)
								end
								self.VersionDownlooadComp:StartDownload(tsdks,
								function(TasksGroup)
									LoadingView:UpDateDownloadProgress(TasksGroup)
								end,
								function(TasksGroup, Task)
									local ResId = Task.Id;
									local ResPath = AppConfig.AppAssetBundleTemp .. "/" .. ResId;
									local TargetPath = AppConfig.AppAssetBundleAddress .. "/" .. ResId;
									FilesTools.CopyFile(ResPath, TargetPath);
									LocalAssetInfo.AppResInfo[ResId] = sassetinfos.AppResInfo[ResId];
									local AssetInfoStr  = Json.encode(LocalAssetInfo)
									FilesTools.WriteStrToFile(AppConfig.AppAssetBundleAddress .. "/AssetInfo.json", AssetInfoStr);
									LuaHelpTools.LoadAppAssetInfo()
									if TasksGroup.IsComp then
										local VersionStr = Json.encode(ServiceVersion)
										FilesTools.WriteStrToFile(AppConfig.AppAssetBundleAddress .. "/VersionInfo.json", VersionStr);
										FilesTools.ClearDirectory(AppConfig.AppAssetBundleTemp);
										if CompleteFun then
											CompleteFun(VersionManagerModule.VersionCheckReturnType.UpdateCompleted)
										end
									end
								end,
								function(TasksGroup,error)
									if CompleteFun then
										CompleteFun(VersionManagerModule.VersionCheckReturnType.CheckVersionError)
									end
								end)
							else
								if CompleteFun then
									CompleteFun(VersionManagerModule.VersionCheckReturnType.VersionConsistency)
								end
							end
						end,function(err)
							LoadingView:OnError("请求资源服务器失败 url:"..AssetUrl.." err: "..www.error,function()
								if CompleteFun then
									CompleteFun(VersionManagerModule.VersionCheckReturnType.CheckVersionError)
								end
							end)
						end)
					else
						if CompleteFun then
							CompleteFun(VersionManagerModule.VersionCheckReturnType.EVersionLow)
						end
					end
				else
					if CompleteFun then
						CompleteFun(VersionManagerModule.VersionCheckReturnType.BigVersionLow)
					end
				end
			else
				LoadingView:OnError("请求资源服务器失败 url:"..Url.." err: "..www.error,function()
					if CompleteFun then
						CompleteFun(VersionManagerModule.VersionCheckReturnType.CheckVersionError)
					end
				end)
			end

		end)
	end)
end

--启动模块版本校验
function Module:StartModuleVersionCheck(LoadingView,ModuleName,CompleteFun)
	self.LocalCheckeComp:CheckLoadModule(ModuleName,function(LocalVersion,LocalAssetInfo)
		local AssetUrl = self.ResServiceUrl ..self.CurrPlatform .."/" ..string.lower(ModuleName).."/AssetInfo.json";
		self.VersionContrastComp:ContrastVersion(AssetUrl,LocalAssetInfo,function(sassetinfos,upres)
			Debug.Log("检查更新结果"..LuaTableTools.TableToStr(upres))
			if upres and #upres > 0 then
				local tasks = {}
				for i,v in ipairs(upres) do
					local DownloadTask = Class.new(DownloadTask,
					v.Id,
					self.ResServiceUrl ..self.CurrPlatform .. "/" .. v.Id,
					AppConfig.AppAssetBundleTemp .. "/" .. v.Id,
					v.Size)
					table.insert(tasks,DownloadTask)
				end
				self.VersionDownlooadComp:StartDownload(tasks,
				function(TasksGroup)
					LoadingView:UpDateDownloadProgress(TasksGroup)
				end,
				function(TasksGroup, Task)
					local ResId = Task.Id;
					local ResPath = AppConfig.AppAssetBundleTemp .. "/" .. ResId;
					local TargetPath = AppConfig.AppAssetBundleAddress .. "/" .. ResId;
					FilesTools.CopyFile(ResPath, TargetPath);
					LocalAssetInfo.AppResInfo[ResId] = sassetinfos.AppResInfo[ResId];
					local AssetInfoStr  = Json.encode(LocalAssetInfo)
					FilesTools.WriteStrToFile(AppConfig.AppAssetBundleAddress .."/"..ModuleName.. "/AssetInfo.json", AssetInfoStr);
					if TasksGroup.IsComp then
						FilesTools.ClearDirectory(AppConfig.AppAssetBundleTemp);
						if CompleteFun then
							CompleteFun(VersionManagerModule.VersionCheckReturnType.UpdateCompleted)
						end
					end
				end,
				function(TasksGroup,error)
					if CompleteFun then
						CompleteFun(VersionManagerModule.VersionCheckReturnType.CheckVersionError)
					end
				end)
			else
				if CompleteFun then
					CompleteFun(VersionManagerModule.VersionCheckReturnType.VersionConsistency)
				end
			end
		end,function(err)
			LoadingView:OnError("请求资源服务器失败 url:"..AssetUrl.." err: "..www.error,function()
				if CompleteFun then
					CompleteFun(VersionManagerModule.VersionCheckReturnType.CheckVersionError)
				end
			end)
		end)
	end)
end

return Module;