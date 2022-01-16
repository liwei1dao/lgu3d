using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace lgu3d
{
  public enum UILevel
  {
    LowUI,                  //一般用于app的业务逻辑界面
    NormalUI,               //一般用于比业务逻辑界面高一级的弹出框类似界面
    HightUI,                //一般用于类似与退出游街的弹窗界面
  }

  public enum UIOption
  {
    Create = 1,                  //创建界面
    Find = 2,                    //寻找界面
    Auto = 3,                    //自动 先找 找不到就创建
  }

  public class ViewManagerModule : ManagerContorBase<ViewManagerModule>
  {
    private GameObject mUIRoot;
    private GameObject mLowUIRoot;                //底优先级显示节点
    private GameObject mNormalUIRoot;             //中优先级显示节点
    private GameObject mHightUIRoot;              //高优先级显示节点

    private Camera mLowUICamera;                //底优先级显示相机
    private Camera mNormalUCamera;             //中优先级显示相机
    private Camera mHightUICamera;              //高优先级显示相机

    private Vector2 mViewSzie;                    //UI界面尺寸
    private List<ViewComp> mLowViewComps;         //底优先级UI面板
    private List<ViewComp> mNormalViewComps;      //底优先级UI面板
    private List<ViewComp> mHightViewComps;       //底优先级UI面板
    private float mMatch;                         //UI适配权重

    public GameObject LowUIRoot
    {
      get { return mLowUIRoot; }
    }

    public GameObject NormalUIRoot
    {
      get { return mNormalUIRoot; }
    }

    public GameObject HightUIRoot
    {
      get { return mHightUIRoot; }
    }

    public Camera LowUICamera
    {
      get { return mLowUICamera; }
    }

    public Camera NormalUICamera
    {
      get { return mNormalUCamera; }
    }

    public Camera HightUICamera
    {
      get { return mHightUICamera; }
    }

    public Vector2 ViewSzie
    {
      get { return mViewSzie; }
    }

    public override void Load(params object[] agr)
    {
      ResourceComp = AddComp<Module_ResourceComp>();
      base.Load(agr);
      if (agr.Length == 2)
      {
        mViewSzie = (Vector2)agr[0];
        // float lv = Mathf.Clamp(1.33333f, 1.77777f, Screen.width / Screen.height);
        // mMatch = (lv - 1.33333f) / 0.44444f;
        mMatch = (float)agr[1];
        mLowViewComps = new List<ViewComp>();
        mNormalViewComps = new List<ViewComp>();
        mHightViewComps = new List<ViewComp>();
        CreateUIRoot();
      }
      else
      {
        Debug.LogError("ViewManagerModule 启动参数错误，请检查代码");
      }
    }

    private void CreateUIRoot()
    {
      mUIRoot = GameObject.Find("UIRoot");
      if (mUIRoot == null)
      {

        mUIRoot = CreateObj("Prefab", "UIRoot", null);
        mUIRoot.name = "UIRoot";
      }
      GameObject.DontDestroyOnLoad(mUIRoot);
      mLowUIRoot = GameObject.Find("UIRoot/LowUIRoot");
      Camera _cm0 = GameObject.Find("UIRoot/LowUIRoot/Camera").GetComponent<Camera>();
      _cm0.orthographic = true;
      _cm0.clearFlags = CameraClearFlags.Depth;
      _cm0.nearClipPlane = -100;
      _cm0.farClipPlane = 100;
      _cm0.orthographicSize = 20;
      _cm0.depth = 0;
      _cm0.cullingMask = LayerMask.GetMask("UI");
      mLowUICamera = _cm0;
      Canvas _ca0 = mLowUIRoot.GetComponent<Canvas>();
      _ca0.renderMode = RenderMode.ScreenSpaceCamera;
      _ca0.worldCamera = _cm0;
      _ca0.sortingOrder = 0;
      UnityEngine.UI.CanvasScaler _cs0 = mLowUIRoot.GetComponent<UnityEngine.UI.CanvasScaler>();
      _cs0.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
      _cs0.referenceResolution = mViewSzie;
      _cs0.matchWidthOrHeight = mMatch;

      mNormalUIRoot = GameObject.Find("UIRoot/NormalUIRoot");
      Camera _cm1 = GameObject.Find("UIRoot/NormalUIRoot/Camera").GetComponent<Camera>();
      _cm1.orthographic = true;
      _cm1.clearFlags = CameraClearFlags.Depth;
      _cm1.nearClipPlane = -100;
      _cm1.farClipPlane = 100;
      _cm1.orthographicSize = 20;
      _cm1.depth = 1;
      _cm1.cullingMask = LayerMask.GetMask("UI");
      mNormalUCamera = _cm1;
      Canvas _ca1 = mNormalUIRoot.GetComponent<Canvas>();
      _ca1.renderMode = RenderMode.ScreenSpaceCamera;
      _ca1.worldCamera = _cm1;
      _ca1.sortingOrder = 1;
      UnityEngine.UI.CanvasScaler _cs1 = mNormalUIRoot.GetComponent<UnityEngine.UI.CanvasScaler>();
      _cs1.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
      _cs1.referenceResolution = mViewSzie;
      _cs1.matchWidthOrHeight = mMatch;


      mHightUIRoot = GameObject.Find("UIRoot/HightUIRoot");
      Camera _cm2 = GameObject.Find("UIRoot/HightUIRoot/Camera").GetComponent<Camera>();
      _cm2.orthographic = true;
      _cm2.clearFlags = CameraClearFlags.Depth;
      _cm2.nearClipPlane = -100;
      _cm2.farClipPlane = 100;
      _cm2.orthographicSize = 20;
      _cm2.depth = 2;
      _cm2.cullingMask = LayerMask.GetMask("UI");
      mHightUICamera = _cm2;
      Canvas _ca2 = mHightUIRoot.GetComponent<Canvas>();
      _ca2.renderMode = RenderMode.ScreenSpaceCamera;
      _ca2.worldCamera = _cm2;
      _ca2.sortingOrder = 2;
      UnityEngine.UI.CanvasScaler _cs2 = mHightUIRoot.GetComponent<UnityEngine.UI.CanvasScaler>();
      _cs2.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
      _cs2.referenceResolution = mViewSzie;
      _cs2.matchWidthOrHeight = mMatch;

      // GameObject EventSystem = mUIRoot.CreateChild("EventSystem", typeof(UnityEngine.EventSystems.EventSystem), typeof(UnityEngine.EventSystems.StandaloneInputModule));
    }

    public GameObject CreateView(ViewComp View, GameObject ViewAsset)
    {
      if (ViewAsset == null)
      {
        Debug.LogError("加载的界面 UI prefab 不存在");
        return null;
      }
      GameObject UIRoot = null;
      List<ViewComp> views = null;
      switch (View.GetLevel())
      {
        case UILevel.LowUI:
          UIRoot = LowUIRoot;
          views = mLowViewComps;
          break;
        case UILevel.NormalUI:
          UIRoot = NormalUIRoot;
          views = mNormalViewComps;
          break;
        case UILevel.HightUI:
          UIRoot = HightUIRoot;
          views = mHightViewComps;
          break;
      }
      GameObject UIGameobject = ViewAsset.CreateToParnt(UIRoot);
      RectTransform rectTrans = UIGameobject.GetComponent<RectTransform>();
      rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
      rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
      rectTrans.anchorMin = Vector2.zero;
      rectTrans.anchorMax = Vector2.one;
      Canvas canvas = UIGameobject.AddMissingComponent<Canvas>();
      UIGameobject.AddMissingComponent<GraphicRaycaster>();
      canvas.overrideSorting = true;
      View.SetCanvas(canvas);
      View.SetIndex(views.Count);
      views.Add(View);
      return UIGameobject;
    }

    public GameObject FindView(ViewComp View, string ViewName)
    {
      GameObject UIRoot = null;
      List<ViewComp> views = null;
      switch (View.GetLevel())
      {
        case UILevel.LowUI:
          UIRoot = LowUIRoot;
          views = mLowViewComps;
          break;
        case UILevel.NormalUI:
          UIRoot = NormalUIRoot;
          views = mNormalViewComps;
          break;
        case UILevel.HightUI:
          UIRoot = HightUIRoot;
          views = mHightViewComps;
          break;
      }
      GameObject UIGameobject = GameObject.Find(ViewName);
      if (UIGameobject != null)
      {
        RectTransform rectTrans = UIGameobject.GetComponent<RectTransform>();
        rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 0);
        rectTrans.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 0);
        rectTrans.anchorMin = Vector2.zero;
        rectTrans.anchorMax = Vector2.one;
        Canvas canvas = UIGameobject.AddMissingComponent<Canvas>();
        UIGameobject.AddMissingComponent<GraphicRaycaster>();
        canvas.overrideSorting = true;
        View.SetCanvas(canvas);
        View.SetIndex(views.Count);
        views.Add(View);
        return UIGameobject;
      }
      else
      {
        return null;
      }
    }



    public void SetViewToTop(ViewComp View)
    {
      List<ViewComp> views = null;
      switch (View.GetLevel())
      {
        case UILevel.LowUI:
          views = mLowViewComps;
          break;
        case UILevel.NormalUI:
          views = mNormalViewComps;
          break;
        case UILevel.HightUI:
          views = mHightViewComps;
          break;
      }
      if (View.GetIndex() == views.Count - 1)
      {
        return;
      }
      views.RemoveAt(View.GetIndex());
      views.Add(View);
      UpdataViews(View.GetLevel());
    }

    public void DeletView(ViewComp View)
    {
      switch (View.GetLevel())
      {
        case UILevel.LowUI:
          mLowViewComps.RemoveAt(View.GetIndex());
          break;
        case UILevel.NormalUI:
          mNormalViewComps.RemoveAt(View.GetIndex());
          break;
        case UILevel.HightUI:
          mHightViewComps.RemoveAt(View.GetIndex());
          break;
      }
      UpdataViews(View.GetLevel());
    }

    public void UpdataViews(UILevel Level)
    {
      List<ViewComp> views = null;
      switch (Level)
      {
        case UILevel.LowUI:
          views = mLowViewComps;
          break;
        case UILevel.NormalUI:
          views = mNormalViewComps;
          break;
        case UILevel.HightUI:
          views = mHightViewComps;
          break;
      }
      for (int i = 0; i < views.Count; i++)
      {
        views[i].SetIndex(i);
      }

    }
  }
}
