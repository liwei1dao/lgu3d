local VersionContrastComp = Class.define("VersionContrastComp",BaseModelComp)


function VersionContrastComp:Load(MyModel,...)
    self:super(VersionContrastComp,"Load",MyModel,...);
    self:LoadEnd();
end

function VersionContrastComp:ContrastVersion(AssetInfoUrl,Localassetinfo,CallBackFunc,ErrorBackFunc)
    self:RequestAssetVersionInfo(AssetInfoUrl,function(SerassetInfo) 
        local UpdataAssetinfo = self:ContrastAssetInfo(Localassetinfo, SerassetInfo);
        if CallBackFunc then
            CallBackFunc(SerassetInfo,UpdataAssetinfo)
        end
    end,ErrorBackFunc);
end

function VersionContrastComp:RequestAssetVersionInfo(AssetInfoUrl,CallBackFunc,ErrorBackFunc)
    WebServiceModule.Instance():Get(AssetInfoUrl,function(www)
        if not www.isHttpError and not www.isNetworkError then
            local data = www.downloadHandler.text
            local SerassetInfo = Json.decode(data)
            if CallBackFunc then
                CallBackFunc(SerassetInfo)
            end
        else
            if ErrorBackFunc then
                CallBackFunc(www.error)
            end
        end
    end)
end


function VersionContrastComp:ContrastAssetInfo(LocalAssetInfo, ServiceAssetInfo)
    local UpdataAssetinfo = {}
    for k,v in pairs(ServiceAssetInfo.AppResInfo) do
        if LocalAssetInfo.AppResInfo[k] then
            if v.Md5 ~= LocalAssetInfo.AppResInfo[k].Md5 then
                table.insert( UpdataAssetinfo, v)
            end
        else
            table.insert( UpdataAssetinfo, v)
        end
    end
    return UpdataAssetinfo;
end


return VersionContrastComp