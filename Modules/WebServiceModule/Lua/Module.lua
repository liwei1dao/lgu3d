local Module = Class.define("WebServiceModule",BaseModel);

function Module:New(_csobj)
	self:super(Module,"New", _csobj);
end

--检查网络环境
function Module:DetectNetwork()
    if Application.internetReachability == UnityEngine.NetworkReachability.NotReachable then
        return false;
    else
        return true;
    end
end

--get
function Module:Get(url, func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequest.Get(url)
        Yield(www:SendWebRequest())
        if func then
            func(www);
        end
    end)
end

--post
function Module:Post(url,data,func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequest.Post(url,data)
        Yield(www:SendWebRequest())
        if func then
            func(www);
        end
    end)
end

--下载文件
function Module:DownloadFile(url,downloadFilePathAndName, func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequest.Post(url,data)
        www.downloadHandler  = UnityEngine.Networking.DownloadHandlerFile.New(downloadFilePathAndName)
        Yield(www:SendWebRequest())
        if func then
            func(www);
        end
    end)
end

--获取Texture
function Module:GetTexture(url, func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequest.Post(url,data)
        www.downloadHandler  = UnityEngine.Networking.DownloadHandlerTexture.New(true)
        Yield(www:SendWebRequest())
        if func then
            func(www.downloadHandler.texture);
        end
    end)
end

--获取AssetBundle
function Module:GetAssetBundle(url, func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequest.Post(url,data)
        www.downloadHandler  = UnityEngine.Networking.DownloadHandlerAssetBundle.New(true)
        Yield(www:SendWebRequest())
        if func then
            func(www.downloadHandler.assetBundle);
        end
    end)
end

--获取AudioClip
function Module:GetAudioClip(url ,audioType, func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequestMultimedia.GetAudioClip(url,audioType)
        Yield(www:SendWebRequest())
        if func then
            func(DownloadHandlerAudioClip.GetContent(www));
        end
    end)
end

--上传文件
function Module:UploadByPut(url,contentBytes,contentType, func)
    StartCoroutine(function()
        local www = UnityEngine.Networking.UnityWebRequest.New()
        local uploader = UnityEngine.Networking.UploadHandlerRaw.New(contentBytes)
        uploader.contentType = contentType;
        www.uploadHandler = uploader;

        Yield(www:SendWebRequest())
        if func then
            func(www);
        end
    end)
end

return Module;