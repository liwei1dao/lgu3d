local Module = Class.define("ViewManagerModule",BaseModel)


function Module:New(_csobj)
    self:super(Module,"New", _csobj);
    self.CSModelObj:LoadResourceComp()
    self.mUIRoot = nil
    self.mLowUIRoot = nil                --//底优先级显示节点
    self.mNormalUIRoot = nil             --//中优先级显示节点
    self.mHightUIRoot = nil              --//高优先级显示节点
    self.mLowViewComps = {};             --//底优先级UI面板
    self.mNormalViewComps = {};          --//底优先级UI面板
    self.mHightViewComps = {};           --//底优先级UI面板
    self.mViewSzie = nil                 --//UI界面尺寸
    self.mMatch = nil                    --//UI适配权重
end

function Module:getter_LowUIRoot()
    return self.mLowUIRoot;
end
function Module:getter_NormalUIRoot()
    return self.mNormalUIRoot;
end
function Module:getter_HightUIRoot()
    return self.mHightUIRoot;
end
function Module:getter_ViewSzie()
    return self.mViewSzie;
end

function Module:Load(...)
    self.mViewSzie = select(1, ...)
    self.mMatch = LuaMath.ClampByLerp(1.33333,1.77777,UnityEngine.Screen.width/UnityEngine.Screen.height)
	self:CreateUIRoot();
	self:super(Module,"Load",nil);
end

function Module:Close()
    GameObject.Destroy(self.mUIRoot);
end

