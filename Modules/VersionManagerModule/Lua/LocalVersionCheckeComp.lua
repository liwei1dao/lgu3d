local LocalVersionCheckeComp = Class.define("LocalVersionCheckeComp",BaseModelComp)


function LocalVersionCheckeComp:Load(MyModel,...)
    self:super(LocalVersionCheckeComp,"Load",MyModel,...);
    self:LoadEnd();
end

 --校验本地环境
function LocalVersionCheckeComp:CheckeLocalVersion(LoadingView,CallBackFunc)
    local localVersionPath = AppConfig.AppAssetBundleAddress .. "/VersionInfo.json"
    local localAssetPath = AppConfig.AppAssetBundleAddress .. "/AssetInfo.json"
    local IsSucc = FilesTools.IsKeepFileOrDirectory(localVersionPath)
    if not IsSucc then --外部资源不存在 进行版本迁移处理
        StartCoroutine(self.AssemblyAsset,self,LoadingView,function()
            local versionstr = FilesTools.ReadFileToStr(localVersionPath)
            local assetinfostr = FilesTools.ReadFileToStr(localAssetPath)
            local LocalVersion = Json.decode(versionstr)
            local LocalAssetInfo = Json.decode(assetinfostr)
            if CallBackFunc then
                CallBackFunc(LocalVersion, LocalAssetInfo)
            end
        end)
    else
        local versionstr = FilesTools.ReadFileToStr(localVersionPath)
        local assetinfostr = FilesTools.ReadFileToStr(localAssetPath)
        local LocalVersion = Json.decode(versionstr)
        local LocalAssetInfo = Json.decode(assetinfostr)
        self:CheckeAppInside(LoadingView,LocalVersion, LocalAssetInfo, CallBackFunc)
    end
end

function LocalVersionCheckeComp:CheckeAppInside(LoadingView,LocalVersion,LocalAssetInfo,CallBackFunc)
    local AppVersion = tonumber(Application.version)
    if LocalVersion.ProVersion < AppVersion  then      --外部资源为上一个版本的资源文件，需要重新覆盖
        print("校验本部资源本版高于外部资源版本 insideVer:"..AppVersion.."  externalVer:"..LocalVersion.ProVersion)
        StartCoroutine(self.AssemblyAsset,self,LoadingView,function()
            local localVersionPath = AppConfig.AppAssetBundleAddress .. "/VersionInfo.json"
            local localAssetPath = AppConfig.AppAssetBundleAddress .. "/AssetInfo.json"
            local versionstr = FilesTools.ReadFileToStr(localVersionPath)
            local assetinfostr = FilesTools.ReadFileToStr(localAssetPath)
            local LocalVersion = Json.decode(versionstr)
            local LocalAssetInfo = Json.decode(assetinfostr)
            if CallBackFunc then
                CallBackFunc(LocalVersion, LocalAssetInfo)
            end
        end)
    else
        if CallBackFunc then
            CallBackFunc(LocalVersion, LocalAssetInfo)
        end
    end
end

function LocalVersionCheckeComp:AssemblyAsset(LoadingView,Func)
    local www =  UnityEngine.Networking.UnityWebRequest.New(AppConfig.GetstreamingAssetsPath .. "/Res.zip")
    local dowle = UnityEngine.Networking.DownloadHandlerBuffer.New()
    www.downloadHandler = dowle
    Yield(www:SendWebRequest())
    if www.error then
        error("读取本地资源文件失败:"..AppConfig.GetstreamingAssetsPath .. "/Res.zip")
    else
        Yield(ZipTools.UnzipFile(dowle.data, AppConfig.AppAssetBundleAddress, AppConfig.ResZipPassword,lgu3d.ZipTools.ZipOpeProgress(function(Describe,Progress)
            LoadingView:UpDateProgress("初次运行解压资源文件"..Describe,Progress)
        end)))  
        if Func then
            Func()
        end
        www:Dispose()
    end
end

--校验本地模块资源信息文件
function LocalVersionCheckeComp:CheckLoadModule(ModuleName,CallBackFunc)
    local localmodulepath = AppConfig.AppAssetBundleAddress .."/"..ModuleName.. "/AssetInfo.json"
    local IsSucc = FilesTools.IsKeepFileOrDirectory(localmodulepath)
    if IsSucc then
        local assetinfostr = FilesTools.ReadFileToStr(localmodulepath)
        local LocalAssetInfo = Json.decode(assetinfostr)
        if CallBackFunc then
            CallBackFunc(nil, LocalAssetInfo)
        end
    else
        local LocalAssetInfo = {AppResInfo={}}
        if CallBackFunc then
            CallBackFunc(nil, LocalAssetInfo)
        end
    end
end

return LocalVersionCheckeComp