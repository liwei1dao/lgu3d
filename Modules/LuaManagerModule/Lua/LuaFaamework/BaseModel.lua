ModelState = 
{
	Close = -1,         --//关闭状态
    Loading = 1,        --//加载中状态
    LoadEnd = 2,        --//加载完成状态
    Start = 3,          --//启动状态
}

BaseModel = Class.define("BaseModel")

function BaseModel:getter_State()
    return self._State;
end
function BaseModel:getter_CSObj()
    return self.CSModelObj
end

function BaseModel:_ctor()
    self._State = ModelState.Close
    self._MyComps = {};					--模块组件列表
end

function BaseModel:New(_csobj)
    self._State = ModelState.Close
	self.CSModelObj = _csobj;			--模块CS对象
end

function BaseModel:Load(...)
    --print("Model ="..self.className.." IsLoad")
    if self._State == ModelState.Close then
    	self._State = ModelState.Loading;
    	for k, v in pairs(self._MyComps) do
            --print("IsLoad CompName ="..k.." State = "..tostring(v.State))
            if v.State == ModelCompState.Close then
                v:Load(self,...);
            else
                --error("加载组件error"..k)
            end
        end
    end
    self:LoadEnd()
end

function BaseModel:Start(...)
	for k, v in pairs(self._MyComps) do
        v:Start(...);
    end
    self._State = ModelState.Start;
end

function BaseModel:LoadEnd()
    if self:GetEnd() then
        self.CSModelObj:LoadEnd()
    end
end

function BaseModel:GetEnd()        
    --print("GetEnd Model ="..self.className.." State = "..tostring(self._State))
    if self._State == ModelState.Close  then
        return false;
    end
    if self._State >= ModelState.Start then
        for k, v in pairs(self._MyComps) do
            if v.State == ModelCompState.LoadEnd then
                v:Start();
            end
        end
        return false;
    end
    for k, v in pairs(self._MyComps) do
        if v.State ~= ModelCompState.LoadEnd then
            --print("CompName ="..k.." State = "..tostring(v.State))
            return false;
        end
    end
    self._State = ModelState.LoadEnd;
    return true;
end

function BaseModel:Close()
    for k, v in pairs(self._MyComps) do
        v:Close();
    end
    self._MyComps = {}
    self.CSModelObj = nil
    self._State = ModelState.Close;
end

--添加组件
function BaseModel:AddComp(CompName,Comp,...)
    local CompObj = Class.new(Comp,...)
    if self._MyComps[CompName] then
        error("加载重复组件 CompName = "..CompName)
    end
    self._MyComps[CompName] = CompObj
    
	if self._State > ModelState.Close then
		CompObj:Load(self)
	end
    return self._MyComps[CompName]
end

--获取组件
function BaseModel:GetComp(CompName)
    return self._MyComps[CompName]
end


--移除组件
function BaseModel:RemoveComp(CompName)
	if self._MyComps[CompName] then
		self._MyComps[CompName]:Close()
	   	self._MyComps[CompName] = nil
   	end
end

----------------------------------------------------CS层接口-------------------------------------
--创建游戏对象
function BaseModel:CreateObj(BundlePath,ObjName,Parnt)
    -- print("创建对象:"..Path.." - "..ObjName)
    return self.CSModelObj:CreateObj(BundlePath,ObjName,Parnt)
end
--加载Bundle
function BaseModel:LoadBundle(BundlePath)
    print("加载Bundle  "..BundlePath)
    self.CSModelObj:LoadBundle(BundlePath)
end

function BaseModel:LoadAsset(BundlePath,AssetName)
    return self.CSModelObj:LoadAsset(BundlePath,AssetName)
end
--加载Sprite
function BaseModel:LoadAssetSprite(BundlePath,AssetName)
    return self.CSModelObj:LoadSprite(BundlePath,AssetName)
end
--加载Texture
function BaseModel:LoadAssetTexture(BundlePath,AssetName)
    return self.CSModelObj:LoadTexture(BundlePath,AssetName)
end
--加载Image
function BaseModel:LoadImage(ImageName)
   return self.CSModelObj:LoadSprite("Image",ImageName)
end
--加载AudioClip
function BaseModel:LoadAudioClip(AudioName)
    return self.CSModelObj:LoadAudioClip("Sound",AudioName)
end
--加载AnimatorController
function BaseModel:LoadAnimatorController(BundlePath,AssetName)
	return self.CSModelObj:LoadAnimatorController(BundlePath,AssetName)
end
--加载AnimatorController
function BaseModel:LoadTextAsset(BundlePath,AssetName)
	return self.CSModelObj:LoadTextAsset(BundlePath,AssetName)
end
--加载AnimatorController
function BaseModel:LoadProtoFile(BundlePath,AssetName)
	return self.CSModelObj:LoadProtoFile(BundlePath,AssetName)
end