function Module:CreateUIRoot()
    self.mUIRoot = GameObject.Find("UIRoot");
    if not self.mUIRoot then
        self.mUIRoot = self:CreateObj("Prefab","UIRoot",nil)
        self.mUIRoot.name = "UIRoot"
    end

    GameObject.DontDestroyOnLoad(self.mUIRoot);

    self.mLowUIRoot =  GameObject.Find("UIRoot/LowUIRoot");--self.mUIRoot:CreateChild("LowUIRoot", typeof(UnityEngine.Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
    local _cm0 =  GameObject.Find("UIRoot/LowUIRoot/Camera"):GetComponent("Camera");--self.mLowUIRoot:CreateChild("Camera", typeof(Camera)):GetComponent("Camera");
    _cm0.orthographic = true;
    _cm0.clearFlags = UnityEngine.CameraClearFlags.Depth;
    _cm0.nearClipPlane = -100;
    _cm0.farClipPlane = 100;
    _cm0.orthographicSize = 20;
    _cm0.depth = 0;
    local _ca0 = self.mLowUIRoot:GetComponent("Canvas");
    _ca0.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera;
    _ca0.worldCamera = _cm0;
    _ca0.sortingOrder = 0;
    local _cs0 = self.mLowUIRoot:GetComponent("CanvasScaler");
    _cs0.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
    _cs0.referenceResolution = self.mViewSzie;
    _cs0.matchWidthOrHeight = self.mMatch;


    self.mNormalUIRoot = GameObject.Find("UIRoot/NormalUIRoot"); --self.mUIRoot:CreateChild("NormalUIRoot", typeof(UnityEngine.Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
    local _cm1 = GameObject.Find("UIRoot/NormalUIRoot/Camera"):GetComponent("Camera");--self.mNormalUIRoot:CreateChild("Camera", typeof(Camera)):GetComponent("Camera");
    _cm1.orthographic = true;
    _cm1.clearFlags = UnityEngine.CameraClearFlags.Depth;
    _cm1.nearClipPlane = -100;
    _cm1.farClipPlane = 100;
    _cm1.orthographicSize = 20;
    _cm1.depth = 1;
    local _ca1 = self.mNormalUIRoot:GetComponent("Canvas");
    _ca1.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera;
    _ca1.worldCamera = _cm1;
    _ca1.sortingOrder = 1;
    local _cs1 = self.mNormalUIRoot:GetComponent("CanvasScaler");
    _cs1.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
    _cs1.referenceResolution = self.mViewSzie;
    _cs1.matchWidthOrHeight = self.mMatch;


    self.mHightUIRoot = GameObject.Find("UIRoot/HightUIRoot");--self.mUIRoot:CreateChild("HightUIRoot", typeof(UnityEngine.Canvas), typeof(UnityEngine.UI.CanvasScaler), typeof(UnityEngine.UI.GraphicRaycaster));
    local _cm2 = GameObject.Find("UIRoot/HightUIRoot/Camera"):GetComponent("Camera");--self.mHightUIRoot:CreateChild("Camera", typeof(Camera)):GetComponent("Camera");
    _cm2.orthographic = true;
    _cm2.clearFlags = UnityEngine.CameraClearFlags.Depth;
    _cm2.nearClipPlane = -100;
    _cm2.farClipPlane = 100;
    _cm2.orthographicSize = 20;
    _cm2.depth = 2;
    local _ca2 = self.mHightUIRoot:GetComponent("Canvas");
    _ca2.renderMode = UnityEngine.RenderMode.ScreenSpaceCamera;
    _ca2.worldCamera = _cm2;
    _ca2.sortingOrder = 2;
    local _cs2 = self.mHightUIRoot:GetComponent("CanvasScaler");
    _cs2.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
    _cs2.referenceResolution = self.mViewSzie;
    _cs2.matchWidthOrHeight = self.mMatch;

    --local EventSystem = self.mUIRoot:CreateChild("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
end

function Module:CreateView(View,ViewAsset)
    if ViewAsset == nil then
        Debug.LogError("加载的界面 UI prefab 不存在")
        return nil
    end
    local UIRoot = nil;
    local views = nil;
    if View:GetLevel() == ViewManagerModule.UILevel.LowUI then
        views = self.mLowViewComps;
        UIRoot = self.mLowUIRoot;
    elseif View:GetLevel() == ViewManagerModule.UILevel.NormalUI then
        views = self.mNormalViewComps;
        UIRoot = self.mNormalUIRoot;
    else
        views = self.mHightViewComps;
        UIRoot = self.mHightUIRoot;
    end
    local Viewobject = ViewAsset:CreateToParnt(UIRoot);
	local rectTrans = Viewobject:GetComponent("RectTransform");
    rectTrans:SetInsetAndSizeFromParentEdge(UnityEngine.RectTransform.Edge.Left, 0, 0);
    rectTrans:SetInsetAndSizeFromParentEdge(UnityEngine.RectTransform.Edge.Top, 0, 0);
    rectTrans.anchorMin = UnityEngine.Vector2.zero;
	rectTrans.anchorMax = UnityEngine.Vector2.one;
    local canvas = Viewobject:AddMissingComponent(typeof(UnityEngine.Canvas));
    Viewobject:AddMissingComponent(typeof(UnityEngine.UI.GraphicRaycaster));
	canvas.overrideSorting = true;
    View:SetCanvas(canvas);
    View:SetIndex(#views+1);
    table.insert(views,View)
    return Viewobject;
end

function Module:FindView(View,ViewName)
    local UIRoot = nil;
    local views = nil;
    if View:GetLevel() == ViewManagerModule.UILevel.LowUI then
        views = self.mLowViewComps;
        UIRoot = self.mLowUIRoot;
    elseif View:GetLevel() == ViewManagerModule.UILevel.NormalUI then
        views = self.mNormalViewComps;
        UIRoot = self.mNormalUIRoot;
    else
        views = self.mHightViewComps;
        UIRoot = self.mHightUIRoot;
    end
    local Viewobject = LuaHelpTools.Find(UIRoot,ViewName)
    if Viewobject then
        local rectTrans = Viewobject:GetComponent("RectTransform");
        rectTrans:SetInsetAndSizeFromParentEdge(UnityEngine.RectTransform.Edge.Left, 0, 0);
        rectTrans:SetInsetAndSizeFromParentEdge(UnityEngine.RectTransform.Edge.Top, 0, 0);
        rectTrans.anchorMin = UnityEngine.Vector2.zero;
        rectTrans.anchorMax = UnityEngine.Vector2.one;
        local _Canvas = Viewobject:AddMissingComponent(typeof(UnityEngine.Canvas));
        Viewobject:AddMissingComponent(typeof(UnityEngine.UI.GraphicRaycaster));
        _Canvas.overrideSorting = true;
        View:SetCanvas(_Canvas);
        View:SetIndex(#views+1);
        table.insert(views,View)
        return Viewobject
    else
        return
    end
end



function Module:SetViewToTop(View) 
    local views = nil;
    if View:GetLevel() == ViewManagerModule.UILevel.LowUI then
        views = self.mLowViewComps;
    elseif View:GetLevel() == ViewManagerModule.UILevel.NormalUI then
        views = self.mNormalViewComps;
    else
        views = self.mHightViewComps;
    end
    if View:GetIndex() == #views then
        return;
    end
    table.remove(views, View:GetIndex())
    table.insert(views,View)
    self:UpdataViews(View:GetLevel());
end

function Module:DeletView(View)
    if View:GetLevel() == ViewManagerModule.UILevel.LowUI then
        table.remove(self.mLowViewComps, View:GetIndex())
    elseif View:GetLevel() == ViewManagerModule.UILevel.NormalUI then
        table.remove(self.mNormalViewComps, View:GetIndex())
    else
        table.remove(self.mHightViewComps, View:GetIndex())
    end
    self:UpdataViews(View:GetLevel());
end

function Module:UpdataViews(Level)
    local views = nil;
    if Level == ViewManagerModule.UILevel.LowUI then
        views = self.mLowViewComps;
    elseif Level == ViewManagerModule.UILevel.NormalUI then
        views = self.mNormalViewComps;
    else
        views = self.mHightViewComps;
    end
    for i=1, #(views) do  
        views[i]:SetIndex(i);
    end 
end

return Module